using System.IO;
using System.Linq;
using UnityEngine;

namespace PaleOilSoap
{
    internal sealed class Assets
    {
        public bool initialized => (disableSound && overrideSound && constrainedSound);

        public AudioClip disableSound { get; private set; }
        public AudioClip overrideSound { get; private set; }
        public AudioClip constrainedSound { get; private set; }

        public Assets()
        {
            // Assuming existing loaded bundle will not be unloaded (given "sfxstatic_assets_shared")
            string guid = "81cdd0803a8bcfb81097fa4b8f33bb6e.bundle";
            AssetBundle assetBundle = AssetBundle.GetAllLoadedAssetBundles().FirstOrDefault((bundle) => bundle.name == guid);
            if (assetBundle != null) {
                Plugin.Logger.LogDebug("Found target asset bundle in loaded asset bundles.");
                LoadSounds(assetBundle);
            }
            else {
                Plugin.Logger.LogDebug("Could not find target asset bundle in loaded asset bundles, attempting to load manually.");
                string path = Path.Combine(Application.streamingAssetsPath, "aa", "StandaloneWindows64", "sfxstatic_assets_shared.bundle");
                assetBundle = AssetBundle.LoadFromFile(path);
                LoadSounds(assetBundle);
                assetBundle.Unload(false);
            }

            if (initialized) Plugin.Logger.LogDebug("Located assets.");
            else Plugin.Logger.LogWarning("Failed to locate assets.");
        }

        private void LoadSounds(AssetBundle assetBundle)
        {
            if (assetBundle == null) {
                Plugin.Logger.LogWarning("Failed to locate target asset bundle.");
                return;
            }

            disableSound = assetBundle.LoadAsset<AudioClip>("Assets/Audio/SFX/Heroes/Hornet/Crest Weapons/hornet_hunter_needleart_slash_2.wav");
            overrideSound = assetBundle.LoadAsset<AudioClip>("Assets/Audio/SFX/Enemy/Bosses/Hornet/hornet_needle_catch.wav");
            constrainedSound = assetBundle.LoadAsset<AudioClip>("Assets/Audio/SFX/sword_hit_reject.wav");
        }
    }
}
