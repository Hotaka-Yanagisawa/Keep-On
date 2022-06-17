#region �w�b�_�R�����g
// AudioMixerExtention.cs
// �I�[�f�B�I�~�L�T�[�̊g�����\�b�h
//
// 2021/04/21 : �O��D�l
#endregion


using UnityEngine;
using UnityEngine.Audio;


namespace Mikami
{
    /// <summary>
    /// �g���I�[�f�B�I�~�L�T�[�N���X
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