#region ヘッダコメント
// AudioMixerManager.cs
// オーディオミキサー管理クラス
//
// 2021/04/02 : 三上優斗
#endregion


using UnityEngine;
using UnityEngine.Audio;


namespace Mikami
{
    /// <summary>
    /// オーディオミキサー管理クラス
    /// </summary>
    public class AudioMixerManager : MonoBehaviour
    {
        #region 変数宣言部
        [SerializeField]
        private AudioMixer audioMixer;

        [SerializeField]
        private AudioMixerGroup master, bgm, gameSe, menuSe, voice, jingle;
        #endregion


        #region プロパティ
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