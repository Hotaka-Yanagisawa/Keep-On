#region ヘッダコメント
// TransitioManager.cs
// シーン遷移管理クラス。参考URL→https://qiita.com/toRisouP/items/1713d2addf6f5dc9f9b8
//
// 2021.03.18 : 三上優斗
#endregion


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mizuno;


namespace Mikami
{
    /// <summary>
    /// シーン遷移管理クラス
    /// </summary>
    public class TransitionManager : SingletonMonoBehaviour<TransitionManager>
    {
        #region 変数宣言部
        [SerializeField] Slider bar;
        [SerializeField] RectTransform wheelO;
        [SerializeField] RectTransform pendulumE;
        [SerializeField] RectTransform reachOnE;
        [SerializeField] GameObject loadingPanel;
        private bool autoStartFade = false;
        private Fade fadeComponent;
        public delegate void OnTransitionEventHandler();
        public event OnTransitionEventHandler OnAllSceneLoaded;
        public event OnTransitionEventHandler OnTransitionAnimationStart;
        public event OnTransitionEventHandler OnTransitionAnimationFinished;
        private AsyncOperation loadAsyncOperation;
        #endregion
        

        #region プロパティ
        public bool IsRunning { get; private set; }
        public Scenes CurrentGameScene { get; private set; }
        #endregion


        #region パブリック関数実装部
        /// <summary>
        /// シーン遷移時、自動フェードをfalseにした場合に呼び出す必要がある
        /// </summary>
        public void Open()
        {
            autoStartFade = true;
        }


		/// <summary>
		/// シーン遷移を実行する
		/// </summary>
		/// <param name="nextScene">次のシーン</param>
		/// <param name="data">次のシーンへ引き継ぐデータ</param>
		/// <param name="additiveLoadScenes">追加ロードするシーン</param>
		/// <param name="autoMove">トランジションの自動遷移を行うか</param>
		public void StartTransaction(
			Scenes nextScene,
			//SceneDataPack data,
			Scenes[] additiveLoadScenes = null,
			bool autoMove = true
			)
		{
			if (IsRunning) return;
			StartCoroutine(TransitionCoroutine(nextScene, /*data,*/ additiveLoadScenes, autoMove));
		}
        #endregion


        #region プライベート関数実装部
        private void Start()
        {
            IsRunning = false;
            CurrentGameScene = Scenes.Manager;
            fadeComponent = GetComponent<Fade>();
            fadeComponent.Image.raycastTarget = false;//タッチイベントを蓋絵でブロクしない
            //初期化直後にシーン遷移完了通知を発行する(デバッグで任意のシーンからゲームを開始できるように)
            OnAllSceneLoaded?.Invoke();
        }


        #region コルーチン関数
        /// <summary>
        /// シーン遷移処理の本体
        /// </summary>
        private IEnumerator TransitionCoroutine(
            Scenes nextScene,
            //SceneDataPack data,
            Scenes[] additiveLoadScenes,
            bool autoMove
            )
        {
            if (IsRunning) yield break;
            //処理開始フラグセット
            IsRunning = true;

            //トランジション開始（蓋絵で画面を隠す）
            autoStartFade = autoMove;
            fadeComponent.Image.raycastTarget = true;
            fadeComponent.FadeIn(1f);
            SoundManager.Instance.StopBGMWithFade(1f);
            //トランジションアニメーションが終了するのを待つ
            yield return new WaitWhile(() => fadeComponent.IsRunning);


            // 蓋絵を外しロードUI画面を見せる
            fadeComponent.FadeOut(1f);
            loadingPanel.SetActive(true);
            //トランジションアニメーションが終了するのを待つ
            yield return new WaitWhile(() => fadeComponent.IsRunning);

            // 見せかけプログレスバー(実態を伴わせても現状では終わるため
            float progress = 0f;
            while(progress < 0.99f)
            {
                yield return null;
                bar.value = progress;
                wheelO.rotation = Quaternion.Euler(0f, 0f, -Mathf.Repeat(progress * 360, 360));
                pendulumE.rotation = Quaternion.Euler(0f, 0f, 0f + 10f * Mathf.Cos(Mathf.PI * 2f * progress));
                reachOnE.rotation = Quaternion.Euler(0f, 0f, 30f + 10f * Mathf.Cos(Mathf.PI * 2f * progress));
                progress += 0.01f;
            }

            //メインとなるシーンをSingleで読み込む
            loadAsyncOperation = SceneManager.LoadSceneAsync(nextScene.ToString(), LoadSceneMode.Single);
            
            while(!loadAsyncOperation.isDone)
            {
                yield return null;
                //bar.value = loadAsyncOperation.progress;
                //wheelO.rotation = Quaternion.Euler(0f, 0f, Mathf.Repeat(bar.value * 1800, 360));
            }
            bar.value = 1f;
            wheelO.rotation = Quaternion.Euler(0f, 0f, 0f);

            //yield return SceneManager.LoadSceneAsync(nextScene.ToString(), LoadSceneMode.Single);

            //追加シーンがある場合は一緒に読み込む
            //if (additiveLoadScenes != null)
            //{
            //    yield return additiveLoadScenes.Select(scene =>
            //        SceneManager.LoadSceneAsync("Scenes/" + scene.ToString(), LoadSceneMode.Additive)
            //        .AsObservable()).WhenAll().ToYieldInstruction();
            //}
            //yield return null;

            //使ってないリソースの解放とGCを実行
            Resources.UnloadUnusedAssets();
            GC.Collect();
            yield return null;
            
            CurrentGameScene = nextScene;
            //シーンロードの完了通知を発行
            OnAllSceneLoaded?.Invoke();
            
            if (!autoMove)
            {
                //自動遷移しない設定の場合はフラグがtrueに変化するまで待機
                yield return new WaitUntil(() => autoStartFade);
            }

            fadeComponent.FadeIn(1f);
            yield return new WaitWhile(() => fadeComponent.IsRunning);
            loadingPanel.SetActive(false);

            //蓋絵を開く方のアニメーション開始
            autoStartFade = false;
            fadeComponent.FadeOut(1f);
            OnTransitionAnimationStart?.Invoke();
            //蓋絵が開ききるのを待つ
            yield return new WaitWhile(() => fadeComponent.IsRunning);
            fadeComponent.Image.raycastTarget = false;

            bar.value = 0f;

            //トランジションが全て完了したことを通知
            OnTransitionAnimationFinished?.Invoke();
            
            //終了
            IsRunning = false;
        }
        #endregion
        #endregion
    }
}