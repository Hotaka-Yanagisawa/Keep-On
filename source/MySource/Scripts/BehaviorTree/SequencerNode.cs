using UnityEngine;

/// <summary>
/// �q��1�ł�Failure��Ԃ����炻����Failure��Ԃ��B�S�Ă̎q��Success�Ȃ�Success��Ԃ��B
/// �u�����`�m�[�h
/// </summary>
public class SequencerNode : BranchNode
{
    /// <summary>
    /// �m�[�h���s���̃X�e�[�^�X��Ԃ�
    /// </summary>
    /// <returns>status</returns>
    public override NodeStatus OnRunning()
    {
        base.OnRunning();
        if (_childNodeList.Count <= _childIndex)
        {
            Debug.LogError("index is over");
            return NodeStatus.FAILURE;
        }
        NodeStatus childStatus = NodeStatus.WAITING;
        childStatus = _childNodeList[_childIndex].OnRunning();
        if (childStatus == NodeStatus.SUCCESS)
        {
            _childNodeList[_childIndex].OnFinish();
            _childIndex++;
        }
        status = EvaluateChild();
        return status;
    }

    /// <summary>
    /// �q�̃X�e�[�^�X��]������
    /// </summary>
    /// <returns>status</returns>
    protected override NodeStatus EvaluateChild()
    {
        NodeStatus result = NodeStatus.WAITING;
        foreach (BaseNode child in _childNodeList)
        {
            // 1�ł�Failure�Ȃ�I��
            if (child.status == NodeStatus.FAILURE)
            {
                result = NodeStatus.FAILURE;
                break;
            }
            else if (child.status == NodeStatus.RUNNING || child.status == NodeStatus.WAITING)
            {
                result = NodeStatus.RUNNING;
                break;
            }
            result = NodeStatus.SUCCESS;
        }
        return result;
    }
}