/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/18
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/03/18 �쐬�J�n
//            �U����Ԃ̏������s��
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
        /// �U�����
        /// </summary>
        class StateAttacking : EnemyStateBase
        {
            // Vector3 playerDistance;
            //float actionCnt;
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("�U��");
                owner.rb.velocity = Vector3.zero;

                owner.animator.SetFloat("Speed", 0);
                owner.animator.SetBool("Attack", true);
                //owner.animator.SetBool("AttackEnd", false);
                //actionCnt = owner.attackTime;
            }

            public override void OnFixedUpdate(Enemy owner)
            {
                Vector3 playerPos = player.transform.position;

                playerPos.y = owner.transform.position.y;

                // �G�̊p�x�̍X�V
                // Slerp:���݂̌����A�������������A��������X�s�[�h
                // LookRotation(������������):
                owner.transform.rotation =
                    Quaternion.Slerp(owner.transform.rotation,
                    Quaternion.LookRotation(playerPos - owner.transform.position),
                    owner.enemyStatus.applySpeed);

                PowerEnemy powerEnemy = owner as PowerEnemy;
                powerEnemy.attackBehavior.OnUpdate();
            }

            //public override void OnExit(Enemy owner, EnemyStateBase nextState)
            //{
            //    owner.animator.SetBool("Attack", false);
            //    owner.animator.SetBool("AttackEnd", true);
            //}
        }
    }
}