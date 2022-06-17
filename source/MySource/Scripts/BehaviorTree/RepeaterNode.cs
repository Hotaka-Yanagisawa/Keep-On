using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �K��̉񐔂��q�m�[�h���Ԃ��܂�Running��Ԃ��B�����𖞂�������Success��Ԃ��B
/// �u�����`�m�[�h
/// </summary>
public class RepeaterNode : BranchNode
{
    public int _repeatNum = 0;
    private int _repeatCounter = 0;
    public override NodeStatus OnRunning()
    {
        base.OnRunning();

        if (_repeatNum == 0)
        {
            Debug.LogError("_repeatNum is 0");
            return NodeStatus.FAILURE;
        }
        status = EvaluateChild();
        if (status == NodeStatus.RUNNING)
        {
            OnRunning();
        }
        return status;
    }


    protected override NodeStatus EvaluateChild()
    {
        NodeStatus result = NodeStatus.WAITING;
        int finishCounter = 0;
        foreach (BaseNode child in _childNodeList)
        {
            // ���łɎ��s���ʂ��o�Ă�����̂̓X�L�b�v
            if (child.status == NodeStatus.FAILURE || child.status == NodeStatus.SUCCESS)
            {
                finishCounter++;
                continue;
            }
            child.OnRunning();
            if (child.status == NodeStatus.FAILURE || child.status == NodeStatus.SUCCESS)
            {
                child.OnFinish();
                finishCounter++;
            }
        }
        // �܂��q��Running���Ȃ�Running��Ԃ�
        if (finishCounter < _childNodeList.Count)
        {
            result = NodeStatus.RUNNING;
            return result;
        }
        _repeatCounter++;
        // �K��񐔌J��Ԃ����Ȃ�Success��Ԃ�
        if (_repeatCounter >= _repeatNum)
        {
            result = NodeStatus.SUCCESS;
            return result;
        }
        // �K��񐔖����Ȃ�S�Ă̎q��Waiting�ɖ߂��A�Ăя�������
        result = NodeStatus.RUNNING;
        ChildWaiting(_childNodeList);
        return result;
    }

    /// <summary>
    /// �q�m�[�h��Waiting�ɂ���
    /// </summary>
    /// <param name="childNodeList">�q�m�[�h���X�g</param>
    private void ChildWaiting(List<BaseNode> childNodeList)
    {
        foreach (BaseNode child in childNodeList)
        {
            child.status = NodeStatus.WAITING;
            BranchNode branchNode = child as BranchNode;
            if (branchNode == null)
            {
                continue;
            }
            ChildWaiting(branchNode.GetChildList());
        }
    }
}