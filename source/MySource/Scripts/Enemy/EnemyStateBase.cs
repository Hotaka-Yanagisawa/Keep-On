/////////////////////////////////////////////////////////////////////////
// 作成日 2021/03/18
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/03/18 作成開始
//            ステートの抽象クラス
// 
//
//
//////////////////////////////////////////////////////////////////////////


namespace Homare
{
    /// <summary>
    /// Stateの抽象クラス
    /// </summary>
    public abstract class EnemyStateBase
    {
        /// <summary>
        /// ステートを開始した時に呼ばれる
        /// </summary>
        public virtual void OnEnter(Enemy owner, EnemyStateBase prevState) { }
        /// <summary>
        /// 毎フレーム呼ばれる
        /// </summary>
        public virtual void OnUpdate(Enemy owner) { }
        /// <summary>
        /// 一定フレームごとに
        /// </summary>
        public virtual void OnFixedUpdate(Enemy owner) { }
        /// <summary>
        /// ステートを終了した時に呼ばれる
        /// </summary>
        public virtual void OnExit(Enemy owner, EnemyStateBase nextState) { }
    }
}