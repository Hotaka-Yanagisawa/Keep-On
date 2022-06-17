//////////////////////////////
// SpeedEnemyStateTracking.cs
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
        public class StateTracking : EnemyStateBase
        {
            // �U���K������
            private const float attackDistance = 6f;

            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("Speed:�ǐ�");
            }

            public override void OnFixedUpdate(Enemy owner)
            {
                float Angle = Mathf.Atan2(player.transform.position.z - owner.transform.position.z,
                                player.transform.position.x - owner.transform.position.x);

                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));

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
                }

                // �v���C���[�Ƃ̋���
                float playerDistance = Vector3.Distance(player.transform.position, owner.transform.position);
                
                // �v���C���[�ւ̍U���K�������ɂȂ����Ƃ�
                if(attackDistance - owner.enemyStatus.moveSpeed * 0.5 < playerDistance &&
                   playerDistance < attackDistance + owner.enemyStatus.moveSpeed * 0.5)
                {
                    owner.ChangeState(stateAIM);
                }

            }
        }
    }
}
