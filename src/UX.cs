using HarmonyLib;

namespace PaleOilSoap
{
    // Casting to InventoryItemNail is not ideal but don't see an alternative
    // as InventoryItemNail does not override the desired methods
    [HarmonyPatch(typeof(InventoryItemSelectable))]
    internal static class UX
    {
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
            Plugin.Config.TargetNeedleUpgradeLevel += delta;
            nail.UpdateState(); // Update sprite
            nail.UpdateDisplay(); // Update description
            Plugin.Logger.LogDebug(Plugin.Config.TargetNeedleUpgradeLevel);
        }
    }
}
