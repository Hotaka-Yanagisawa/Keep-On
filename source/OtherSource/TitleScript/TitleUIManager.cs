#region ヘッダコメント
//===================================
// TitleUIManager.cs
//---------------------------------
// 作成日:2021/03/26
//---------------------------------
// 作成者:Hisada Ritsuki
//---------------------------------
// 更新履歴
//      :2021/03/26 Panelの差し替え機能
//      :2021/04/01 機能拡張 by三上
//===================================
#endregion


using UnityEngine;
using Mikami;
using Mizuno;
using UnityEngine.InputSystem;


namespace Hisada
{
    /// <summary>
    /// タイトルUI管理クラス
    /// </summary>
    public class TitleUIManager : MonoBehaviour
    {
        #region 変数宣言部
        [SerializeField] private PanelChanger titlePanelChanger;
        [SerializeField] private PanelChanger optionPanelChanger;
        [SerializeField] private PanelChanger configPanelChanger;
        [SerializeField] private PlayerInput playerInput;
        #endregion


        #region パブリック関数実装部
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


        #region プライベート関数実装部
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
