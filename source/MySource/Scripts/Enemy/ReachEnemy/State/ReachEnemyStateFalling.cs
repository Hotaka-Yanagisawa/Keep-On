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
using Homare;


namespace Mikami
{
    public partial class ReachEnemy
    {
        /// <summary>
        /// �ҋ@���
        /// </summary>
        private class StateFalling : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                owner.rb.velocity = Vector3.zero;
                owner.GetComponent<EffectOperate>().CreateEffect(1, owner.transform.position, 1.7f);

                owner.rb.constraints = RigidbodyConstraints.FreezeRotationX |
                       RigidbodyConstraints.FreezeRotationY |
                       RigidbodyConstraints.FreezeRotationZ;
                //owner.animator.SetFloat("Speed", 0);

            }
        }
    }
}