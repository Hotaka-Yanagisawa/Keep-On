#region �w�b�_�R�����g
//===================================
// TitleUIManager.cs
//---------------------------------
// �쐬��:2021/03/26
//---------------------------------
// �쐬��:Hisada Ritsuki
//---------------------------------
// �X�V����
//      :2021/03/26 Panel�̍����ւ��@�\
//      :2021/04/01 �@�\�g�� by�O��
//===================================
#endregion


using UnityEngine;
using Mikami;
using Mizuno;
using UnityEngine.InputSystem;


namespace Hisada
{
    /// <summary>
    /// �^�C�g��UI�Ǘ��N���X
    /// </summary>
    public class TitleUIManager : MonoBehaviour
    {
        #region �ϐ��錾��
        [SerializeField] private PanelChanger titlePanelChanger;
        [SerializeField] private PanelChanger optionPanelChanger;
        [SerializeField] private PanelChanger configPanelChanger;
        [SerializeField] private PlayerInput playerInput;
        #endregion


        #region �p�u���b�N�֐�������
        public void OnClickPushAnyButton()
        {
            Mizuno.SoundManager.Instance.PlayMenuSe("SE_Submit");
            optionPanelChanger.Open();
            titlePanelChanger.Close();
            playerInput.actions["Cancel"].started += OnCancelOption;
        }

        public void OnClickGameStartButton()
        {
            Mizuno.SoundManager.Instance.PlayMenuSe("SE_Submit");
            SoundManager.Instance.StopBGMWithFade(2f);
            TransitionManager.Instance.StartTransaction(Scenes.Tutorial, null, true);
            playerInput.actions["Cancel"].started -= OnCancelOption;
            playerInput.actions["Move"].started -= OnCursolMove;
        }

        public void OnClickConfigButton()
        {
            Mizuno.SoundManager.Instance.PlayMenuSe("SE_Submit");
            optionPanelChanger.Close();
            configPanelChanger.Open();
            playerInput.actions["Cancel"].started += OnCancelConfig;
            playerInput.actions["Cancel"].started -= OnCancelOption;
        }

        public void OnClickGameEndButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
        }
        #endregion


        #region �v���C�x�[�g�֐�������
        void Start()
        {
            titlePanelChanger.Open();
            optionPanelChanger.Close();
            configPanelChanger.Close();
            SoundManager.Instance.PlayBGMWithFade("BGM_Title");
            playerInput.actions["Move"].started += OnCursolMove;
        }


        private void OnCancelOption(InputAction.CallbackContext callback)
        {
            Mizuno.SoundManager.Instance.PlayMenuSe("SE_Cancel");
            optionPanelChanger.Close();
            titlePanelChanger.Open();
            playerInput.actions["Cancel"].started -= OnCancelOption;
        }

        private void OnCancelConfig(InputAction.CallbackContext callback)
        {
            Mizuno.SoundManager.Instance.PlayMenuSe("SE_Cancel");
            configPanelChanger.Close();
            optionPanelChanger.Open();
            playerInput.actions["Cancel"].started -= OnCancelConfig;
            playerInput.actions["Cancel"].started += OnCancelOption;
        }

        private void OnCursolMove(InputAction.CallbackContext callback)
        {
            Mizuno.SoundManager.Instance.PlayMenuSe("SE_Select");
        }

        #endregion
    }
}
