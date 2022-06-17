using UnityEngine;
using Mikami;
using UnityEngine.UI;

#region HeaderComent
//==================================================================================
// Result
//	�v���C���[�N���X�̖{�́@partial�ŃA�N�V�����ʂȂǕ����̃X�N���v�g�ɕ����Ă���@
//  �������ƂɂȂ�X�N���v�g
// �쐬����	:2021/03/19
// �쐬��	:�O�c����
//---------- �X�V���� ----------
// 2021/03/19	�Q�l����̃T���v������Ƀv���C���[�̃X�e�[�g�}�V������J�n         �Q�l����(https://www.youtube.com/watch?v=PbtJt5tnnI8&t=1181s)
// 2021/03/19   InputSystem�̓���                                                  �Q�l����(https://www.youtube.com/watch?v=pRSZr6CFcpQ&t=1080s)
//      :               :                                                          �Q�l����(http://tsubakit1.hateblo.jp/entry/2019/10/13/143530)  �����׋����E�E�E
// 2021/03/20	�v���C���[�̊�{�I�ȓ���(�ړ��A�W�����v�A���)������ �� �J�ڂ���Ƃ��X���[�Y�ɑJ�ڂ��Ȃ�
// 2021/03/20   ��ԑJ�ڂ���Ƃ��ɃX���[�Y�ɑJ�ڂ���悤�ɏC��
// 2021/03/21   �R�����g�A�E�g�����Ă��Ȃ������̂ł��܂����B���t���͎኱�K����邵�āB
// 2021/04/01   �A�j���[�V�����ɔ����U���̃X�e�[�g��ǉ�
//==================================================================================
#endregion

namespace Maeda
{
    public partial class Result
    {
        #region �X�e�[�g�̃C���X�^���X
        private static readonly StateDisplay stateDisplay = new StateDisplay();         // ���U���g��UI�\��
        #endregion

        enum E_DISP_STATE
        {
            DARK,
            UI,
            VALUE,
            RANK,

            MAX_DISP_STATE,
        }


        #region �v���C�x�[�g�ϐ�       
        private bool okClick = false;               
        private E_DISP_STATE disp = E_DISP_STATE.DARK;          // �\������p
        private Vector3 rectSize = new Vector3(8, 8, 8);

        #endregion

        #region �p�u���b�N�ϐ�
        #endregion

        #region �v���C�x�[�g�֐�
        /// <summary>
        /// ���݂̃X�e�[�g
        /// </summary>
        private ResultStateBase currentState = stateDisplay;

        /// <summary>
        /// Start()����Ă΂��
        /// </summary>
        private void OnStart()
        {          
            panelCanvas.alpha = 0.0f;
            uiCanvas.alpha    = 0.0f;
            valueCanvas.alpha = 0.0f;
            rankCanvas.alpha  = 0.0f;
            baseRect.localScale = rectSize;
            once = false;
            currentState.OnEnter(this, null);
        }


        /// <summary>
        /// Update()����Ă΂��
        /// </summary>
        private void OnUpdate()
        {
            if (input.Menu.Cancel.triggered)
            {              
                OnClickClose();
            }


            currentState.OnUpdate(this);
            
        }

        /// <summary>
        /// FixedUpdate()����Ă΂��
        /// </summary>
        private void OnFixedUpdate()
        {
            currentState.OnFixUpdate(this);
        }

        /// <summary>
        /// �X�e�[�g�̕ύX
        /// </summary>
        private void ChangeState(ResultStateBase nextState)
        {
            if (currentState != nextState)
            {
                currentState.OnExit(this, nextState);
                nextState.OnEnter(this, currentState);
                currentState = nextState;
            }
        }


        #endregion

        #region �p�u���b�N�֐�
        public void OnClickClose()
        {
            if(okClick)
            {
                buttonRect.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                TransitionManager.Instance.StartTransaction(Scenes.Title, null, true);
                //Mizuno.SoundManager.Instance.StopBGM();
            }
           
        }
        #endregion
    }
}

