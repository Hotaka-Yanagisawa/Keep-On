//////////////////////////////
// SpeedEnemyStateWaiting.cs
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
    public partial class SpeedEnemy
    {
        /// <summary>
        /// �ҋ@���
        /// </summary>
        private class StateWaiting : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("Speed:�ҋ@");

                owner.actionCnt = 300;
                owner.rb.velocity = Vector3.zero;
                owner.rb.constraints = RigidbodyConstraints.FreezeRotationX |
                       RigidbodyConstraints.FreezeRotationY |
                       RigidbodyConstraints.FreezeRotationZ |
                       RigidbodyConstraints.FreezePositionY;

            }

            public override void OnUpdate(Enemy owner)
            {
                owner.rb.velocity = Vector3.zero;

                owner.actionCnt--;
                if (owner.actionCnt <= 0)
                {
                    owner.animator.SetBool("isWait", false);
                    owner.ChangeState(stateMoving);
                }
            }

        }
    }
}