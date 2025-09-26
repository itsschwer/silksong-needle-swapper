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

            Plugin.Logger.LogWarning(File.Exists(path));
            AssetBundle.UnloadAllAssetBundles(true);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            Plugin.Logger.LogWarning($"{assetBundle.name} | {assetBundle.GetName()}");

            foreach (var bundle in AssetBundle.GetAllLoadedAssetBundles()) {
                Plugin.Logger.LogWarning($"{bundle.name} | {bundle.GetName()}");
                if (bundle.name == "81cdd0803a8bcfb81097fa4b8f33bb6e.bundle") {
                    assetBundle = bundle;
                }
            } 

            //if (assetBundle == null) {
            //    Plugin.Logger.LogError($"Failed to load asset bundle at: {path}");
            //    return;
            //}

            disableSound = assetBundle.LoadAsset<AudioClip>("hornet_hunter_needleart_slash_2");
            overrideSound = assetBundle.LoadAsset<AudioClip>("hornet_needle_catch");
            constrainedSound = assetBundle.LoadAsset<AudioClip>("sword_hit_reject");
            initialized = true;

            Plugin.Logger.LogWarning("A");
        }
    }
}
