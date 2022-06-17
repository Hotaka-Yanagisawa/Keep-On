/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/27
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/03/27 �쐬�J�n
//            �����̌��̍s���͈͂܂Ŗ߂��Ԃ̏������s��
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
        /// �A�ҏ��
        /// </summary>
        class StateReturn : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("�A��");

                //Debug.Log(owner.creater.EnemyNum);
            }

            public override void OnFixedUpdate(Enemy owner)
            {
                float Angle = Mathf.Atan2(owner.firstPos.z - owner.transform.position.z,
                                owner.firstPos.x - owner.transform.position.x);

                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));
                //owner.velocity.x = Mathf.Cos(Angle);
                //owner.velocity.z = Mathf.Sin(Angle);
                // ���x�x�N�g���̒�����1�b��moveSpeed�����i�ނ悤�ɒ������܂�
                owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed;

                // �����ꂩ�̕����Ɉړ����Ă���ꍇ
                if (owner.rb.velocity.magnitude > 0)
                {
                    // �G�̊p�x�̍X�V
                    // Slerp:���݂̌����A�������������A��������X�s�[�h
                    // LookRotation(������������):
                    owner.transform.rotation =
                        Quaternion.Slerp(owner.transform.rotation,
                        Quaternion.LookRotation(owner.rb.velocity),
                        owner.enemyStatus.applySpeed);
                    owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);

                    //owner.rb.velocity = owner.velocity;
                }

                //�ړ�����
                Vector3 moveDistance = owner.transform.position - owner.firstPos;
                if (owner.enemyStatus.moveRange * 0.5f > moveDistance.magnitude)
                {
                    owner.ChangeState(stateWaiting);
                }
            }
        }
    }
}