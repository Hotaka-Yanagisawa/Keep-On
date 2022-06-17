/////////////////////////////////////////////////////////////////////////
// 作成日 2021/05/19
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/05/19 作成開始
//            有限オートマトンでは不便なこともあるので
// 
//
//
//////////////////////////////////////////////////////////////////////////


namespace Homare
{
    /// <summary>
    /// Stateの抽象クラス
    /// </summary>
    public abstract class BossStateBase
    {
        /// <summary>
        /// ステートを開始した時に呼ばれる
        /// </summary>
        public virtual void OnEnter(Boss owner, BossStateBase prevState) { }
        /// <summary>
        /// 毎フレーム呼ばれる
        /// </summary>
        public virtual void OnUpdate(Boss owner) { }
        /// <summary>
        /// 一定フレームごとに
        /// </summary>
        public virtual void OnFixedUpdate(Boss owner) { }
        /// <summary>
        /// ステートを終了した時に呼ばれる
        /// </summary>
        public virtual void OnExit(Boss owner, BossStateBase nextState) { }
    }
}