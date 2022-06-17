using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateAttack
//	�v���C���[�̍U�����
// �쐬����	:2021/04/01
// �쐬��	:�O�c����
//---------- �X�V���� ----------
// 2021/04/01   �A�j���[�V�����ɔ����U���̏�ԑJ�ڂ�ǉ�
//==================================================================================
#endregion


/// <summary>
/// ������Ԃ̏���
/// ���̏�Ԃ���J�ڂł����Ԃ͈ȉ��̒ʂ�
/// �E�������
/// �E�ړ����
/// �E�W�����v���
/// </summary>
namespace Maeda
{
    public partial class Player
    {
        public class StateAttack : PlayerStateBase
        {
            int attacktype;
            #region �I�[�o�[���C�h�֐�

            /// <summary>
            /// ��ԑJ�ڂ����ŏ��ɌĂ΂��֐�     Init()�Ɠ���
            /// </summary>
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                attacktype = 0;
                owner.animator.SetTrigger("attack");
               
            }


            /// <summary>
            /// ���̏�Ԃ̎��J��Ԃ��Ă΂��֐�    Update()�Ɠ���
            /// </summary>
            public override void OnUpdate(Player owner)
            {
                //owner.rigidBody.velocity = Vector3.zero;
                if(owner.input.Player.Attack.triggered)
                {
                    attacktype++;
                }
                else if(!owner.input.Player.Attack.triggered && owner.oldPos == owner.transform.position)
                {
                    owner.ChangeState(stateStanding);
                }



                #region ���͂̕���
                // �ړ����̗͂L��
                if (owner.oldPos != owner.transform.position || owner.input.Player.Move.triggered)
                {

                    owner.ChangeState(stateMoving);     // �ړ���Ԃ�
                }
                #endregion
            }

            public override void OnExit(Player owner, PlayerStateBase nextState)
            {
                owner.animator.ResetTrigger("attack");
            }

            #endregion
        }
    }
}
