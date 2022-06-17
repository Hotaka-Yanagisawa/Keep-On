#region �w�b�_�R�����g
// AudioMixerManager.cs
// �I�[�f�B�I�~�L�T�[�Ǘ��N���X
//
// 2021/04/02 : �O��D�l
#endregion


using UnityEngine;
using UnityEngine.Audio;


namespace Mikami
{
    /// <summary>
    /// �I�[�f�B�I�~�L�T�[�Ǘ��N���X
    /// </summary>
    public class AudioMixerManager : MonoBehaviour
    {
        #region �ϐ��錾��
        [SerializeField]
        private AudioMixer audioMixer;

        [SerializeField]
        private AudioMixerGroup master, bgm, gameSe, menuSe, voice, jingle;
        #endregion


        #region �v���p�e�B
        public AudioMixerGroup bgmAudioMixerGroup { get => bgm; }
        public AudioMixerGroup menuSeAudioMixerGroup { get => menuSe; }
        public AudioMixerGroup jingleAudioMixerGroup { get => jingle; }

        public float MasterVolumeByLinear
        {
            get => master.GetVolumeByLinear();
            set => master.SetVolumeByLinear(value);
        }

        public float BgmVolumeByLinear
        {
            get => bgm.GetVolumeByLinear();
            set => bgm.SetVolumeByLinear(value);
        }

        public float SeVolumeByLinear
        {
            get => gameSe.GetVolumeByLinear();
            set { gameSe.SetVolumeByLinear(value); menuSe.SetVolumeByLinear(value); }
        }
        #endregion
    }
}