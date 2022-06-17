using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateStun
//	�v���C���[�̉E�։��������
// �쐬����	:2021/03/23
// �쐬��	:�O�c����
//---------- �X�V���� ----------
// 2021/03/23   �v���C���[�̍d���̎���     �ȉ��̏�Ԃ̑J�ڐ�
//                                          �W�����v��A�X�e�b�v��
//==================================================================================
#endregion

    /// <summary>
    /// �v���C���[�̍d���̏���
    /// �d���̓X�e�b�v�̃X�^�~�i�؂�݂̂Ƃ���
    /// </summary>

namespace Maeda
{
    public partial class Player
    {
        public class StateStun : PlayerStateBase
        {
            #region �I�[�o�[���C�h�֐�
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                
                owner.stunCnt = 0;
                owner.rigidBody.velocity = Vector3.zero;
                owner.rigidBody.angularVelocity = Vector3.zero;
                owner.animator.SetBool("landing", false);
            }


            public override void OnUpdate(Player owner)
            {
                owner.stunCnt++;

                if (owner.stunCnt >= owner.baseParam.stepStun)
                {
                    if (owner.animator.GetBool("jump"))
                    {
                        //Debug.Log("�O�cyaa");
                        owner.ChangeState(stateJumping);
                    }
                    else
                        owner.ChangeState(stateStanding);
                }
               
            }

            public override void OnExit(Player owner, PlayerStateBase nextState)
            {
              
                owner.animator.SetBool("step", false);
            }
            #endregion
        }
    }
}

