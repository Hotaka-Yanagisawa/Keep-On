/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/18
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/03/18 �쐬�J�n
//            �ҋ@��Ԃ̏������s��
// 
// 2021/04/22 �č\�zby�O��
//
//////////////////////////////////////////////////////////////////////////


using UnityEngine;


namespace Homare
{
    public partial class PowerEnemy
    {
        /// <summary>
        /// �ҋ@���
        /// </summary>
        private class StateWaiting : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("�ҋ@");

                owner.actionCnt = 2;
                owner.rb.velocity = Vector3.zero;
                owner.rb.constraints = RigidbodyConstraints.FreezeRotationX |
                                       RigidbodyConstraints.FreezeRotationY |
                                       RigidbodyConstraints.FreezeRotationZ |
                                       RigidbodyConstraints.FreezePositionY;
                owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);
            }
            public override void OnUpdate(Enemy owner)
            {
                owner.rb.velocity = Vector3.zero;

                owner.actionCnt -= Time.deltaTime;
                if (owner.actionCnt <= 0)
                {
                    owner.ChangeState(stateMoving);
                }
            }
        }
    }
}