/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/18
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/03/18 �쐬�J�n
//            �G�l�~�[���S���̏������s��
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
        /// ���S���
        /// </summary>
        public class StateDead : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("���S");
                owner.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                // �����~
                owner.actionCnt = 90;
                owner.GetComponent<EffectOperate>().CreateEffect(0, owner.transform.position, 1.7f);
                owner.rb.velocity = Vector3.zero;
                owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);

            }

            public override void OnFixedUpdate(Enemy owner)
            {
                owner.actionCnt--;
                if (owner.actionCnt <= 0)
                {
                    Debug.Log("�����܂�");
                    //owner.ChangeState(stateWaiting);
                    owner.gameObject.GetComponent<CapsuleCollider>().enabled = true;
                    owner.transform.position = new Vector3(0, 20, 0);

                    //����
                    owner.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }
}