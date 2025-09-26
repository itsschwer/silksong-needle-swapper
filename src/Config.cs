using BepInEx.Configuration;

namespace PaleOilSoap
{
    internal sealed class Config
    {
        private readonly ConfigFile file;
        internal void Reload() { Plugin.Logger.LogDebug($"Reloading {file.ConfigFilePath.Substring(file.ConfigFilePath.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1)}"); file.Reload(); }


        private readonly AcceptableValueRange<int> acceptableTargetNeedleUpgradeLevels = new AcceptableValueRange<int>(-1, 4);
        private readonly ConfigEntry<int> targetNeedleUpgradeLevel;
        public int TargetNeedleUpgradeLevel {
            get { return targetNeedleUpgradeLevel.Value; }
            internal set { targetNeedleUpgradeLevel.Value = value; }
        }

        private readonly ConfigEntry<bool> allowTargetAboveUpgradedLevel;
        public bool AllowTargetAboveUpgradedLevel => allowTargetAboveUpgradedLevel.Value;

        internal Config(ConfigFile config)
        {
            file = config;

            const string Needle = "Needle";

            targetNeedleUpgradeLevel = config.Bind<int>(Needle, nameof(targetNeedleUpgradeLevel), -1,
                new ConfigDescription(
                    "The target upgrade level to override the Needle to." +
                    "\n(-1 to disable override, 0 for starting Needle, 1–4 for each Needle upgrade)",
                    acceptableTargetNeedleUpgradeLevels));

            allowTargetAboveUpgradedLevel = config.Bind<bool>(Needle, nameof(allowTargetAboveUpgradedLevel), false,
                "Allow targetNeedleUpgradeLevel to use levels higher than has been acquired through the Pinmaster.");
        }
    }
}
