#region ヘッダコメント
// AudioData.cs
// 音データ
//
// 2021/04/08 : 三上優斗
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