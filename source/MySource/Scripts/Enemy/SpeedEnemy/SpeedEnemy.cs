//////////////////////////////
// SpeedEnemy.cs
//----------------------------
// 作成日:2021/4/25 
// 作成者:久田律貴
//----------------------------
// 更新日時・内容
//  ・スクリプト作成
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
        /// 視野判定イベント用の関数
        /// </summary>
        public void GoToTrackingState()
        {
            if (currentState != stateAIM)
                if (currentState != stateAttacking)
                    ChangeState(stateTracking);
        }
        /// <summary>
        /// 視野判定イベント用の関数
        /// </summary>
        public void GoToAimState()
        {
            if (currentState != stateAttacking)
                ChangeState(stateAIM);
        }

        /// <summary>
        /// 視野判定イベント用の関数
        /// </summary>
        public void GoToWaitingState()
        {
            if (currentState != stateAIM)
                ChangeState(stateWaiting);
        }


        /// <summary>
        /// 視野判定イベント用の関数
        /// </summary>
        public void GoToAttackingState()
        {
            ChangeState(stateAttacking);
        }


        void AttackStart()
        {

        }


        //アニメーションイベント
        //アニメーションの終了と同時にColliderを消す
        //アニメーションを攻撃から待機へ戻す
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

            //武器を消す処理追加する
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
            //死んでなくかつ敵対してない場合追跡開始
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