using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateDodging
//	�v���C���[�̃X�e�b�v���
// �쐬����	:2021/03/23
// �쐬��	:�O�c����
//---------- �X�V���� ----------
// 2021/03/23   �X�e�b�v�̃X�N���v�g���ЂƂɂ܂Ƃ߂܂���   	
//==================================================================================
#endregion

namespace Maeda
{
    public partial class Player
    {

        float consumption;
        public class StateDodging : PlayerStateBase
        {
            Vector3 vec;
            
            
            #region �I�[�o�[���C�h�֐�

            /// <summary>
            /// ���̃X�e�[�g�ɓ��������̏���  
            /// </summary>
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                owner.isStep = true;
                owner.frameCnt = 0;                             // ������
               
                
                Debug.Log(owner.baseParam.style.style);

                #region �f��
                if (owner.baseParam.style.style == Style.E_Style.NORMAL)
                {
                    owner.consumption = owner.baseParam.maxSp / owner.normal.maxStep; // �X�^�~�i����ʂ̌v�Z

                    owner.OnCommonEnterFnc();
                }
                #endregion

                #region �Η͌^
                if (owner.baseParam.style.style == Style.E_Style.POWER)
                {
                    owner.consumption = owner.baseParam.maxSp / owner.power.maxStep;

                    owner.OnCommonEnterFnc();
                }
                #endregion

                #region �@���^
                if (owner.baseParam.style.style == Style.E_Style.MOBILITY)
                {
                    owner.consumption = owner.baseParam.maxSp / owner.mobility.maxStep;
                    owner.OnCommonEnterFnc();
                }
                #endregion

                #region �͈͌^
                if (owner.baseParam.style.style == Style.E_Style.REACH)
                {
                    owner.consumption = owner.baseParam.maxSp / owner.reach.maxStep;

                    owner.OnCommonEnterFnc();
                }
                #endregion
            }


            /// <summary>
            /// ���̃X�e�[�g�̊ԏ�������
            /// </summary>
            public override void OnUpdate(Player owner)
            {
                owner.frameCnt++;                                   // �t���[���J�E���g�𑝂₷
                //�����X�s�[�h�̒���
                owner.rigidBody.velocity = new Vector3(owner.rigidBody.velocity.x, owner.rigidBody.velocity.y * 0.8f, owner.rigidBody.velocity.z);

                // �t���[���J�E���g���ݒ肵���J�E���g��葽���Ȃ����珈�����s��
                #region �f��
                if (owner.baseParam.style.style == Style.E_Style.NORMAL)
                {
                    if (owner.frameCnt >= owner.normal.stepRange * 0.7f) owner.rigidBody.useGravity = true;
                    // �t���[���J�E���g���ݒ肵���J�E���g��葽���Ȃ����珈�����s��
                    if (owner.frameCnt >= owner.normal.stepRange)
                    {
                        owner.OnCommonUpdateFnc();
                    }
                }
                #endregion

                #region �Η͌^
                else if (owner.baseParam.style.style == Style.E_Style.POWER)
                {
                    if (owner.frameCnt >= owner.power.stepRange * 0.7f) owner.rigidBody.useGravity = true;
                    // �t���[���J�E���g���ݒ肵���J�E���g��葽���Ȃ����珈�����s��
                    if (owner.frameCnt >= owner.power.stepRange)
                    {
                        owner.OnCommonUpdateFnc();
                    }
                }
                #endregion

                #region �@���^
                else if (owner.baseParam.style.style == Style.E_Style.MOBILITY)
                {
                    if (owner.frameCnt >= owner.mobility.stepRange * 0.7f) owner.rigidBody.useGravity = true;
                    // �t���[���J�E���g���ݒ肵���J�E���g��葽���Ȃ����珈�����s��
                    if (owner.frameCnt >= owner.mobility.stepRange)
                    {
                        owner.OnCommonUpdateFnc();
                    }
                }
                #endregion

                #region �͈͌^
                else if (owner.baseParam.style.style == Style.E_Style.REACH)
                {
                    //����K�v�Ȃ̂�
                    if (owner.frameCnt >= owner.reach.stepRange * 0.7f) owner.rigidBody.useGravity = true;

                    if(owner.frameCnt >= owner.reach.stepRange)
                    {
                        owner.OnCommonUpdateFnc();
                    }
                   

                }
                #endregion
            }

            /// <summary>
            /// ���̃X�e�[�g���甲����Ƃ��̏���
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="nextState"></param>
            public override void OnExit(Player owner, PlayerStateBase nextState)
            {
                owner.isStep = false;
                owner.rigidBody.useGravity = true;
                owner.animator.SetBool("step", false);           
            }
            #endregion
        }


        #region �v���C�x�[�g�֐�
        private void OnCommonEnterFnc()
        {
            // �X�^�~�i�̃`�F�b�N�ƃC���^�[�o���̃`�F�b�N
            if (baseParam.currentSp >= consumption && stepIntervalTime <= 0 && okStep)
            {
                baseParam.currentSp -= consumption;                 // �X�^�~�i�̏��� 

                if(baseParam.currentSp< consumption)
                okStep = false;

                stepIntervalTime = baseParam.stepInterval;          // �X�e�b�v�̃C���^�[�o���̐ݒ�

                animator.SetBool("step", true);

                OnStep();                                           // �X�e�b�v�̏���               
            }
            else
            {
                ChangeState(stateMoving);
            }
        }


        private void OnCommonUpdateFnc()
        {
            if (baseParam.currentSp < consumption)         // �����m�F
            {
                ChangeState(stateMoving);                   // �d����Ԃ�
            }
            else if (animator.GetBool("jump"))
            {
                animator.SetInteger("jumpType", 1);
                ChangeState(stateFalldown);
            }
            else if (!animator.GetBool("jump"))
            {
                ChangeState(stateMoving);
            }
            
        }


        private void OnStep()
        {
            //rigidBody.useGravity = false;
            //rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
            rigidBody.velocity = Vector3.zero;
            rigidBody.AddForce(moveForward.normalized * 15f, ForceMode.Impulse);
            PlaySE("SE_Step");
        }
        #endregion
    }
}

