using System;
using UnityEngine;
using System.Reflection;
using ColossalFramework.IO;
using System.IO;

namespace CSLMusicMod
{
    /// <summary>
    /// Behavior containing all method detours that are done if vanilla method
    /// behavior must be overwritten.
    /// </summary>
    public class Detours : MonoBehaviour
    {
        private RedirectCallsState m_RedirectObtainMusicClip;
        private RedirectCallsState m_RedirectStationName;
        private RedirectCallsState m_RedirectRadioPanelButtonGeneration;
        private RedirectCallsState m_RedirectAudioManagerQueueBroadcast;
        private RedirectCallsState m_RedirectAudioManagerCollectRadioContentInfo;

        public Detours()
        {
        }

        public void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void OnEnable()
        {
            Install();
        }

        public void OnDisable()
        {
            Uninstall();
        }

        public void Install()
        {
            CSLMusicMod.Log("[CSLMusic] Installing detours ...");
            m_RedirectObtainMusicClip = RedirectionHelper.RedirectCalls(typeof(RadioContentInfo).GetMethod(nameof(RadioContentInfo.ObtainClip), BindingFlags.Instance | BindingFlags.Public),
                typeof(CustomRadioContentInfo).GetMethod(nameof(CustomRadioContentInfo.CustomObtainClip), BindingFlags.Instance | BindingFlags.Public));
            m_RedirectStationName = RedirectionHelper.RedirectCalls(typeof(RadioChannelInfo).GetMethod(nameof(RadioContentInfo.GetLocalizedTitle), BindingFlags.Instance | BindingFlags.Public),
                typeof(CustomRadioChannelInfo).GetMethod(nameof(CustomRadioChannelInfo.CustomGetLocalizedTitle), BindingFlags.Instance | BindingFlags.Public));
            m_RedirectRadioPanelButtonGeneration = RedirectionHelper.RedirectCalls(typeof(RadioPanel).GetMethod("AssignStationToButton", BindingFlags.Instance | BindingFlags.NonPublic),
                typeof(CustomRadioPanel).GetMethod(nameof(CustomRadioPanel.CustomAssignStationToButton), BindingFlags.Instance | BindingFlags.Public));
            m_RedirectAudioManagerQueueBroadcast = RedirectionHelper.RedirectCalls(typeof(AudioManager).GetMethod(nameof(AudioManager.QueueBroadcast), BindingFlags.Instance | BindingFlags.Public),
                typeof(CustomAudioManager).GetMethod(nameof(CustomAudioManager.CustomQueueBroadcast), BindingFlags.Instance | BindingFlags.Public));
			m_RedirectAudioManagerCollectRadioContentInfo = RedirectionHelper.RedirectCalls(typeof(AudioManager).GetMethod(nameof(AudioManager.CollectRadioContentInfo), BindingFlags.Instance | BindingFlags.Public),
				typeof(CustomAudioManager).GetMethod(nameof(CustomAudioManager.CustomCollectRadioContentInfo), BindingFlags.Instance | BindingFlags.Public));
        }

        public void Uninstall()
        {
            CSLMusicMod.Log("[CSLMusic] Uninstalling detours ...");
            RedirectionHelper.RevertRedirect(m_RedirectObtainMusicClip);
            RedirectionHelper.RevertRedirect(m_RedirectStationName);
            RedirectionHelper.RevertRedirect(m_RedirectRadioPanelButtonGeneration);
            RedirectionHelper.RevertRedirect(m_RedirectAudioManagerQueueBroadcast);
            RedirectionHelper.RevertRedirect(m_RedirectAudioManagerCollectRadioContentInfo);
        }
    }
}

