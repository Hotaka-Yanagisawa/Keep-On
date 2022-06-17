using UnityEngine;
using Mikami;
using UnityEngine.UI;

#region HeaderComent
//==================================================================================
// Result
//	プレイヤークラスの本体　partialでアクション別など複数のスクリプトに分けている　
//  おおもとになるスクリプト
// 作成日時	:2021/03/19
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/03/19	参考動画のサンプルを基にプレイヤーのステートマシン制作開始         参考動画(https://www.youtube.com/watch?v=PbtJt5tnnI8&t=1181s)
// 2021/03/19   InputSystemの導入                                                  参考動画(https://www.youtube.com/watch?v=pRSZr6CFcpQ&t=1080s)
//      :               :                                                          参考資料(http://tsubakit1.hateblo.jp/entry/2019/10/13/143530)  未だ勉強中・・・
// 2021/03/20	プレイヤーの基本的な動き(移動、ジャンプ、回避)を実装 ← 遷移するときスムーズに遷移しない
// 2021/03/20   状態遷移するときにスムーズに遷移するように修正
// 2021/03/21   コメントアウトをしていなかったのでしました。日付等は若干適当ゆるして。
// 2021/04/01   アニメーションに伴い攻撃のステートを追加
//==================================================================================
#endregion

namespace Maeda
{
    public partial class Result
    {
        #region ステートのインスタンス
        private static readonly StateDisplay stateDisplay = new StateDisplay();         // リザルトのUI表示
        #endregion

        enum E_DISP_STATE
        {
            DARK,
            UI,
            VALUE,
            RANK,

            MAX_DISP_STATE,
        }


        #region プライベート変数       
        private bool okClick = false;               
        private E_DISP_STATE disp = E_DISP_STATE.DARK;          // 表示判定用
        private Vector3 rectSize = new Vector3(8, 8, 8);

        #endregion

        #region パブリック変数
        #endregion

        #region プライベート関数
        /// <summary>
        /// 現在のステート
        /// </summary>
        private ResultStateBase currentState = stateDisplay;

        /// <summary>
        /// Start()から呼ばれる
        /// </summary>
        private void OnStart()
        {          
            panelCanvas.alpha = 0.0f;
            uiCanvas.alpha    = 0.0f;
            valueCanvas.alpha = 0.0f;
            rankCanvas.alpha  = 0.0f;
            baseRect.localScale = rectSize;
            once = false;
            currentState.OnEnter(this, null);
        }


        /// <summary>
        /// Update()から呼ばれる
        /// </summary>
        private void OnUpdate()
        {
            if (input.Menu.Cancel.triggered)
            {              
                OnClickClose();
            }


            currentState.OnUpdate(this);
            
        }

        /// <summary>
        /// FixedUpdate()から呼ばれる
        /// </summary>
        private void OnFixedUpdate()
        {
            currentState.OnFixUpdate(this);
        }

        /// <summary>
        /// ステートの変更
        /// </summary>
        private void ChangeState(ResultStateBase nextState)
        {
            if (currentState != nextState)
            {
                currentState.OnExit(this, nextState);
                nextState.OnEnter(this, currentState);
                currentState = nextState;
            }
        }


        #endregion

        #region パブリック関数
        public void OnClickClose()
        {
            if(okClick)
            {
                buttonRect.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                TransitionManager.Instance.StartTransaction(Scenes.Title, null, true);
                //Mizuno.SoundManager.Instance.StopBGM();
            }
           
        }
        #endregion
    }
}

