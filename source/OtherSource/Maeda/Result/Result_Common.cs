using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Homare;

#region HeaderComent
//==================================================================================
// Result_Common
//	ステート以外の処理の部分
// 作成日時	:2021/03/20
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/03/20   サンプルから必要ないものにコメントアウト    
//              参考になるかもしれないので消さないでおく
// 2021/03/31   インスペクターから編集できるように追加
//==================================================================================
#endregion

namespace Maeda
{
    /// <summary>
    /// ステート以外の処理　Start(),Update()や当たり判定関数等はここ
    /// </summary>
    public partial class Result : MonoBehaviour
    {

        #region インスペクター
        [Tooltip ("Manager")]
        [SerializeField] private ManageGame manage;

        [Tooltip("Panel")]
        [SerializeField] private GameObject panel;

        [Tooltip("UI")]
        [SerializeField] private GameObject baseUi;
        [SerializeField] private GameObject baseFrame;

        [Tooltip("Value")]
        [SerializeField] private GameObject value;

        [Tooltip("Rank")]
        [SerializeField] private GameObject rank;

        [Tooltip("SpriteNum")]
        [SerializeField] private GameObject num;

        [SerializeField] private GameObject[] score;

        [SerializeField] private GameObject[] result;

        [SerializeField] private TextMeshProUGUI text;

        [SerializeField] private Boss boss;

        [SerializeField] private GameObject button;
     


        #endregion

        #region プライベート変数
        private int nDefeatedEnemies;               // 敵の撃破数
        private int nTotalCombo;                    // 合計コンボ数
        private int nMaxCombo;                      // 最大連続コンボ数
        private int nComparisonMaxCombo;            // 最大連続コンボ数の比較用

        private CanvasGroup panelCanvas;                   // パネルのα値変更用
        private CanvasGroup uiCanvas;                      // UI表示のα値変更用
        private CanvasGroup valueCanvas;                   // スコアのα値変更用
        private CanvasGroup rankCanvas;                    // ランクのα値変更用
        private RectTransform baseRect;                    // ベースのサイズ変更用
        private RectTransform buttonRect;                  // ボタンのサイズ変更用
        #endregion

        #region パブリック変数
        public PlayerControler input;

        #endregion

        #region プライベート関数

        // アクション
        public event Action ChangeFlags;

        /// <summary>
        /// 最初に呼ばれる関数
        /// </summary>
        private void Start()
        {
            panelCanvas = panel.GetComponent<CanvasGroup>();
            uiCanvas = baseFrame.GetComponent<CanvasGroup>();
            valueCanvas = value.GetComponent<CanvasGroup>();
            rankCanvas = rank.GetComponent<CanvasGroup>();

            baseRect = baseUi.GetComponent<RectTransform>();
            buttonRect = button.GetComponent<RectTransform>();
            input = new PlayerControler();
            input.Enable();
            OnStart();
        }


        /// <summary>
        /// 毎フレーム呼ばれる関数
        /// </summary>
        private void Update()
        {
            OnUpdate();
        }


        /// <summary>
        /// 一定フレームごと呼ばれる関数
        /// </summary>
        private void FixedUpdate()
        {
            OnFixedUpdate();
        }

        #endregion

    
    }
}
