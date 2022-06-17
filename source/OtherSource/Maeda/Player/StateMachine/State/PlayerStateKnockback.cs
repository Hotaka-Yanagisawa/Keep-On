using UnityEngine;
using Ohira;

#region HeaderComent
//==================================================================================
// PlayerStateKnockback
// �v���C���[�̃m�b�N�o�b�N���
// �쐬����	:2021/04/19
// �쐬��	:�O�c����
//---------- �X�V���� ----------
// 2021/04/19   �m�b�N�o�b�N�̎���
//==================================================================================
#endregion

/// <summary>
/// �m�b�N�o�b�N���̏���
/// </summary>
namespace Maeda
{
    public partial class Player
    {
        public class StateKnockBack : PlayerStateBase
        {
            #region �v���C�x�[�g�ϐ�
            //float knockbackTime =0.2395f;
            #endregion

            #region �I�[�o�[���C�h�֐�
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                owner.PlaySE("SE_Damage");
                if (!owner.animator.GetBool("knockback"))
                {
                    owner.rigidBody.velocity = new Vector3(0, owner.rigidBody.velocity.y, 0);
                    // owner.animator.SetInteger("knockbackType", 1);
                    owner.animator.SetBool("knockback", true);
                    // owner.SetInvinc(2.5f);

                    CameraController.Instance.ShakeCamera(5f, 3f, 1f);
                }
              
            }

            public override void OnUpdate(Player owner)
            {

                if (!owner.animator.GetBool("knockback") && !owner.animator.GetBool("standup"))
                {
                    owner.ChangeState(stateStanding);
                }

                owner.rigidBody.velocity = new Vector3(0, owner.rigidBody.velocity.y, 0);
            }

            public override void OnFixUpdate(Player owner)
            {
              
            }

            public override void OnExit(Player owner, PlayerStateBase nextState)
            {

                //knockbackTime = 0.2395f;
            }

            #endregion
        }
        #region �v���C�x�[�g�֐�
        private void KnockBackEnd()
        {
            if (animator.GetInteger("knockbackType") == 1)
                animator.SetBool("standup", true);

            animator.SetBool("knockback", false);
        }

        private void StandUpEnd()
        {
            animator.SetBool("standup", false);
            ChangeState(stateStanding);
        }
    
        #endregion
    }
}
