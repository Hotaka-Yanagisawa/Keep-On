/////////////////////////////////////////////////////////////////////////
// 作成日 2021/04/02
// 作成者 柳沢帆貴
// PBO ⇒ PowerBossOne
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/04/02 作成開始
//            BossScript関係の汎用的な行動AIをコピペ
//            攻撃の選択肢を乱数で選ぶようにした
//
//
//////////////////////////////////////////////////////////////////////////


using UnityEngine;

using State = StateMachine<PBO>.State;

public partial class PBO : MonoBehaviour
{
    private StateMachine<PBO> stateMachine;

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
        //風圧
        Wind,
        //攻撃終了
        AtkEnd,
        // 攻撃範囲外
        OutOfAttackRange,
        // 残り1体になった
        Death,
    }

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float applySpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveRange;
    [SerializeField] private Animator animator;
    [SerializeField] private CapsuleCollider capsuleCollider;//剣のやつ多分消す
    [SerializeField] private SphereCollider sphereCollider; //風圧用
    [SerializeField] private BoxCollider boxCollider;       //突進攻撃用
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject slash;              //飛ぶ斬撃敵なの

    private PBOBehavior behavior;
    private Vector3 firstPos;
    
    [SerializeField] private float maxHp = 5f;
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
                Death();
            }
        }
    }

    private void Start()
    {
        stateMachine = new StateMachine<PBO>(this);
        
        firstPos = transform.position;
        boxCollider.enabled = false;

        //攻撃後隙⇒待機 条件：タイムアウト
        stateMachine.AddTransition<PBOStateAtkDelay, PBOStateWaiting>((int)Event.Timeout);
        //待機⇒移動 条件：タイムアウト
        stateMachine.AddTransition<PBOStateWaiting, PBOStateMoving>((int)Event.Timeout);
        //移動⇒待機 条件：タイムアウト
        stateMachine.AddTransition<PBOStateMoving, PBOStateWaiting>((int)Event.Timeout);
        //追跡⇒待機 条件：視野範囲外
        stateMachine.AddTransition<PBOStateTracking, PBOStateWaiting>((int)Event.OutOfRange);
        //stateMachine.AddTransition<PBOStateTracking, PBOStateMoving>((int)Event.OutOfRange);
        //攻撃⇒待機 条件：攻撃範囲外
        stateMachine.AddTransition<PBOStateAttacking, PBOStateWaiting>((int)Event.OutOfAttackRange);
        //攻撃⇒待機 条件：攻撃モーション終わり
        stateMachine.AddTransition<PBOStateAttacking, PBOStateAtkDelay>((int)Event.AtkEnd);
        //追跡⇒攻撃 条件：攻撃範囲内
        stateMachine.AddTransition<PBOStateTracking, PBOStateAttacking>((int)Event.Attack);
        ////待機⇒攻撃　条件：攻撃範囲内
        //stateMachine.AddTransition<PBOStateWaiting, PBOStateAttacking>((int)Event.Attack);
        //待機⇒追跡 条件：視野範囲内
        stateMachine.AddTransition<PBOStateWaiting, PBOStateTracking>((int)Event.Tracking);
        //移動⇒追跡 条件：視野範囲内
        stateMachine.AddTransition<PBOStateMoving, PBOStateTracking>((int)Event.Tracking);
        //追跡⇒突進攻撃 条件：？？？？
        stateMachine.AddTransition<PBOStateTracking, PBOStateLungeAtk>((int)Event.Lunge);
        //突進攻撃⇒待機 条件：タイムアウト
        stateMachine.AddTransition<PBOStateLungeAtk, PBOStateAtkDelay>((int)Event.Timeout);
        //追跡⇒風圧 条件：攻撃範囲内(風圧)
        stateMachine.AddTransition<PBOStateTracking, PBOStateWindAtk>((int)Event.Wind);
        //風圧⇒待機 条件：タイムアウト
        stateMachine.AddTransition<PBOStateWindAtk, PBOStateAtkDelay>((int)Event.Timeout);

        //どこからでも⇒死亡 条件：HP0
        stateMachine.AddAnyTransition<PBOStateDead>((int)Event.Death);

        //待機ステートから始まる
        stateMachine.Start<PBOStateWaiting>();

        //ビヘイビアツリーを作成、初期化
        behavior = new PBOBehavior(this);
        behavior.OnStart();
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.fixedUpdate();
    }

    public void OnHitPlayerAttack(float damage)
    {
        Hp -= damage;
    }

    // 死亡
    private void Death()
    {
        OnDeath();
        transform.GetChild(0).gameObject.GetComponent<SphereCollider>().enabled = false;
        transform.GetChild(1).gameObject.GetComponent<SphereCollider>().enabled = false;
    }

    private void OnDeath()
    {
        stateMachine.Dispatch((int)Event.Death);
    }

    //アニメーションイベント
    //アニメーションの開始と同時にColliderを出す
    void AttackStart()
    {
        capsuleCollider.enabled = true;
    }

    void Slash()
    {
        //slash.SetActive(true);
    }

    //アニメーションイベント
    //アニメーションの終了と同時にColliderを消す
    //アニメーションを攻撃から待機へ戻す
    void AttackEnd()
    {
        capsuleCollider.enabled = false;
        animator.SetBool("Attack", false);
        stateMachine.Dispatch((int)Event.AtkEnd);
        Debug.Log(stateMachine.CurrentState);
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


        //    float Distance = Vector3.Distance(transform.position, player.transform.position);

        //    //小射程
        //    if (Distance < 4)
        //    {
        //        if (Random.Range(0, 2) < 1)
        //        {
        //            //横なぎ（拡散）
        //            stateMachine.Dispatch((int)Event.Attack);
        //        }
        //        else
        //        {
        //            //風圧
        //            stateMachine.Dispatch((int)Event.Wind);
        //        }
        //    }

        //    //中射程
        //    else if (Distance < 6)
        //    {
        //        int randomNum = Random.Range(0, 3);
        //        if (randomNum < 1)
        //        {
        //            //横なぎ
        //            stateMachine.Dispatch((int)Event.Attack);
        //        }
        //        else if (randomNum < 2)
        //        {
        //            stateMachine.Dispatch((int)Event.Attack);

        //            //溜め切り上げ
        //        }
        //        else
        //        {
        //            stateMachine.Dispatch((int)Event.Attack);

        //            //降り下ろし
        //        }
        //    }

        //    //大射程
        //    else
        //    {
        //        if (Random.Range(0, 2) < 1)
        //        {
        //            //横なぎ（一回転）
        //            stateMachine.Dispatch((int)Event.Attack);
        //        }
        //        else
        //        {
        //            stateMachine.Dispatch((int)Event.Lunge);

        //            //突進吹き飛ばし
        //        }
        //    }
        //}
    }
}
