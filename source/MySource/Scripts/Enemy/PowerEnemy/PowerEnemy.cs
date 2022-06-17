/////////////////////////////////////////////////////////////////////////
// 作成日 2021/03/16
// 作成者 柳沢帆貴
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
        /// 視野判定イベント用の関数
        /// </summary>
        public void GoToTrackingState()
        {   
            if(currentState != stateEscape)
            if(currentState != stateDead)
            ChangeState(stateTracking);
        }
        /// <summary>
        /// 視野判定イベント用の関数
        /// </summary>
        public void GoToWaitingState()
        {
            if (currentState != stateAttacking)
                if (currentState != stateDead)
               ChangeState(stateWaiting);
        }
        /// <summary>
        /// 視野判定イベント用の関数
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
        
        //アニメーションイベント
        //アニメーションの終了と同時にColliderを消す
        //アニメーションを攻撃から待機へ戻す
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
            //武器を消す処理追加する
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
            //死んでなくかつ敵対してない場合追跡開始
            if(Hp > 0 && (currentState == stateWaiting || currentState == stateMoving))
            ChangeState(stateTracking);
        }
    }
}