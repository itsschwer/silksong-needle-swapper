using BepInEx;
using UnityEngine;

namespace PaleOilSoap
{
    [BepInPlugin(GUID, Name, Version)]
    public sealed class Plugin : BaseUnityPlugin
    {
        public const string GUID = Author + "." + Name;
        public const string Author = "itsschwer";
        public const string Name = "NeedleSwapper";
        public const string Version = "0.2.0";

        internal static new BepInEx.Logging.ManualLogSource Logger { get; private set; }

        internal static new Config Config { get; private set; }
        internal static Assets Assets { get; private set; }

        private void Awake()
        {
            // Use Plugin.GUID instead of Plugin.Name as source name
            BepInEx.Logging.Logger.Sources.Remove(base.Logger);
            Logger = BepInEx.Logging.Logger.CreateLogSource(Plugin.GUID);

            Config = new Config(base.Config);
            Assets = new Assets();

            new HarmonyLib.Harmony(Info.Metadata.GUID).PatchAll();

            Logger.LogMessage("~awake.");
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Home) && Input.GetKeyDown(KeyCode.End)) {
                Plugin.Logger.LogWarning("Debugging input triggered, reloading config.");
                Config.Reload();
            }
        }
    }
}
