/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/16
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/03/16 �쐬�J�n
// 2021/03/18 �\�[�X�R�[�h�𕪊����č쐬�A �X�e�[�g�p�^�[���ŏ�ԊǗ�
// 2021/03/18 �ҋ@�A�ړ���ԍ쐬
// 
// 2021/04/22 �č\�zby�O��
//
//////////////////////////////////////////////////////////////////////////


using UnityEngine;


namespace Homare
{
    public partial class PowerEnemy : Enemy
    {
        private static readonly StateWaiting stateWaiting = new StateWaiting();
        private static readonly StateTracking stateTracking = new StateTracking();
        private static readonly StateMoving stateMoving = new StateMoving();
        private static readonly StateReturn stateReturn = new StateReturn();
        private static readonly StateKnockback stateKnockback = new StateKnockback();
        private static readonly StateDead stateDead = new StateDead();
        private static readonly StateAttacking stateAttacking = new StateAttacking();
        private static readonly StateEscape stateEscape = new StateEscape();
        private static readonly StateFalling stateFalling = new StateFalling();

        public override bool IsDead => currentState is StateDead;

        private PowerEnemyBehavior attackBehavior;

        [SerializeField] EnemyWeaponDamage weaponDamage;
        [SerializeField] GameObject weapon;

        protected override void Awake()
        {
            base.Awake();

            attackBehavior = new PowerEnemyBehavior(this);

            currentState = stateFalling;
            attackBehavior.OnStart();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ChangeState(stateFalling);
            weapon.SetActive(true);
            //ChangeState(stateEscape);
        }

        /// <summary>
        /// ���씻��C�x���g�p�̊֐�
        /// </summary>
        public void GoToTrackingState()
        {   
            if(currentState != stateEscape)
            if(currentState != stateDead)
            ChangeState(stateTracking);
        }
        /// <summary>
        /// ���씻��C�x���g�p�̊֐�
        /// </summary>
        public void GoToWaitingState()
        {
            if (currentState != stateAttacking)
                if (currentState != stateDead)
               ChangeState(stateWaiting);
        }
        /// <summary>
        /// ���씻��C�x���g�p�̊֐�
        /// </summary>
        public void GoToAttackingState()
        {
            if (currentState != stateDead)
                ChangeState(stateAttacking);
        }

        void AttackStart()
        {
            weaponDamage.Collider.enabled = true;
        }
        
        //�A�j���[�V�����C�x���g
        //�A�j���[�V�����̏I���Ɠ�����Collider������
        //�A�j���[�V�������U������ҋ@�֖߂�
        void AttackEnd()
        {
            weaponDamage.Collider.enabled = false;
            if (currentState != stateDead)
                if(currentState != stateEscape)
                ChangeState(stateWaiting);
            animator.SetBool("Attack", false);
        }

        protected override void Death()
        {
            base.Death();
            ChangeState(stateDead);
        }

        protected override void Escape()
        {
            base.Escape();
            ChangeState(stateEscape);
            //��������������ǉ�����
            weapon.SetActive(false);
        }

        protected override void Landing()
        {
            base.Landing();
            if (currentState != stateEscape)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(true);
                ChangeState(stateWaiting);
            }
        }

        public override void OnHitPlayerAttack(float damage)
        {
            base.OnHitPlayerAttack(damage);
            //����łȂ����G�΂��ĂȂ��ꍇ�ǐՊJ�n
            if(Hp > 0 && (currentState == stateWaiting || currentState == stateMoving))
            ChangeState(stateTracking);
        }
    }
}