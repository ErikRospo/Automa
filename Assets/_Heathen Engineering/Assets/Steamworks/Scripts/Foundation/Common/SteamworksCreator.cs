#if !DISABLESTEAMWORKS && !UNITY_SERVER
using UnityEngine;

namespace HeathenEngineering.SteamAPI
{
    [HelpURL("https://kb.heathenengineering.com/assets/steamworks")]
    [DisallowMultipleComponent]
    public class SteamworksCreator : MonoBehaviour
    {
        public bool createOnStart;
        public bool markAsDoNotDestroy;
        public SteamSettings settings;

        private void Start()
        {
            if (createOnStart)
                CreateIfMissing(settings, markAsDoNotDestroy);
        }

        public void CreateIfMissing()
        {
            CreateIfMissing(settings, markAsDoNotDestroy);
        }

        public static void CreateIfMissing(SteamSettings settings, bool doNotDestroy = false) => settings.CreateBehaviour(doNotDestroy);
    }
}
#endif