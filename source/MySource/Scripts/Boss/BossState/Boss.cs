/////////////////////////////////////////////////////////////////////////
// 作成日 2021/03/28
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/03/28 作成開始
//             ステートマシンクラスを用いてBossAIを作成
// 
//
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.VFX;
//using UnityEngine.InputSystem;


namespace Homare
{
using State = StateMachine<Boss>.State;
    public partial class Boss : MonoBehaviour
    {
        private StateMachine<Boss> stateMachine;

        private enum Event : int
        {
            // 時間切れ
            Timeout,
            // 追跡
            Tracking,
            //追跡範囲外
            OutOfRange,
            // 攻撃
            Attack,
            //突進
            Lunge,
            // 攻撃範囲外
            OutOfAttackRange,
            // Down状態になったら
            Down,
            // 起き上がり
            GettingUp,
            // 残り1体になった
            Death,
        }

        public enum BossType
        {
            Reach,
            Speed,
            Power,
            Non
        }

        enum BossAction
        {
            Non,
            Atk,
            Fly,
            Down,
        }

        [SerializeField] private SphereCollider searchArea;
        [SerializeField] [Tooltip("視野の半径")] private float sphereRadius;
        [Header("searchAngle * 2 = 視野の角度")]
        [SerializeField] private float searchAngle = 180f;
        [SerializeField] private float atkAngle = 30f;
        private Vector3 firstPos;
        private ReachBehavior reachBehavior;
        private SpeedBehavior speedBehavior;
        BossAction bossAction;

        float playerAtkNum;
        [Header("攻撃回数（パワー）")]
        [SerializeField] int playerAtkCapa;
        float speedDamageNum;
        float reachDamageNum;
        [Header("ダメージ（スピード）")]
        [SerializeField] float speedDamageCapa;
        [Header("ダメージ（リーチ）")]
        [SerializeField] float reachDamageCapa;
        [SerializeField] float downTime = 10;
        [SerializeField] private float typeMagnification = 3;

        //private PowerBehavior powerBehavior;


        [SerializeField] private float maxHp = 5f;
        [SerializeField] private float applySpeed;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveRange;
        //[SerializeField] private int powerDownNum = 20;
        [SerializeField] private ManageGame manage = null;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Animator animator;
        [SerializeField] private BossType type;
        [SerializeField] private Style reachStyle;
        [SerializeField] private Style speedStyle;
        [SerializeField] private Style powerStyle;
        //[SerializeField] private Extinction extinction = null;
        [SerializeField] private DivisionExtinction divisionExtinction;
        [SerializeField] private Arc arc;
        [SerializeField] private BossWeapon bossWeapon;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject wind;
        [SerializeField] private GameObject fallDownAtk;
        [SerializeField] private GameObject reachWeapon;
        [SerializeField] private GameObject powerWeapon;
        [SerializeField] private Collider downCollider;
        [SerializeField] private Collider bodyCollider;
        [SerializeField] private Collider lArmCollider;
        [SerializeField] private Collider rArmCollider;
        [SerializeField] private AudioClip footAudioClip1;
        [SerializeField] private AudioClip footAudioClip2;
        [SerializeField] private AudioClip footAudioClip3;
        [SerializeField] private AudioClip atkAudioClip1;
        [SerializeField] private AudioClip atkAudioClip2; //jump

        [SerializeField] public AudioSource audio = null;
        [SerializeField] public AudioClip audioClip;
        [SerializeField] public Style playerStyle;

        GameObject canvas;
        DamageText damageText;

        int atkCnt;
        [SerializeField] int powerAtkNum = 1;
        [SerializeField] int reachAtkNum = 1;
        [SerializeField] int spdAtkNum = 1;
        [SerializeField] public VisualEffect vEffect;
        [SerializeField] public StyleHolder styleHolder;

        [SerializeField] public VisualEffect startBombing;
        public bool isAtk { private set; get; }
        public bool IsDead { private set; get; }

        public bool isWeapon;
        public bool isDown;
        public float MaxHp => maxHp;

        private float _Hp;
        public float Hp
        {
            get => _Hp;
            set
            {
                _Hp = Mathf.Min(value, maxHp);
                if (_Hp <= 0)
                {
                    _Hp = 0;
                    IsDead = true;
                    Death();
                }
            }
        }


        private void Start()
        {
            Physics.IgnoreCollision(downCollider, bodyCollider, true);
            Physics.IgnoreCollision(downCollider, lArmCollider, true);
            Physics.IgnoreCollision(downCollider, rArmCollider, true);

            switch (type)
            {
                case BossType.Reach:
                    styleHolder.style = reachStyle;
                    styleHolder.style.style = Style.E_Style.REACH;
                    atkCnt = reachAtkNum;

                    break;
                case BossType.Speed:
                    styleHolder.style = speedStyle;
                    styleHolder.style.style = Style.E_Style.MOBILITY;
                    atkCnt = spdAtkNum;

                    break;
                case BossType.Power:
                    styleHolder.style = powerStyle;
                    styleHolder.style.style = Style.E_Style.POWER;
                    atkCnt = powerAtkNum;

                    break;
            }
            ChangeWeapon();
            BossHitCollider.bossCS = this;
            Steam.bossCS = this;
            Change.boss = this;
            playerAtkNum = 0;
            speedDamageNum = 0;
            reachDamageNum = 0;
            isDown = false;
            isWeapon = true;
            IsDead = false;
            canvas = GameObject.Find("DamageUICanvas");
            Hp = maxHp;
            firstPos = transform.position;
            stateMachine = new StateMachine<Boss>(this);
            //攻撃⇒待機 条件：攻撃範囲外
            stateMachine.AddTransition<BossStateAttacking, BossStateWaiting>((int)Event.OutOfAttackRange);
            //待機⇒攻撃　条件：攻撃範囲内
            stateMachine.AddTransition<BossStateWaiting, BossStateAttacking>((int)Event.Attack);
            //攻撃⇒ダウン　条件：ダウンしたら
            stateMachine.AddTransition<BossStateAttacking, BossStateDown>((int)Event.Down);
            //ダウン⇒待機　条件：起き上がったらしたら
            stateMachine.AddTransition<BossStateDown, BossStateWaiting>((int)Event.GettingUp);

            //どこからでも⇒死亡 条件：HP0
            stateMachine.AddAnyTransition<BossStateDead>((int)Event.Death);

            //待機ステートから始まる
            //stateMachine.Start<BossStateAttacking>();
            stateMachine.Start<BossStateWaiting>();
            //stateMachine.Start<BossStateMoving>();

            PowerCurState = moveState;
            ReachCurState = centerMoveState;
            SpeedCurState = jumpMoveState;
            bossAction = BossAction.Non;

            //ビヘイビアツリーを作成、初期化
            //reachBehavior = new ReachBehavior(this);
            //reachBehavior.OnStart();

            //speedBehavior = new SpeedBehavior(this);
            //speedBehavior.OnStart();

            //powerBehavior = new PowerBehavior(this);
            //powerBehavior.OnStart();
            animator.SetInteger("BossType", (int)type);

            animator.SetBool("Change", true);
        }

        private void Update()
        {
            stateMachine?.Update();
        }

        private void FixedUpdate()
        {
            stateMachine?.fixedUpdate();
        }

        void Down(float damage)
        {
            float magnification = 1;
            if (isDown) return;
            switch (type)
            {
                case BossType.Reach:
                    if (bossAction == BossAction.Atk)
                    {
                        if (playerStyle.style == Style.E_Style.POWER) magnification = typeMagnification;
                        reachDamageNum += damage * magnification;
                        if (reachDamageNum >= reachDamageCapa)
                        {
                            reachDamageNum = 0;
                            isDown = true;
                            bossAction = BossAction.Down;
                        }
                    }
                    break;

                case BossType.Speed:
                    if (bossAction == BossAction.Fly)
                    {
                        if (playerStyle.style == Style.E_Style.REACH) magnification = typeMagnification;
                        speedDamageNum += damage * magnification;
                        if (speedDamageNum >= speedDamageCapa)
                        {
                            speedDamageNum = 0;
                            isDown = true;
                            bossAction = BossAction.Down;
                            fallDownAtk.SetActive(false);
                            fallDownAtk.GetComponent<FallDownAtk>().enabled = false;
                        }
                    }
                    break;

                case BossType.Power:
                    if (bossAction == BossAction.Atk)
                    {
                        if (playerStyle.style == Style.E_Style.MOBILITY) magnification = typeMagnification;
                        playerAtkNum += 1 * magnification;

                        if (playerAtkNum >= playerAtkCapa)
                        {
                            playerAtkNum = 0;
                            isDown = true;
                            bossAction = BossAction.Down;
                        }
                    }
                    break;
            }
        }

        public void OnHitPlayerAttack(float damage, Vector3 hitPos)
        {
            Hp -= damage;
            Down(damage);
            //ダメージテキストの表示
            foreach (Transform t in canvas.transform)
            {
                if (!t.gameObject.activeSelf)
                {
                    damageText = t.GetComponent<DamageText>();
                    t.gameObject.SetActive(true);
                    break;
                }
            }
            damageText?.GetDrawPos(hitPos + new Vector3(0, 1.5f, 0), damage);
        }
        public bool OnHitPlayerSteal()
        {
            if (isDown)
            {
                isDown = false;
                return true;
            }
            return false;
        }
        // 死亡
        private void Death()
        {
            OnDeath();
            Ohira.CameraController.Instance.RemoveLockOn(gameObject);
        }

        private void OnDeath()
        {
            stateMachine.Dispatch((int)Event.Death);
        }

        public void ChangeWeapon()
        {

            switch (type)
            {
                case BossType.Reach:
                    powerWeapon.SetActive(false);
                    reachWeapon.SetActive(true);
                    break;
                case BossType.Speed:
                    powerWeapon.SetActive(false);
                    reachWeapon.SetActive(false);
                    break;
                case BossType.Power:
                    powerWeapon.SetActive(true);
                    reachWeapon.SetActive(false);
                    break;
            }
        }

        private void ReachStateMachine()
        {
            #region リーチ
            styleHolder.style = reachStyle;
            styleHolder.style.style = Style.E_Style.REACH;
            atkCnt = reachAtkNum;

            stateMachine = new StateMachine<Boss>(this);

            //攻撃⇒待機 条件：攻撃範囲外
            stateMachine.AddTransition<BossStateAttacking, BossStateWaiting>((int)Event.OutOfAttackRange);
            //待機⇒攻撃　条件：攻撃範囲内
            stateMachine.AddTransition<BossStateWaiting, BossStateAttacking>((int)Event.Attack);
            //攻撃⇒ダウン　条件：ダウンしたら
            stateMachine.AddTransition<BossStateAttacking, BossStateDown>((int)Event.Down);
            //ダウン⇒待機　条件：起き上がったらしたら
            stateMachine.AddTransition<BossStateDown, BossStateWaiting>((int)Event.GettingUp);

            //どこからでも⇒死亡 条件：HP0
            stateMachine.AddAnyTransition<BossStateDead>((int)Event.Death);
            #endregion
            //待機ステートから始まる
            stateMachine.Start<BossStateWaiting>();

            ChangeReachState(centerMoveState);
            //ReachCurState = centerMoveState;
        }

        private void SpeedStateMachine()
        {
            #region スピード
            styleHolder.style = speedStyle;
            styleHolder.style.style = Style.E_Style.MOBILITY;
            atkCnt = spdAtkNum;
            stateMachine = new StateMachine<Boss>(this);

            //攻撃⇒待機 条件：攻撃範囲外
            stateMachine.AddTransition<BossStateAttacking, BossStateWaiting>((int)Event.OutOfAttackRange);
            //待機⇒攻撃　条件：攻撃範囲内
            stateMachine.AddTransition<BossStateWaiting, BossStateAttacking>((int)Event.Attack);
            //攻撃⇒ダウン　条件：ダウンしたら
            stateMachine.AddTransition<BossStateAttacking, BossStateDown>((int)Event.Down);
            //ダウン⇒待機　条件：起き上がったらしたら
            stateMachine.AddTransition<BossStateDown, BossStateWaiting>((int)Event.GettingUp);

            //どこからでも⇒死亡 条件：HP0
            stateMachine.AddAnyTransition<BossStateDead>((int)Event.Death);
            #endregion
            //待機ステートから始まる
            stateMachine.Start<BossStateWaiting>();
            ChangeSpeedState(jumpMoveState);
            //SpeedCurState = jumpMoveState;

        }

        private void PowerStateMachine()
        {
            #region パワー
            styleHolder.style = powerStyle;
            styleHolder.style.style = Style.E_Style.POWER;
            atkCnt = powerAtkNum;
            stateMachine = new StateMachine<Boss>(this);

            //攻撃⇒待機 条件：攻撃範囲外
            stateMachine.AddTransition<BossStateAttacking, BossStateWaiting>((int)Event.OutOfAttackRange);
            //待機⇒攻撃　条件：攻撃範囲内
            stateMachine.AddTransition<BossStateWaiting, BossStateAttacking>((int)Event.Attack);
            //攻撃⇒ダウン　条件：ダウンしたら
            stateMachine.AddTransition<BossStateAttacking, BossStateDown>((int)Event.Down);
            //ダウン⇒待機　条件：起き上がったらしたら
            stateMachine.AddTransition<BossStateDown, BossStateWaiting>((int)Event.GettingUp);

            //どこからでも⇒死亡 条件：HP0
            stateMachine.AddAnyTransition<BossStateDead>((int)Event.Death);
            #endregion
            //待機ステートから始まる
            stateMachine.Start<BossStateWaiting>();
            ChangePowerState(moveState);
            //PowerCurState = moveState;
        }

        private void ChangeStateMachine(BossType newType)
        {
            stateMachine.ExitState();
            isAtk = false;
            type = newType;

            switch (newType)
            {
                case BossType.Reach:
                    ReachStateMachine();
                    break;
                case BossType.Speed:
                    SpeedStateMachine();
                    break;
                case BossType.Power:
                    PowerStateMachine();
                    break;
            }
            animator.SetInteger("BossType", (int)newType);
            animator.SetBool("Change", true);
        }
        private void RandomChangeStateMachine()
        {
            BossType newType = (BossType)Random.Range(0, 3);
            
            while (type == newType)
            {
                newType = (BossType)Random.Range(0, 3);
            }
            type = newType;

            switch (newType)
            {
                case BossType.Reach:
                    ReachStateMachine();
                    break;
                case BossType.Speed:
                    SpeedStateMachine();
                    break;
                case BossType.Power:
                    PowerStateMachine();
                    break;
            }
        }

        public void DownToStandUp()
        {
            switch (type)
            {
                case BossType.Reach:                   
                    styleHolder.style = reachStyle;
                    break;
                case BossType.Speed:                    
                   styleHolder.style = speedStyle;

                    break;
                case BossType.Power:                  
                   styleHolder.style = powerStyle;

                    break;
            }
        }



        private void OnTriggerStay(Collider other)
        {
            if (other.tag != "Player") return;
            if (animator.GetBool("Change")) return;
            //　主人公の方向
            Vector3 playerDirection = other.transform.position - transform.position;
            //　敵の前方からの主人公の方向
            float angle = Vector3.Angle(transform.forward, playerDirection);
            //　サーチする角度内だったら軸合わせ
            if (angle <= searchAngle)
            {
                //　サーチする角度内だったら攻撃
                if (angle <= atkAngle)
                {
                    if (Random.Range(0, 101) < 10)
                    {
                        //攻撃状態へ
                        isAtk = true;
                    }
                }
               
            }
        }

#if UNITY_EDITOR
        //　サーチする角度表示
        private void OnDrawGizmos()
        {
            if (searchArea.enabled)
            {
                UnityEditor.Handles.color = new Color(0.9f, 0.3f, 0, 0.05f);
                UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius* 1.5f);
                UnityEditor.Handles.color = new Color(0.0f, 0.5f, 0.5f, 0.05f);
                UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -atkAngle, 0f) * transform.forward, atkAngle * 2f, searchArea.radius * 1.5f);

            }
        }
#endif

        //アニメーションイベント
        //アニメーションの開始と同時にColliderを出す
        void AttackStart()
        {
            bossWeapon.bladeCollider.enabled = true;
            bossWeapon.handleCollider.enabled = true;
        }

        //アニメーションイベント
        //アニメーションの終了と同時にColliderを消す
        //アニメーションを攻撃から待機へ戻す
        void AttackEnd()
        {
            bossWeapon.bladeCollider.enabled = false;
            bossWeapon.handleCollider.enabled = false;
            //animator.SetBool("Attack", false);
        }

        void FootSE()
        {
            switch(Random.Range(0, 3))
            {
                case 0:
                    audio.PlayOneShot(footAudioClip1);
                    break;
                case 1:
                    audio.PlayOneShot(footAudioClip2);
                    break;
                case 2:
                    audio.PlayOneShot(footAudioClip3);
                    break;
            }
        }

        void PowerSE()
        {
            audio.PlayOneShot(atkAudioClip1);
        }
        void JumpSE()
        {
            audio.PlayOneShot(atkAudioClip2);
        }


        public void ToTrackingState()
        {
            if (!animator.GetBool("Attack"))
                stateMachine.Dispatch((int)Event.Tracking);
        }

        public void FromAttackToWait()
        {
            stateMachine.Dispatch((int)Event.OutOfAttackRange);
        }

        public void FromTrackingToWait()
        {
            stateMachine.Dispatch((int)Event.OutOfRange);
        }

        public void FromTrackingToAttack()
        {
            stateMachine.Dispatch((int)Event.Attack);
        }
    }
}

////待機⇒移動 条件：タイムアウト
////stateMachine.AddTransition<BossStateWaiting, BossStateMoving>((int)Event.Timeout);
////移動⇒待機 条件：タイムアウト
////stateMachine.AddTransition<BossStateMoving, BossStateWaiting>((int)Event.Timeout);
////追跡⇒待機 条件：視野範囲外
////stateMachine.AddTransition<BossStateTracking, BossStateWaiting>((int)Event.OutOfRange);
////stateMachine.AddTransition<BossStateTracking, BossStateMoving>((int)Event.OutOfRange);
////攻撃⇒待機 条件：攻撃範囲外
//stateMachine.AddTransition<BossStateAttacking, BossStateWaiting>((int) Event.OutOfAttackRange);
//            //追跡⇒攻撃 条件：攻撃範囲内
//            //stateMachine.AddTransition<BossStateTracking, BossStateAttacking>((int)Event.Attack);
//            //待機⇒攻撃　条件：攻撃範囲内
//            stateMachine.AddTransition<BossStateWaiting, BossStateAttacking>((int) Event.Attack);
//            //攻撃⇒ダウン　条件：ダウンしたら
//            stateMachine.AddTransition<BossStateAttacking, BossStateDown>((int) Event.Down);
//            //ダウン⇒待機　条件：起き上がったらしたら
//            stateMachine.AddTransition<BossStateDown, BossStateWaiting>((int) Event.GettingUp);
//            //待機⇒追跡 条件：視野範囲内
//            //stateMachine.AddTransition<BossStateWaiting, BossStateTracking>((int)Event.Tracking);
//            //移動⇒追跡 条件：視野範囲内
//            //stateMachine.AddTransition<BossStateMoving, BossStateTracking>((int)Event.Tracking);
//            //追跡⇒突進 条件：？？？？
//            //stateMachine.AddTransition<BossStateTracking, BossStateLungeTracking>((int)Event.Lunge);
//            //突進⇒待機 条件：攻撃範囲外
//            //stateMachine.AddTransition<BossStateLungeTracking, BossStateWaiting>((int)Event.Timeout);