using BepInEx.Bootstrap;
using System.Runtime.CompilerServices;

namespace PaleOilSoap
{
    internal static class Compatibility
    {
        public const string SilksongPrepatcherGUID = "org.silksong-modding.prepatcher";

        public static bool PrepatcherPresent => Chainloader.PluginInfos.ContainsKey(SilksongPrepatcherGUID);

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void Apply()
        {
            // Should probably use SilksongPrepatcher's PlayerDataVariableEvents but
            // that appears to be applied globally, whereas this
            // mod only reroutes specific accesses to PlayerData.nailUpgrades
        }
    }
}
