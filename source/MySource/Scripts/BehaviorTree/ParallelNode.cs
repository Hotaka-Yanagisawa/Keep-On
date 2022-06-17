using UnityEngine;

/// <summary>
/// �q�𓯎��Ɏ��s���A�S�Ă̎q��Success�Ȃ�Success��Ԃ��A����ȊO��Failure��Ԃ�
/// �u�����`�m�[�h
/// </summary>
public class ParallelNode : BranchNode
{

    /// <summary>
    /// �m�[�h���s���̃X�e�[�^�X��Ԃ�
    /// </summary>
    /// <returns>status</returns>
    public override NodeStatus OnRunning()
    {
        base.OnRunning();
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
        int successCounter = 0;
        int runningCounter = 0;
        foreach (BaseNode child in _childNodeList)
        {
            // ���łɎ��s���ʂ��o�Ă�����̂̓X�L�b�v
            if (child.status == NodeStatus.SUCCESS)
            {
                successCounter++;
                continue;
            }
            child.OnRunning();
            if (child.status != NodeStatus.FAILURE)
            {
                if (child.status == NodeStatus.RUNNING)
                {
                    runningCounter++;
                }
                else if (child.status == NodeStatus.SUCCESS)
                {
                    successCounter++;
                    child.OnFinish();
                }
                _childIndex++;
                continue;
            }
            // �q��1�ł�Failure�Ȃ瑼��Waiting�ɂ���Failure��Ԃ�
            result = NodeStatus.FAILURE;
            child.OnFinish();
            for (int i = 0; i < _childNodeList.Count; i++)
            {
                if (i == _childIndex)
                {
                    continue;
                }
                _childNodeList[i].status = NodeStatus.WAITING;
            }
            return result;

        }
        if (runningCounter > 0)
        {
            result = NodeStatus.RUNNING;
            // �S�Ă̎q��Success�Ȃ�Success��Ԃ�
        }
        else if (successCounter == _childNodeList.Count)
        {
            result = NodeStatus.SUCCESS;
        }
        return result;
    }
}