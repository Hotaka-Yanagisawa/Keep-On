using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.Controls;

#region HeaderComent
//==================================================================================
// Player_Common
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
    public partial class Player : MonoBehaviour
    {

        #region インスペクター
      
        /// <summary>
        /// インスペクターでまとめるやつ(ここでは表示していない)
        /// </summary>
       
        #region シリアライザブルでまとめてる
        [System.Serializable]
        class BaseParam
        {
            [Header("基本のステータス")]
            [Tooltip("スタイル")]
            [SerializeField] public Style style;

            [Tooltip("サテライト")]
            [SerializeField] public GameObject satellite;

            [Tooltip("最大のHP")]
            [SerializeField] public float maxHp;

            [Tooltip("現在のHP")]
            [SerializeField] public float currentHp;

            [Tooltip("最大のスタミナ")]
            [SerializeField] public float maxSp;

            [Tooltip("現在のスタミナ")]
            [SerializeField] public float currentSp;

            [Tooltip("武器を持つ最大経過時間")]
            [SerializeField] public float maxHavingWeapon;

            [Tooltip("現在の武器を持っている継続時間")]
            [SerializeField] public float currentHavingWeapon;

            [Tooltip("歩く速度")]
            [SerializeField] public int moveWalk = 3;

            [Tooltip("走る速度")]
            [SerializeField] public int moveDash = 5;

            [Tooltip("ステップ後の硬直フレーム数")]
            [SerializeField] public int stepStun = 60;

            [Tooltip("ステップの間のインターバルfloat型秒数管理")]
            [SerializeField] public float stepInterval;

            [Tooltip ("走りと歩きのスティック入力の境界値")]
            [SerializeField] public float stickBoundaryValue = 0.8f;

            [Tooltip ("HPバー")]
            [SerializeField] public GameObject hpBar;

            [Tooltip("スタミナバー")]
            [SerializeField] public GameObject spBar;

            [Tooltip("コンボバー")]
            [SerializeField] public GameObject combobar;

            [Tooltip("コンボの回復量")]
            [SerializeField] public float addHealTime;
        }

        [System.Serializable]
        class NormalParam
        {
            [Tooltip("連続ステップ回数")]
            [SerializeField] public int maxStep;

            [Tooltip("ジャンプの高さ")]
            [SerializeField] public float jumpPower = 5f;

            [Tooltip("ステップの距離(フレーム数で管理)")]
            [SerializeField] public int stepRange = 30;
        }

        [System.Serializable]
        class PowerParam
        {
            [Tooltip("連続ステップ回数")]
            [SerializeField] public int maxStep;

            [Tooltip("ジャンプの高さ")]
            [SerializeField] public float jumpPower = 5f;

            [Tooltip("ステップの距離(フレーム数で管理)")]
            [SerializeField] public int stepRange = 30;
        }

        [System.Serializable]
        class MobilityParam
        {
            [Tooltip("連続ステップ回数")]
            [SerializeField] public int maxStep;

            [Tooltip("ジャンプの高さ")]
            [SerializeField] public float jumpPower = 7f;

            [Tooltip("ステップの距離(フレーム数で管理)")]
            [SerializeField] public int stepRange = 30;
        }

        [System.Serializable]
        class ReachParam
        {
            [Tooltip("連続ステップ回数")]
            [SerializeField] public int maxStep;

            [Tooltip("ジャンプの高さ")]
            [SerializeField] public float jumpPower = 5f;

            [Tooltip("ステップの距離(フレーム数で管理)")]
            [SerializeField] public int stepRange = 30;
        }

        [SerializeField] private GameObject sotai;

        [SerializeField] private GameObject DamageFX;

        [SerializeField] private ManageGame manage=null;

        #endregion


        /// <summary>
        /// インスペクターに表示するもの
        /// </summary>

        #region まとめたものを表示
        [Header("基本ステータス")]
        [SerializeField] private BaseParam baseParam;

        [Header("素体のステータス")]
        [SerializeField] private NormalParam normal;

        [Header("火力型のステータス")]
        [SerializeField] private PowerParam power;

        [Header("機動型のステータス")]
        [SerializeField] private MobilityParam mobility;

        [Header("範囲型のステータス")]
        [SerializeField] private ReachParam reach;
        #endregion
        
        #endregion

        #region プライベート変数
        //private float _Hp;
        //private Vector3 defaultScale;
        //private Material materialInstance = null;
        private Rigidbody rigidBody;
        private Animator animator;
        private Material hpmaterial;
        private Material spmaterial;
        private Material combomaterial;

        #endregion
        public PlayerControler input;		// 勝手にパブリックにしたよ。ごめんねby柳沢(大平)
        public bool isDecraseComboGauge;    // コンボゲージを減らすか 
        #region パブリック変数
        //public float MaxHp => maxHp;
        //public const string Tag = "Player";
        #endregion

        #region デリゲート？
        public System.Action OnDeathAction;
        #endregion

        #region プライベート関数


        /// <summary>
        /// 最初に呼ばれる関数
        /// </summary>
        private void Start()
        {
            input = new PlayerControler();
            rigidBody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            hpmaterial = baseParam.hpBar.GetComponent<Image>().material;
            spmaterial = baseParam.spBar.GetComponent<Image>().material;
            combomaterial = baseParam.combobar.GetComponent<Image>().material;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyWeapon"), false);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
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


        /// <summary>
        /// オブジェクトを消す関数
        /// </summary>
        private void OnDestroy()
        {
            //Destroy(materialInstance);
        }


        /// <summary>
        /// プレイヤーが死亡したとき呼ばれる関数
        /// </summary>
        private void Death()
        {
            OnDeath();
            OnDeathAction?.Invoke();
        }



        #region 敵との当たり判定


        /// <summary>
        /// 当たり判定関数
        /// 触れた時に呼ばれる
        /// </summary>
        


        /// <summary>
        /// 当たり判定関数
        /// 触れている間呼ばれる
        /// </summary>
        private void OnTriggerStay(Collider other)
        {
            //if (other.TryGetComponent(out Enemy enemy))
            //{
            //    Hp -= enemy.Damage * Time.deltaTime;
            //}
        }


        /// <summary>
        /// 当たり判定関数
        /// 触れてから離れた時に呼ばれる
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            //materialInstance.color = Color.blue;
        }


        #endregion
        #endregion

        #region パブリック関数
        public void OnHitEnemyAttack(float damage, Vector3 pos)
        {

            if (animator.GetBool("jump"))
                animator.SetInteger("knockbackType", 1);
            else
                animator.SetInteger("knockbackType", 0);

            baseParam.currentHp -= damage;
            GameObject fx = Instantiate(DamageFX, pos, Quaternion.identity);
            Destroy(fx, 3f);
            
        }

        public void OnHitBossAttack(float damage, Vector3 pos)
        {
            animator.SetInteger("knockbackType", 1);
            baseParam.currentHp -= damage;
            GameObject fx = Instantiate(DamageFX, pos, Quaternion.identity);
            Destroy(fx, 3f);
        }

        public void OnStartComboTime()
        {
            baseParam.currentHavingWeapon = baseParam.maxHavingWeapon;
            manage?.OnCntCombo();
            // コンボカウントを+させる
        }

        public void OnKnockBack()
        {
            ChangeState(stateKnockBack);
        }

        public void OnHeal(float heal)
        {
            // 体力の回復
            baseParam.currentHp += heal;

            // 上限設定
            if(baseParam.currentHp > baseParam.maxHp)
            {
                baseParam.currentHp = baseParam.maxHp;
            }

            // コンボのゲージの回復
            if(baseParam.style.style != Style.E_Style.NORMAL)
            baseParam.currentHavingWeapon += baseParam.addHealTime;

            // 上限設定
            if(baseParam.currentHavingWeapon > baseParam.maxHavingWeapon)
            {
                baseParam.currentHavingWeapon = baseParam.maxHavingWeapon;
            }

            gameObject.GetComponent<EffectOperate>().PlayEffect(0);

            PlaySE("SE_Heal");
        }


       public float GetHp()
        {
            //KeyControl key;
            //key.rele
            return baseParam.currentHp;
        }
        #endregion
    }
}

