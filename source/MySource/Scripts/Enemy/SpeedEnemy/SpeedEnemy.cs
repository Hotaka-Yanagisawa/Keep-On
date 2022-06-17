//////////////////////////////
// SpeedEnemy.cs
//----------------------------
// �쐬��:2021/4/25 
// �쐬��:�v�c���M
//----------------------------
// �X�V�����E���e
//  �E�X�N���v�g�쐬
//
//
//////////////////////////////


using UnityEngine;
using Homare;


namespace Hisada
{
    public partial class SpeedEnemy : Enemy
    {
        private static readonly StateWaiting stateWaiting = new StateWaiting();
        private static readonly StateTracking stateTracking = new StateTracking();
        private static readonly StateMoving stateMoving = new StateMoving();
        private static readonly StateReturn stateReturn = new StateReturn();
        private static readonly StateKnockback stateKnockback = new StateKnockback();
        private static readonly StateDead stateDead = new StateDead();
        private static readonly StateAttacking stateAttacking = new StateAttacking();
        private static readonly StateAIM stateAIM = new StateAIM();
        private static readonly StateEscape stateEscape = new StateEscape();
        private static readonly StateFalling stateFalling = new StateFalling();

        public override bool IsDead => currentState is StateDead;

        [SerializeField,EnumIndex(typeof(ColliderKind))] private Collider[] colliders = new Collider[2];
        [SerializeField] static public Collider playerCollider;
        [SerializeField] AudioClip audioCharge;
        [SerializeField] AudioClip audioPunch;
        [SerializeField] public AudioSource audio = null;

        protected override void Awake()
        {
            base.Awake();
            
            currentState = stateFalling;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            ChangeState(stateFalling);
            
            //ChangeState(stateEscape);
        }

        /// <summary>
        /// ���씻��C�x���g�p�̊֐�
        /// </summary>
        public void GoToTrackingState()
        {
            if (currentState != stateAIM)
                if (currentState != stateAttacking)
                    ChangeState(stateTracking);
        }
        /// <summary>
        /// ���씻��C�x���g�p�̊֐�
        /// </summary>
        public void GoToAimState()
        {
            if (currentState != stateAttacking)
                ChangeState(stateAIM);
        }

        /// <summary>
        /// ���씻��C�x���g�p�̊֐�
        /// </summary>
        public void GoToWaitingState()
        {
            if (currentState != stateAIM)
                ChangeState(stateWaiting);
        }


        /// <summary>
        /// ���씻��C�x���g�p�̊֐�
        /// </summary>
        public void GoToAttackingState()
        {
            ChangeState(stateAttacking);
        }


        void AttackStart()
        {

        }


        //�A�j���[�V�����C�x���g
        //�A�j���[�V�����̏I���Ɠ�����Collider������
        //�A�j���[�V�������U������ҋ@�֖߂�
        void AttackEnd()
        {
            //animator.SetBool("Attack", false);
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
            //weapon.SetActive(false);
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
            if (Hp > 0 && (currentState == stateWaiting || currentState == stateMoving))
                ChangeState(stateTracking);
        }
        void ChargeSE()
        {
            audio.PlayOneShot(audioCharge);
        }
        void PunchSE()
        {
            audio.PlayOneShot(audioPunch);
        }
    }

}

namespace Hisada
{
    public enum ColliderKind
    {
        BodyCollider,
        AttackCollider,
    }
}