#region �w�b�_�R�����g
//==================================================================================
// ConfigAudioVolume.cs
// ���ʐݒ�N���X
// �쐬����	: 2021/03/31
// �쐬��	: �O��D�l
//==================================================================================
#endregion


using UnityEngine;
using UnityEngine.UI;
using Mizuno;


namespace Mikami
{
    /// <summary>
    /// ���ʐݒ�UI�N���X
    /// </summary>
    public class ConfigAudioVolume : MonoBehaviour
    {
        [SerializeField]
        private Slider sliderMasterVolume, sliderBgmVolume, sliderSeVolume;
		[SerializeField] private ConfigData configData;


        #region �v���C�x�[�g�֐�������
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