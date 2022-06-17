#region ヘッダコメント
// SoundManager.cs
// サウンド管理クラス
//
// 2021/03/22 : 水野樹生
// 2021/04/02 : 三上優斗
#endregion


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Mikami;


namespace Mizuno
{
    /// <summary>
    /// サウンド管理クラス
    /// </summary>
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        #region 変数宣言部
        [SerializeField] private AudioMixerManager audioMixermanager;
        [SerializeField] private Transform audioListenerTransform;
        [SerializeField] private AudioData audioData;
        [SerializeField] private ConfigData configData;
        private AudioSource audioSourceMenuSe;
        private AudioSource audioSourceJingle;
        private List<AudioSource> audioSourceBGMList = new List<AudioSource>(2);
        private List<IEnumerator> fadeCoroutineList = new List<IEnumerator>();
        private IEnumerator jinglePlayCompCallbackCoroutine;
        #endregion


        #region パブリック関数実装部
        public AudioMixerManager GetAudioMixerManager()
        {
            return audioMixermanager;
        }

        public void PlayJingle(string clipName, UnityAction compCallback = null)
        {
            compCallback += () => { jinglePlayCompCallbackCoroutine = null; };

            AudioClip audioClip = audioData.jingleAudioClips.FirstOrDefault(clip => clip.name == clipName);

            if (audioClip == null)
            {
                Debug.Log("Can't find audioClip " + clipName);
                return;
            }

            jinglePlayCompCallbackCoroutine = audioSourceJingle.PlayWithCompCallback(audioClip: audioClip, compCallback: compCallback);

            StartCoroutine(jinglePlayCompCallbackCoroutine);
        }
        
        public void PlayMenuSe(string clipName)
        {
            AudioClip audioClip = audioData.menuSeAudioClips.FirstOrDefault(clip => clip.name == clipName);

            if (audioClip == null)
            {
                Debug.Log("Can't find audioClip " + clipName);
                return;
            }
            audioSourceMenuSe.PlayOneShot(audioClip);
            //audioSourceMenuSe.Play(audioClip);
        }

        public void PlayBGM(string clipName)
        {
            PlayBGMWithFade(clipName, 0.1f);
        }

        public void PlayBGMWithFade(string clipName)
        {
            PlayBGMWithFade(clipName, 2f);
        }

        public void PlayBGMWithFade(string clipName, float fadeTime)
        {
            AudioClip audioClip = audioData.bgmAudioClips.FirstOrDefault(clip => clip.name == clipName);
            
            if (audioClip == null)
            {
                Debug.Log("Can't find audioClip " + clipName);
                return;
            }

            AudioSource audioSourceEmpty = audioSourceBGMList.FirstOrDefault(asb => asb.isPlaying == false);

            if (audioSourceEmpty == null)
            {
                Debug.LogWarning("フェード処理中は新たなBGMを再生開始できません");
                return;
            }
            else
            {
                StopFadeCoroutine();

                // 再生中ならフェードアウト処理
                AudioSource audioSourcePlaying = audioSourceBGMList.FirstOrDefault(asb => asb.isPlaying == true);
                if (audioSourcePlaying)
                    AddFadeCoroutineListAndStart(audioSourcePlaying.StopWithFadeOut(fadeTime));

                AddFadeCoroutineListAndStart(audioSourceEmpty.PlayWithFadeIn(audioClip, fadeTime: fadeTime));
            }
        }

        public void StopBGM()
        {
            StopBGMWithFade(0.1f);
        }

        public void StopBGMWithFade(float fadeTime)
        {
            StopFadeCoroutine();
            
            foreach (AudioSource asb in audioSourceBGMList.Where(asb => asb.isPlaying == true))
                AddFadeCoroutineListAndStart(asb.StopWithFadeOut(fadeTime));
        }

        public void SetAudioListener(Transform followTransform)
        {
            audioListenerTransform.SetParent(followTransform);
            audioListenerTransform.SetPositionAndRotation(followTransform.position, followTransform.rotation);
        }

        public void ClearAudioListenerPos()
        {
            audioListenerTransform.SetParent(this.transform);
            audioListenerTransform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        #endregion


        #region プライベート関数実装部
        protected override void Awake()
        {
            base.Awake();

            // オーディオソースの設定
            audioSourceMenuSe = gameObject.AddComponent<AudioSource>();
            audioSourceMenuSe.playOnAwake = false;
            audioSourceMenuSe.outputAudioMixerGroup = audioMixermanager.menuSeAudioMixerGroup;
            audioSourceJingle = gameObject.AddComponent<AudioSource>();
            audioSourceJingle.playOnAwake = false;
            audioSourceJingle.outputAudioMixerGroup = audioMixermanager.jingleAudioMixerGroup;
            audioSourceBGMList.Add(gameObject.AddComponent<AudioSource>());
            audioSourceBGMList.Add(gameObject.AddComponent<AudioSource>());
            audioSourceBGMList.ForEach(asb =>
            {
                asb.playOnAwake = false;
                asb.outputAudioMixerGroup = audioMixermanager.bgmAudioMixerGroup;
                asb.loop = true;
            });
        }

        private void Start()
        {
            audioMixermanager.MasterVolumeByLinear = configData.masterVolume;
            audioMixermanager.BgmVolumeByLinear = configData.bgmVolume;
            audioMixermanager.SeVolumeByLinear = configData.seVolume;
        }

        private void AddFadeCoroutineListAndStart(IEnumerator routine)
        {
            fadeCoroutineList.Add(routine);
            StartCoroutine(routine);
        }

        private void StopFadeCoroutine()
        {
            fadeCoroutineList.ForEach(routine => StopCoroutine(routine));
            fadeCoroutineList.Clear();
        }
        #endregion
    }

}