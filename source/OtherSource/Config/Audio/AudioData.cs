#region �w�b�_�R�����g
// AudioData.cs
// ���f�[�^
//
// 2021/04/08 : �O��D�l
#endregion


using System.Collections.Generic;
using UnityEngine;


namespace Mikami
{
    [CreateAssetMenu(menuName = "ScriptableObject/AudioData",fileName = "AudioData")]
    public class AudioData : ScriptableObject
    {
        public List<AudioClip> bgmAudioClips;
        public List<AudioClip> menuSeAudioClips;
        public List<AudioClip> jingleAudioClips;
    }
}