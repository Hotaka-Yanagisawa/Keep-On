/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/18
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/03/18 �쐬�J�n
//            �m�b�N�o�b�N���̏������s��
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
        /// �m�b�N�o�b�N���
        /// </summary>
        class StateKnockback : EnemyStateBase
        {
            int Cnt = 60;
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("�m�b�N�o�b�N");

            }

            public override void OnUpdate(Enemy owner)
            {
                Cnt--;
                if (Cnt <= 0)
                {
                    owner.ChangeState(stateWaiting);
                }
            }
        }
    }
}