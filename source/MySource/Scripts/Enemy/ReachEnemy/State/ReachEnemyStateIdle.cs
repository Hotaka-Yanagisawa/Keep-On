#region �w�b�_�R�����g
// ReachEnemyStateIdle.cs
// �͈͌^�G���G�̑ҋ@��ԃN���XS
//
// 2021/04/25 : �O��D�l
#endregion


using UnityEngine;
using Homare;


namespace Mikami
{
    public partial class ReachEnemy
    {
        private class StateIdle : EnemyStateBase
        {
            private const int IDLE_TIME_COUNT = 300;

            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                owner.actionCnt = IDLE_TIME_COUNT;
                owner.rb.velocity = Vector3.zero;
                owner.rb.constraints = RigidbodyConstraints.FreezeRotationX |
                       RigidbodyConstraints.FreezeRotationY |
                       RigidbodyConstraints.FreezeRotationZ |
                       RigidbodyConstraints.FreezePositionY;
            }

            public override void OnUpdate(Enemy owner)
            {
                owner.rb.velocity = Vector3.zero;

                if (owner.actionCnt-- <= 0)
                    owner.ChangeState(statePatrol);
            }
        }
    }
}