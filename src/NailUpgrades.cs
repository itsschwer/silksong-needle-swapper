using HarmonyLib;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;

namespace PaleOilSoap
{
    [HarmonyPatch]
    internal static class NailUpgrades
    {
        internal static int AdjustNailUpgrade(int acquiredNailUpgrade)
        {
            Config c = Plugin.Config;

            if (c.TargetNeedleUpgradeLevel < 0) return acquiredNailUpgrade;
            if (!c.AllowTargetAboveUpgradedLevel && c.TargetNeedleUpgradeLevel > acquiredNailUpgrade) return acquiredNailUpgrade;

            return c.TargetNeedleUpgradeLevel;
        }

        // Only target these specific uses of PlayerData.nailUpgrades to avoid unwanted side-effects
        // e.g. Pinmaster upgrades, completion percentage
        [HarmonyILManipulator]
        [HarmonyPatch(typeof(InventoryItemNail), nameof(InventoryItemNail.UpdateState))]
        [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.nailDamage), MethodType.Getter)]
        [HarmonyPatch(typeof(HealthManager), nameof(HealthManager.ApplyDamageScaling))]
        private static void Redirect__PlayerData_nailUpgrades(ILContext il)
        {
            Plugin.Logger.LogDebug($"Attempting to patch {il.Method.Name}");

            ILCursor c = new ILCursor(il);

            Func<Instruction, bool>[] match = [
                // int num = PlayerData.instance.nailUpgrades;
                // nailDamage belongs to instance so won't match instance getter
                // x => x.MatchCallOrCallvirt<PlayerData>($"get_{nameof(PlayerData.instance)}"),
                x => x.MatchLdfld<PlayerData>(nameof(PlayerData.nailUpgrades)),
                x => x.MatchStloc(out int _)
            ];

            if (Compatibility.PrepatcherPresent) {
                Plugin.Logger.LogDebug($"Detected '{Compatibility.SilksongPrepatcherGUID}', trying alternative IL instruction match.");
                match = [
                    // int num = PlayerData.instance.GetInt("nailUpgrades");
                    // x => x.MatchCallOrCallvirt<PlayerData>($"get_{nameof(PlayerData.instance)}"),
                    x => x.MatchLdstr(nameof(PlayerData.nailUpgrades)),
                    x => x.MatchCallOrCallvirt<PlayerData>(nameof(PlayerData.GetInt)),
                    x => x.MatchStloc(out int _)
                ];
            }

            if (c.TryGotoNext(MoveType.After, match)) {
                c.Index--; // move before stloc
                c.EmitDelegate(AdjustNailUpgrade);
#if DEBUG
                Plugin.Logger.LogDebug(il.ToString());
#endif
            }
            else Plugin.Logger.LogError($"Cannot hook: failed to match IL instructions for {il.Method.Name}");
        }
    }
}
