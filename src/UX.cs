using HarmonyLib;

namespace PaleOilSoap
{
    // Casting to InventoryItemNail is not ideal but don't see an alternative
    // as InventoryItemNail does not override the desired methods
    [HarmonyPatch(typeof(InventoryItemSelectable))]
    internal static class UX
    {
        [HarmonyPostfix, HarmonyPatch(typeof(InventoryItemNail), nameof(InventoryItemNail.Start))]
        private static void InventoryItemNail_Start(InventoryItemNail __instance)
        {
            Plugin.Logger.LogWarning(__instance is InventoryItemNail);

            InventoryPaneList root = __instance.GetComponentInParent<InventoryPaneList>();
            if (root == null) {
                Plugin.Logger.LogWarning($"Tried to set up button prompts but parent {nameof(InventoryPaneList)} could not be found!");
                return;
            }

            InventoryItemToolManager tools = root.GetComponentInChildren<InventoryItemToolManager>(true);
            if (tools == null) {
                Plugin.Logger.LogWarning($"Tried to set up button prompts but sibling {nameof(InventoryItemToolManager)} could not be found!");
                return;
            }

            InventoryItemButtonPrompt copy = __instance.GetComponent<InventoryItemButtonPrompt>();
            if (copy == null) {
                Plugin.Logger.LogWarning($"Tried to set up button prompts but {nameof(InventoryItemNail)} did not have any to copy from!");
                return;
            }

            InventoryItemButtonPrompt downgradePrompt = __instance.gameObject.AddComponent<InventoryItemButtonPrompt>();
            downgradePrompt.appearCondition = copy.appearCondition;
            downgradePrompt.display = copy.display;
            downgradePrompt.data.ResponseText = tools.transformMsg;

            InventoryItemButtonPrompt upgradePrompt = __instance.gameObject.AddComponent<InventoryItemButtonPrompt>();
            upgradePrompt.appearCondition = downgradePrompt.appearCondition;
            upgradePrompt.display = downgradePrompt.display;
            upgradePrompt.data.ResponseText = downgradePrompt.data.ResponseText;

            Plugin.Logger.LogDebug($"Set up button prompts.");
        }

        [HarmonyPatch(nameof(InventoryItemSelectable.Submit)), HarmonyPostfix]
        private static void InventoryItemSelectable_Submit(InventoryItemSelectable __instance)
        {
            if (__instance is InventoryItemNail nail) {
                ChangeNail(nail, 1);
            }
        }

        [HarmonyPatch(nameof(InventoryItemSelectable.Extra)), HarmonyPostfix]
        private static void InventoryItemSelectable_Extra(InventoryItemSelectable __instance)
        {
            if (__instance is InventoryItemNail nail) {
                ChangeNail(nail, -1);
            }
        }

        private static void ChangeNail(InventoryItemNail nail, int delta)
        {
            int before = Plugin.Config.TargetNeedleUpgradeLevel;
            Plugin.Config.TargetNeedleUpgradeLevel += delta;
            nail.UpdateState(); // Update sprite
            nail.UpdateDisplay(); // Update description
            Plugin.Logger.LogDebug($"Changed {nameof(Plugin.Config.TargetNeedleUpgradeLevel)} from {before} to {Plugin.Config.TargetNeedleUpgradeLevel}");

            PlayAudioFeedback(before);
        }

        private static void PlayAudioFeedback(int before)
        {
            if (!Plugin.Assets.initialized) return;

            Config c = Plugin.Config;
            PlayerData d = PlayerData.instance;

            if (d == null) {
                Plugin.Logger.LogWarning($"Tried to play audio feedback but {nameof(PlayerData)}.{nameof(PlayerData.instance)} is null!");
                return;
            }

            AudioEvent audioEvent = default;
            audioEvent.PitchMin = 0.95f;
            audioEvent.PitchMax = 1.05f;
            audioEvent.Volume = 1f;

            if (c.TargetNeedleUpgradeLevel < 0) {
                audioEvent.Clip = Plugin.Assets.disableSound;
            }
            else if (!c.AllowTargetAboveUpgradedLevel && c.TargetNeedleUpgradeLevel > d.nailUpgrades
                || before == c.TargetNeedleUpgradeLevel) {
                audioEvent.Clip = Plugin.Assets.constrainedSound;
            }
            else {
                audioEvent.Clip = Plugin.Assets.overrideSound;
                float pitch = 1f + (0.1f * c.TargetNeedleUpgradeLevel);
                audioEvent.PitchMin = pitch;
                audioEvent.PitchMax = pitch;
            }

            audioEvent.SpawnAndPlayOneShot(GlobalSettings.Audio.DefaultUIAudioSourcePrefab, UnityEngine.Vector3.zero);
        }
    }
}
