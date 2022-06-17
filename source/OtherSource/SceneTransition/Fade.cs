#region ヘッダコメント
// Fade.cs
// フェードクラス
//
// 2021.03.23 : 三上優斗
#endregion


using UnityEngine;
using System.Collections;


namespace Mikami
{
    /// <summary>
    /// フェード操作クラス
    /// </summary>
    public class Fade : MonoBehaviour
    {
        #region 変数宣言部
        public bool IsRunning { get; private set; }
        public FadeImage Image { get; private set; }
        private IFade fade;
        private float cutoutRange;
        #endregion


        #region パブリック関数実装部
        public Coroutine FadeOut(float time, System.Action action)
        {
            StopAllCoroutines();
            return StartCoroutine(FadeoutCoroutine(time, action));
        }

        public Coroutine FadeOut(float time)
        {
            return FadeOut(time, null);
        }

        public Coroutine FadeIn(float time, System.Action action)
        {
            StopAllCoroutines();
            return StartCoroutine(FadeinCoroutine(time, action));
        }

        public Coroutine FadeIn(float time)
        {
            return FadeIn(time, null);
        }
        #endregion


        #region プライベート関数実装部
        void Start()
        {
            IsRunning = false;
            Init();
            fade.Range = cutoutRange;
        }

        private void Init()
        {
            fade = GetComponent<IFade>();
            Image = GetComponent<FadeImage>();
        }

        void OnValidate()
        {
            Init();
            fade.Range = cutoutRange;
        }

        #region コルーチン関数
        IEnumerator FadeoutCoroutine(float time, System.Action action)
        {
            if (IsRunning) yield break;
            IsRunning = true;

            float endTime = Time.timeSinceLevelLoad + time * (cutoutRange);

            var endFrame = new WaitForEndOfFrame();

            while (Time.timeSinceLevelLoad <= endTime)
            {
                cutoutRange = (endTime - Time.timeSinceLevelLoad) / time;
                fade.Range = cutoutRange;
                yield return endFrame;
            }
            cutoutRange = 0;
            fade.Range = cutoutRange;

            action?.Invoke();

            IsRunning = false;
        }

        IEnumerator FadeinCoroutine(float time, System.Action action)
        {
            if (IsRunning) yield break;
            IsRunning = true;

            float endTime = Time.timeSinceLevelLoad + time * (1 - cutoutRange);

            var endFrame = new WaitForEndOfFrame();

            while (Time.timeSinceLevelLoad <= endTime)
            {
                cutoutRange = 1 - ((endTime - Time.timeSinceLevelLoad) / time);
                fade.Range = cutoutRange;
                yield return endFrame;
            }

            cutoutRange = 1;
            fade.Range = cutoutRange;

            action?.Invoke();

            IsRunning = false;
        }
        #endregion
        #endregion
    }
}