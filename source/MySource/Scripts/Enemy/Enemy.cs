/////////////////////////////////////////////////////////////////////////
// 作成日 2021/03/16
// 作成者 柳沢帆貴
//
// Enemy.cs
// エネミーの親クラス
// エネミーに共通するパラメータ、プロパティをここに実装
// 共通する処理はEnemy_Common.csに分割して実装
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/03/16 作成開始
// 2021/03/18 ソースコードを分割して作成、 ステートパターンで状態管理
// 2021/03/18 待機、移動状態作成
// 
// 2021/04/22 再構築by三上
//
//////////////////////////////////////////////////////////////////////////


using UnityEngine;


namespace Homare
{
    public partial class Enemy
    {
        // 継承先で書き換えor中身を用意する必要のある変数
        protected EnemyStateBase currentState;
        protected BehaviorTreeController behavior;
        public virtual bool IsDead { set; get; }

        //public GameObject creater;
        [System.NonSerialized]public EnemyCreater creater;

        // 状態を処理（表現)するのに必要な変数
        public EnemyStatus enemyStatus;
        public static GameObject player { get; private set; }
        public Rigidbody rb { get; private set; }
        public Animator animator { get; private set; }
        public AudioSource audioSource { get; private set; }
        [System.NonSerialized] public float actionCnt;
        [System.NonSerialized] public Vector3 firstPos;
        [System.NonSerialized] public bool isEscape;      //エスケープゾーンに来たらフラグ立てる
        [System.NonSerialized] public bool isLanding;
        [System.NonSerialized] public Transform escapeTransform;
        public int attackTime;
        public float alpha = 1;    //エスケープ時に消える時の透過値

        private float _Hp;
        public float Hp
        {
            get => _Hp;
            set
            {
                _Hp = Mathf.Min(value, enemyStatus.maxHp);
                if (_Hp <= 0)
                {
                    _Hp = 0;
                    Death();
                }
            }
        }

        private bool _isWeapon;
        public bool isWeapon
        {
            get => _isWeapon;
            set
            {
                _isWeapon = value;
                if (!_isWeapon)
                {
                    Escape();
                }
            }
        }
    }
}