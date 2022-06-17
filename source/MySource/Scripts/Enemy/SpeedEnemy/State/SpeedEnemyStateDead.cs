//////////////////////////////
// SpeedEnemyStateDead.cs
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
        /// ���S���
        /// </summary>
        public class StateDead : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("Speed:���S");
                owner.animator.ResetTrigger("Found");
                owner.animator.SetBool("isPunch", false);
                owner.animator.SetBool("isWait", true);

                //owner.GetComponent<Animator>().SetBool("Dead", true);

                // �����~
                owner.actionCnt = 90;
                //owner.animator.SetBool("Dead", true);
                owner.GetComponent<EffectOperate>().CreateEffect(0, owner.transform.position, 1.7f);
                owner.rb.velocity = Vector3.zero;
            }

            public override void OnFixedUpdate(Enemy owner)
            {
                owner.rb.velocity = Vector3.zero;

                owner.actionCnt--;
                if (owner.actionCnt <= 0)
                {
                    Debug.Log("Speed:����");
                    //owner.creater.EnemyNum--;
                    //owner.transform.parent.gameObject.SetActive(false);
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
