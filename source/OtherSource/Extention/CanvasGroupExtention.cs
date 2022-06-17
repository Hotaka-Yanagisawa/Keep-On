#region ヘッダコメント
// CanvasGroupExtention.cs
// キャンバスグループの拡張メソッド
//
// 2021/04/05 : 三上優斗
#endregion


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mikami
{
    public static class CanvasGroupExtention
    {
        public static void Show(this CanvasGroup canvasGroup)
        {
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public static void Hide(this CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.gameObject.SetActive(false);
        }

        public static IEnumerator FadeOutCoroutine(this CanvasGroup canvasGroup, float time, System.Action action)
        {
            float endTime = Time.timeSinceLevelLoad + time * canvasGroup.alpha;

            var endFrame = new WaitForEndOfFrame();

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            while (Time.timeSinceLevelLoad <= endTime)
            {
                canvasGroup.alpha = (endTime - Time.timeSinceLevelLoad) / time;
                yield return endFrame;
            }
            canvasGroup.alpha = 0;

            action?.Invoke();
            canvasGroup.gameObject.SetActive(false);
        }

        public static IEnumerator FadeInCoroutine(this CanvasGroup canvasGroup, float time, System.Action action)
        {
            float endTime = Time.timeSinceLevelLoad + time * (1 - canvasGroup.alpha);

            var endFrame = new WaitForEndOfFrame();

            canvasGroup.gameObject.SetActive(true);

            while (Time.timeSinceLevelLoad <= endTime)
            {
                canvasGroup.alpha = 1 - ((endTime - Time.timeSinceLevelLoad) / time);
                yield return endFrame;
            }
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            action?.Invoke();
        }
    }
}