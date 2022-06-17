/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/04/02
// �쐬�� ���򔿋M
// PBO �� PowerBossOne
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/04/02 �쐬�J�n
//            BossScript�֌W�̔ėp�I�ȍs��AI���R�s�y
//            �U���̑I�����𗐐��őI�Ԃ悤�ɂ���
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
        // ���Ԑ؂�
        Timeout,
        // �ǐ�
        Tracking,
        //�ǐՔ͈͊O
        OutOfRange,
        // �U��
        Attack,
        //�ːi
        Lunge,
        //����
        Wind,
        //�U���I��
        AtkEnd,
        // �U���͈͊O
        OutOfAttackRange,
        // �c��1�̂ɂȂ���
        Death,
    }

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float applySpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveRange;
    [SerializeField] private Animator animator;
    [SerializeField] private CapsuleCollider capsuleCollider;//���̂��������
    [SerializeField] private SphereCollider sphereCollider; //�����p
    [SerializeField] private BoxCollider boxCollider;       //�ːi�U���p
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject slash;              //��Ԏa���G�Ȃ�

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

        //�U���㌄�ˑҋ@ �����F�^�C���A�E�g
        stateMachine.AddTransition<PBOStateAtkDelay, PBOStateWaiting>((int)Event.Timeout);
        //�ҋ@�ˈړ� �����F�^�C���A�E�g
        stateMachine.AddTransition<PBOStateWaiting, PBOStateMoving>((int)Event.Timeout);
        //�ړ��ˑҋ@ �����F�^�C���A�E�g
        stateMachine.AddTransition<PBOStateMoving, PBOStateWaiting>((int)Event.Timeout);
        //�ǐՁˑҋ@ �����F����͈͊O
        stateMachine.AddTransition<PBOStateTracking, PBOStateWaiting>((int)Event.OutOfRange);
        //stateMachine.AddTransition<PBOStateTracking, PBOStateMoving>((int)Event.OutOfRange);
        //�U���ˑҋ@ �����F�U���͈͊O
        stateMachine.AddTransition<PBOStateAttacking, PBOStateWaiting>((int)Event.OutOfAttackRange);
        //�U���ˑҋ@ �����F�U�����[�V�����I���
        stateMachine.AddTransition<PBOStateAttacking, PBOStateAtkDelay>((int)Event.AtkEnd);
        //�ǐՁˍU�� �����F�U���͈͓�
        stateMachine.AddTransition<PBOStateTracking, PBOStateAttacking>((int)Event.Attack);
        ////�ҋ@�ˍU���@�����F�U���͈͓�
        //stateMachine.AddTransition<PBOStateWaiting, PBOStateAttacking>((int)Event.Attack);
        //�ҋ@�˒ǐ� �����F����͈͓�
        stateMachine.AddTransition<PBOStateWaiting, PBOStateTracking>((int)Event.Tracking);
        //�ړ��˒ǐ� �����F����͈͓�
        stateMachine.AddTransition<PBOStateMoving, PBOStateTracking>((int)Event.Tracking);
        //�ǐՁ˓ːi�U�� �����F�H�H�H�H
        stateMachine.AddTransition<PBOStateTracking, PBOStateLungeAtk>((int)Event.Lunge);
        //�ːi�U���ˑҋ@ �����F�^�C���A�E�g
        stateMachine.AddTransition<PBOStateLungeAtk, PBOStateAtkDelay>((int)Event.Timeout);
        //�ǐՁ˕��� �����F�U���͈͓�(����)
        stateMachine.AddTransition<PBOStateTracking, PBOStateWindAtk>((int)Event.Wind);
        //�����ˑҋ@ �����F�^�C���A�E�g
        stateMachine.AddTransition<PBOStateWindAtk, PBOStateAtkDelay>((int)Event.Timeout);

        //�ǂ�����ł��ˎ��S �����FHP0
        stateMachine.AddAnyTransition<PBOStateDead>((int)Event.Death);

        //�ҋ@�X�e�[�g����n�܂�
        stateMachine.Start<PBOStateWaiting>();

        //�r�w�C�r�A�c���[���쐬�A������
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

    // ���S
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

    //�A�j���[�V�����C�x���g
    //�A�j���[�V�����̊J�n�Ɠ�����Collider���o��
    void AttackStart()
    {
        capsuleCollider.enabled = true;
    }

    void Slash()
    {
        //slash.SetActive(true);
    }

    //�A�j���[�V�����C�x���g
    //�A�j���[�V�����̏I���Ɠ�����Collider������
    //�A�j���[�V�������U������ҋ@�֖߂�
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

        //    //���˒�
        //    if (Distance < 4)
        //    {
        //        if (Random.Range(0, 2) < 1)
        //        {
        //            //���Ȃ��i�g�U�j
        //            stateMachine.Dispatch((int)Event.Attack);
        //        }
        //        else
        //        {
        //            //����
        //            stateMachine.Dispatch((int)Event.Wind);
        //        }
        //    }

        //    //���˒�
        //    else if (Distance < 6)
        //    {
        //        int randomNum = Random.Range(0, 3);
        //        if (randomNum < 1)
        //        {
        //            //���Ȃ�
        //            stateMachine.Dispatch((int)Event.Attack);
        //        }
        //        else if (randomNum < 2)
        //        {
        //            stateMachine.Dispatch((int)Event.Attack);

        //            //���ߐ؂�グ
        //        }
        //        else
        //        {
        //            stateMachine.Dispatch((int)Event.Attack);

        //            //�~�艺�낵
        //        }
        //    }

        //    //��˒�
        //    else
        //    {
        //        if (Random.Range(0, 2) < 1)
        //        {
        //            //���Ȃ��i���]�j
        //            stateMachine.Dispatch((int)Event.Attack);
        //        }
        //        else
        //        {
        //            stateMachine.Dispatch((int)Event.Lunge);

        //            //�ːi������΂�
        //        }
        //    }
        //}
    }
}
