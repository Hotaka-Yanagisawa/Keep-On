#region ヘッダコメント
// AudioMixerExtention.cs
// オーディオミキサーの拡張メソッド
//
// 2021/04/21 : 三上優斗
#endregion


using UnityEngine;
using UnityEngine.Audio;


namespace Mikami
{
    /// <summary>
    /// 拡張オーディオミキサークラス
    /// </summary>
    public static class AudioMiexerExtention
    {
        public static float GetVolumeByLinear(this AudioMixerGroup audioMixerGroup)
        {
            float decibel;

            audioMixerGroup.audioMixer.GetFloat(audioMixerGroup.name, out decibel);

            return Mathf.Pow(10f, decibel / 20f);
        }

        public static void SetVolumeByLinear(this AudioMixerGroup audioMixerGroup, float linearVolume)
        {
            float decibel = 20.0f * Mathf.Log10(linearVolume);

            if (float.IsNegativeInfinity(decibel))
            {
                decibel = -96f;
            }

            audioMixerGroup.audioMixer.SetFloat(audioMixerGroup.name, decibel);
        }
    }
}