//////////////////////////////
// SpeedEnemyStateAttacking.cs
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
        class StateAttacking : EnemyStateBase
        {
            private const int ATTACK_TIME_CNT = 120;

            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("Speed:�U��");

                owner.actionCnt = ATTACK_TIME_CNT;
                // �v���C���[�̈ʒu��velocity��������
                float Angle = Mathf.Atan2(SpeedEnemy.player.transform.position.z - owner.transform.position.z,
                 SpeedEnemy.player.transform.position.x - owner.transform.position.x);

                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));
                // �U�����̋@���U�R�̃X�s�[�h�X�V
                owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed * 2;


                SpeedEnemy speedEnemy = owner as SpeedEnemy;
                // Hit����
                speedEnemy.colliders[(int)ColliderKind.AttackCollider].enabled = true;
                // Body�̓����蔻��
                Physics.IgnoreCollision(speedEnemy.colliders[(int)ColliderKind.BodyCollider],
                                        SpeedEnemy.playerCollider, true);

                Vector3 playerPos = player.transform.position;

                playerPos.y = owner.transform.position.y;

                // �G�̊p�x�̍X�V
                // Slerp:���݂̌����A�������������A��������X�s�[�h
                // LookRotation(������������):
                owner.transform.rotation =
                    Quaternion.Slerp(owner.transform.rotation,
                    Quaternion.LookRotation(playerPos - owner.transform.position),
                    1);

            }

            public override void OnFixedUpdate(Enemy owner)
            {
                if (--owner.actionCnt <= 0)
                {
                    owner.animator.SetBool("isWait", true);
                    owner.ChangeState(stateWaiting);
                }

                owner.GetComponent<EffectOperate>().CreateEffect(5, owner.transform.position, 2f, true, owner.transform.rotation);
                owner.GetComponent<EffectOperate>().CreateEffect(6, owner.transform.position, 2f, true, owner.transform.rotation);
            }

            public override void OnExit(Enemy owner, EnemyStateBase nextState)
            {
                owner.rb.velocity = Vector3.zero;

                SpeedEnemy speedEnemy = owner as SpeedEnemy;
                // Hit����
                speedEnemy.colliders[(int)ColliderKind.AttackCollider].enabled = false;
                // Body�̓����蔻��
                Physics.IgnoreCollision(speedEnemy.colliders[(int)ColliderKind.BodyCollider],
                                        SpeedEnemy.playerCollider, false);

                owner.animator.SetBool("isPunch", false);
                owner.animator.ResetTrigger("Found");

            }

        }
    }
}
