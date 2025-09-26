using System.IO;
using UnityEngine;

namespace PaleOilSoap
{
    internal sealed class Assets
    {
        public bool initialized = false;

        public readonly AudioClip disableSound;
        public readonly AudioClip overrideSound;
        public readonly AudioClip constrainedSound;

        public Assets()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "aa", "StandaloneWindows64", "sfxstatic_assets_shared.bundle");

            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);

            if (assetBundle == null) {
                Plugin.Logger.LogError("Failed to locate asset bundle!");
                return;
            }

            disableSound = assetBundle.LoadAsset<AudioClip>("hornet_hunter_needleart_slash_2");
            overrideSound = assetBundle.LoadAsset<AudioClip>("hornet_needle_catch");
            constrainedSound = assetBundle.LoadAsset<AudioClip>("sword_hit_reject");
            initialized = true;

            assetBundle.Unload(false);

            Plugin.Logger.LogDebug("Located assets.");
        }
    }
}
