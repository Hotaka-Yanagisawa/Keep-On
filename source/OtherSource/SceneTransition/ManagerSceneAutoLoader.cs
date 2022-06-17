#region �w�b�_�R�����g
//==================================================================================
// ManagerSceneAutoLoader.cs
// �}�l�[�W���V�[�������ǂݍ��݃N���X�B�Q�lURL��http://kan-kikuchi.hatenablog.com/entry/ManagerSceneAutoLoader
// �쐬����	: 2021/03/??
// �쐬��	: �O��D�l
//==================================================================================
#endregion
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Mikami
{
    /// <summary>
    /// Awake�O��ManagerScene�������Ń��[�h����N���X
    /// </summary>
    public class ManagerSceneAutoLoader
    {
        //�Q�[���J�n��(�V�[���ǂݍ��ݑO)�Ɏ��s�����
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadManagerScene()
        {
            string managerSceneName = "Manager";

            Application.targetFrameRate = 60; //FPS��60�ɐݒ�

            //ManagerScene���L���łȂ���(�܂��ǂݍ���ł��Ȃ���)�����ǉ����[�h����悤��
            if (!SceneManager.GetSceneByName(managerSceneName).IsValid())
            {
                SceneManager.LoadScene(managerSceneName, LoadSceneMode.Additive);
            }
        }
    }
}