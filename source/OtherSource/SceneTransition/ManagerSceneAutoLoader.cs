#region ヘッダコメント
//==================================================================================
// ManagerSceneAutoLoader.cs
// マネージャシーン自動読み込みクラス。参考URL→http://kan-kikuchi.hatenablog.com/entry/ManagerSceneAutoLoader
// 作成日時	: 2021/03/??
// 作成者	: 三上優斗
//==================================================================================
#endregion
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Mikami
{
    /// <summary>
    /// Awake前にManagerSceneを自動でロードするクラス
    /// </summary>
    public class ManagerSceneAutoLoader
    {
        //ゲーム開始時(シーン読み込み前)に実行される
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadManagerScene()
        {
            string managerSceneName = "Manager";

            Application.targetFrameRate = 60; //FPSを60に設定

            //ManagerSceneが有効でない時(まだ読み込んでいない時)だけ追加ロードするように
            if (!SceneManager.GetSceneByName(managerSceneName).IsValid())
            {
                SceneManager.LoadScene(managerSceneName, LoadSceneMode.Additive);
            }
        }
    }
}