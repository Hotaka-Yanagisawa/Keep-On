using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BaseNodeを継承しておりさらに
/// ブランチとなれるノードに継承する
/// </summary>
public class BranchNode : BaseNode
{
    //子ノードのリスト
    protected List<BaseNode> _childNodeList = new List<BaseNode>();
    //子ノードの番号
    protected int _childIndex = 0;

    /// <summary>
    /// ノード起動時処理
    /// </summary>
    public override void OnStart()
    {
        //継承元の処理
        base.OnStart();
        //インデックスの初期化
        _childIndex = 0;
        //リストが空ならエラー
        if (_childNodeList.Count == 0)
        {
            Debug.LogError("not child");
            return;
        }
    }

    /// <summary>
    /// 子のステータスを評価する
    /// </summary>
    /// <returns>status</returns>
    protected virtual NodeStatus EvaluateChild()
    {
        //分からんので後回し
        return NodeStatus.WAITING;
    }

    /// <summary>
    /// 子を追加する
    /// </summary>
    /// <param name="child">追加する子ノード</param>
    public virtual void AddChild(BaseNode child)
    {
        _childNodeList.Add(child);
    }

    /// <summary>
    /// 子ノードリストを取得
    /// </summary>
    /// <returns>_childNodeList</returns>
    public virtual List<BaseNode> GetChildList()
    {
        return _childNodeList;
    }
}