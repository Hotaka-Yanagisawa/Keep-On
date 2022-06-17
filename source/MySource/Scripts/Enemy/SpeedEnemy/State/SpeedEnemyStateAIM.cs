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
    public partial class SpeedEnemy
    {
        private class StateAIM : EnemyStateBase
        {
            float time = 1.208f * (24f / 60f);

            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                time = 1.5f;
                owner.rb.velocity = Vector3.zero;

                owner.animator.SetTrigger("Found");

            }

            public override void OnUpdate(Enemy owner)
            {
                float Angle = Mathf.Atan2(player.transform.position.z - owner.transform.position.z,
                 player.transform.position.x - owner.transform.position.x);

                var ToPlayer = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));

                // �G�̊p�x�̍X�V
                // Slerp:���݂̌����A�������������A��������X�s�[�h
                // LookRotation(������������):
                owner.transform.rotation =
                    Quaternion.Slerp(owner.transform.rotation,
                    Quaternion.LookRotation(ToPlayer),
                    owner.enemyStatus.applySpeed);

                owner.rb.velocity = Vector3.zero;

                // �����ɏƏ����v���C���[�ɍ��킹�����鏈�����L�q
                time -= Time.deltaTime;
                if(time < 0f)
                {
                    // �X�e�[�g��ς����
                    owner.ChangeState(stateAttacking);
                }

            }

            public override void OnExit(Enemy owner, EnemyStateBase nextState)
            {
                owner.animator.SetBool("isPunch", true);
            }

        }
    }
}
