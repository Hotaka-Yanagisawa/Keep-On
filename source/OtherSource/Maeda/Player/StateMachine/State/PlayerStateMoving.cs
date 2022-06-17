using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateMoving
//	�v���C���[�̈ړ����
// �쐬����	:2021/03/20
// �쐬��	:�O�c����
//---------- �X�V���� ----------
// 2021/03/20   �ړ��̏���������
//==================================================================================
#endregion

/// <summary>
/// �ړ���Ԃ̏���
/// ���̏�Ԃ���J�ڂł����Ԃ͈ȉ��̒ʂ�
/// �E�ړ����
/// �E�W�����v���
/// �E�S�����̉�����
/// �E�������
/// </summary>
namespace Maeda
{
    public partial class Player
    {
        public class StateMoving : PlayerStateBase
        {

            #region �I�[�o�[���C�h�֐�


            /// <summary>
            /// ��ԑJ�ڂ����ŏ��ɌĂ΂��֐�     Init()�Ɠ���
            /// </summary>
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                //Debug.Log("�ړ���");
                //owner.isJumping = false;
            }


            /// <summary>
            /// ���̏�Ԃ̎��J��Ԃ��Ă΂��֐�    Update()�Ɠ���
            /// </summary>
            public override void OnUpdate(Player owner)
            {
               
                // �C���v�b�g�V�X�e������̓��͂̒l���󂯎��
                owner.Velocity = owner.input.Player.Move.ReadValue<Vector2>();

                // �O��̃|�W�V�����̍X�V
                owner.oldPos = owner.transform.position;

                // �ړ�����
               
                Moving(owner);

                #region ���͂ɂ�镪��

                // �W�����v���͂͂�������
                if (owner.input.Player.Jump.triggered && !owner.animator.GetBool("jump"))
                    owner.ChangeState(stateJumping);                    // �W�����v��Ԃ�
                 
                // �X�e�b�v���͂͂�������
                else if (owner.input.Player.Dodge.triggered && !owner.animator.GetBool("step"))
                    owner.ChangeState(stateDodging);                    // �X�e�b�v��Ԃ�

                // �U�����͂͂�������
                else if (owner.input.Player.Attack.triggered)
                    owner.ChangeState(stateAttack);                     // �U����Ԃ�

                // �O��̃|�W�V�����ƈړ���̃|�W�V�����͈ꏏ��
                else if (owner.input.Player.Move.ReadValue<Vector2>() == Vector2.zero || owner.rigidBody.velocity == Vector3.zero)
                    owner.ChangeState(stateStanding);                   // ������Ԃ�


                #endregion
            }


            public override void OnFixUpdate(Player owner)
            {
               // Moving(owner);
            }


            public override void OnExit(Player owner, PlayerStateBase nextState)
            {
                
            }

            #endregion

            #region �v���C�x�[�g�֐�
            /// <summary>
            /// �ړ��֐��@Moving(Player owner)
            /// ���̓��͒l�𒴂����瑖��̑��x�ɂȂ�
            /// �܂������Ȃ�������A�����̑��x�ɂȂ�
            /// �R���g���[���̃X�e�B�b�N�̓��͂�X����Y���̐����ɕ����Ă���
            ///         �㉺��Y��   ��F+�l    ���F�|�l
            ///         ���E��X��   �E�F+�l    ���F�|�l       
            /// </summary>
            /// <param name="owner"></param>
            private void Moving(Player owner)
            {
                // ���͒l�̐�Βl�����l(stickBoundaryValue)�𒴂��Ă遨����
                if (Mathf.Abs(owner.Velocity.x) >= owner.baseParam.stickBoundaryValue || Mathf.Abs(owner.Velocity.y) >= owner.baseParam.stickBoundaryValue)
                {
                    //Debug.Log("�����Ă�");
                    owner.rigidBody.velocity = owner.moveForward * owner.baseParam.moveDash + new Vector3(0, owner.rigidBody.velocity.y, 0) * Time.deltaTime;
                }
                // ���͒l�̐�Βl�����l(stickBoundaryValue)�𒴂��Ă��Ȃ�������
                else
                {
                    //Debug.Log("�����Ă�");
                    owner.rigidBody.velocity = owner.moveForward * owner.baseParam.moveWalk + new Vector3(0, owner.rigidBody.velocity.y, 0) * Time.deltaTime;
                }
            }

            #endregion
        }
    }
}


