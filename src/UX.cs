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
            InventoryItemButtonPrompt copy = __instance.GetComponent<InventoryItemButtonPrompt>();
            if (copy == null) {
                Plugin.Logger.LogWarning($"Tried to set up button prompts but {nameof(InventoryItemNail)} did not have any to copy from!");
                return;
            }

            // Hard-coded, not ideal; obtained through Unity Explorer on Voltvessels (Lightning Rod) tool
            TeamCherry.Localization.LocalisedString transformMsg = new TeamCherry.Localization.LocalisedString("Tools", "UI_BUTTON_TOGGLE_STATE");

            // Will NRE in OnEnable when adding component because appearCondition is not set yet
            InventoryItemButtonPrompt downgradePrompt = __instance.gameObject.AddComponent<InventoryItemButtonPrompt>();
            downgradePrompt.appearCondition = copy.appearCondition;
            downgradePrompt.display = copy.display;
            downgradePrompt.data.ResponseText = transformMsg;
            downgradePrompt.data.Action = ControlReminder.MapActionToAction(GlobalEnums.HeroActionButton.MENU_EXTRA);
            // MENU_EXTRA is None on keyboard but DASH is incorrect for gamepad (uses ATTACK instead); see global::ControlReminder.MapActionToAction
            // This means the downgrade button prompt will be incorrect if switching between gamepad and keyboard

            // Will NRE in OnEnable when adding component because appearCondition is not set yet
            InventoryItemButtonPrompt upgradePrompt = __instance.gameObject.AddComponent<InventoryItemButtonPrompt>();
            upgradePrompt.appearCondition = downgradePrompt.appearCondition;
            upgradePrompt.display = downgradePrompt.display;
            upgradePrompt.data.ResponseText = downgradePrompt.data.ResponseText;
            upgradePrompt.data.Action = GlobalEnums.HeroActionButton.JUMP;
            // MENU_SUBMIT shows Enter on keyboard, so JUMP is safer for both keyboard and gamepad; see global::ControlReminder.MapActionToAction

            // The Voltvessels transform prompt uses MenuButtonIcon, which converts a Platform.MenuActions to a HeroActionButton when updating the display,
            // whereas InventoryItemButtonPrompt is fed into ActionButtonIcon, which only has a set HeroActionButton with no conversion when updating the display

            Plugin.Logger.LogInfo($"Set up button prompts (with {nameof(Platform.Current.WasLastInputKeyboard)}: {Platform.Current?.WasLastInputKeyboard})" +
                $"\nThe preceding two instances of {nameof(System.NullReferenceException)} from {nameof(InventoryItemButtonPromptBase<bool>)}.{nameof(InventoryItemButtonPrompt.OnEnable)} should be safe to ignore (no elegant workaround).");
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
            string debugAdditionalLine = (PlayerData.instance == null) ? "{nameof(PlayerData)}.{nameof(PlayerData.instance)} is null??" : $"AcquiredNailUpgrades: {PlayerData.instance.nailUpgrades}}} (resolves as {NailUpgrades.AdjustNailUpgrade(PlayerData.instance.nailUpgrades)}";
            Plugin.Logger.LogDebug($"Changed {nameof(Plugin.Config.TargetNeedleUpgradeLevel)} from {before} to {Plugin.Config.TargetNeedleUpgradeLevel}" +
                $"\n\t{{{nameof(Plugin.Config.AllowTargetAboveUpgradedLevel)}: {Plugin.Config.AllowTargetAboveUpgradedLevel}, {debugAdditionalLine})");
            PlayAudioFeedback(before);
        }

        private static void PlayAudioFeedback(int before)
        {
            if (!Plugin.Assets.initialized) {
                Plugin.Logger.LogWarning($"Tried to play audio feedback but {nameof(Assets)}.{nameof(Assets.initialized)} is {Plugin.Assets.initialized}!");
                return;
            }

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
