#region ヘッダコメント
//==================================================================================
// ConfigAudioVolume.cs
// 音量設定クラス
// 作成日時	: 2021/03/31
// 作成者	: 三上優斗
//==================================================================================
#endregion


using UnityEngine;
using UnityEngine.UI;
using Mizuno;


namespace Mikami
{
    /// <summary>
    /// 音量設定UIクラス
    /// </summary>
    public class ConfigAudioVolume : MonoBehaviour
    {
        [SerializeField]
        private Slider sliderMasterVolume, sliderBgmVolume, sliderSeVolume;
		[SerializeField] private ConfigData configData;


        #region プライベート関数実装部
        private void Start()
        {
            var audioMixerManager = SoundManager.Instance.GetAudioMixerManager();

            sliderMasterVolume.value = audioMixerManager.MasterVolumeByLinear;
            sliderBgmVolume.value = audioMixerManager.BgmVolumeByLinear;
            sliderSeVolume.value = audioMixerManager.SeVolumeByLinear;

            sliderMasterVolume?.onValueChanged.AddListener(volume => audioMixerManager.MasterVolumeByLinear = volume);
            sliderBgmVolume?.onValueChanged.AddListener(volume => audioMixerManager.BgmVolumeByLinear = volume);
            sliderSeVolume?.onValueChanged.AddListener(volume =>
            {
                audioMixerManager.SeVolumeByLinear = volume;
                SoundManager.Instance.PlayMenuSe("SE_TEST");
            }
            );
        }
        #endregion


		public void ResetValue()
		{
			var audioMixerManager = SoundManager.Instance.GetAudioMixerManager();

			configData.OnAfterDeserialize();

			audioMixerManager.MasterVolumeByLinear = configData.masterVolume;
			audioMixerManager.BgmVolumeByLinear = configData.bgmVolume;
			audioMixerManager.SeVolumeByLinear = configData.seVolume;

			sliderMasterVolume.value = audioMixerManager.MasterVolumeByLinear;
			sliderBgmVolume.value = audioMixerManager.BgmVolumeByLinear;
			sliderSeVolume.value = audioMixerManager.SeVolumeByLinear;
		}
    }
}