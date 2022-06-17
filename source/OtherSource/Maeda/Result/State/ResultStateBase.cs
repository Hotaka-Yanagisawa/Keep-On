#region HeaderComent
//==================================================================================
// ResultStateBase
// リザルトのステートの抽象クラス
// 作成日時	:2021/03/20
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/03/19   OnFixUpdateを追加  特に意味はないので現在OnUpdate()と同じ処理を行っている
//==================================================================================
#endregion


/// <summary>
/// Stateの抽象クラス
/// ここから欲しくなった機能を追加できる
/// 例）On～～(Player owner){ } 
/// 基底クラスだと思う...
/// </summary>
namespace Maeda
{
    public abstract class ResultStateBase
    {
        #region パブリック関数


        /// <summary>
        /// ステートを開始した時に呼ばれる
        /// </summary>
        public virtual void OnEnter(Result owner, ResultStateBase prevState) { }


        /// <summary>
        /// 毎フレーム呼ばれる
        /// </summary>
        public virtual void OnUpdate(Result owner) { }


        /// <summary>
        /// 一定フレーム毎に呼ばれる
        /// </summary>
        public virtual void OnFixUpdate(Result owner) { }


        /// <summary>
        /// ステートを終了した時に呼ばれる
        /// </summary>
        public virtual void OnExit(Result owner, ResultStateBase nextState) { }


        #endregion
    }
}
