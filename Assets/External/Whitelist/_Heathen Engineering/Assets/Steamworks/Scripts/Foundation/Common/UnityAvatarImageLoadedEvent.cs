#if !DISABLESTEAMWORKS
using Steamworks;
using System;
using UnityEngine.Events;

namespace HeathenEngineering.SteamAPI
{
    /// <summary>
    /// A custom serializable <see cref="UnityEvent{T0}"/> which handles <see cref="AvatarImageLoaded_t"/> data.
    /// </summary>
    [Serializable]
    public class UnityAvatarImageLoadedEvent : UnityEvent<AvatarImageLoaded_t>
    { }
}
#endif
