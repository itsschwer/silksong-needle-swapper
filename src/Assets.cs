using System.IO;
using UnityEngine;

namespace PaleOilSoap
{
    internal sealed class Assets
    {
        public bool initialized => (disableSound && overrideSound && constrainedSound);

        public readonly AudioClip disableSound;
        public readonly AudioClip overrideSound;
        public readonly AudioClip constrainedSound;

        public Assets()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "aa", "StandaloneWindows64", "sfxstatic_assets_shared.bundle");

            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);

            if (assetBundle == null) {
                Plugin.Logger.LogWarning("Failed to locate asset bundle.");
                return;
            }

            disableSound = assetBundle.LoadAsset<AudioClip>("Assets/Audio/SFX/Heroes/Hornet/Crest Weapons/hornet_hunter_needleart_slash_2.wav");
            overrideSound = assetBundle.LoadAsset<AudioClip>("Assets/Audio/SFX/Enemy/Bosses/Hornet/hornet_needle_catch.wav");
            constrainedSound = assetBundle.LoadAsset<AudioClip>("Assets/Audio/SFX/sword_hit_reject.wav");
            assetBundle.Unload(false);

            if (initialized) Plugin.Logger.LogDebug("Located assets.");
            else Plugin.Logger.LogWarning("Failed to locate assets.");
        }
    }
}
