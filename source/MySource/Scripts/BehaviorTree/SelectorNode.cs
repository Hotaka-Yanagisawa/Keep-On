using UnityEngine;

/// <summary>
/// �q��1�ł�Success��Ԃ����炻����Success��Ԃ��B�S�Ă̎q��Failure�Ȃ�Failure��Ԃ��B
/// �u�����`�m�[�h
/// </summary>
public class SelectorNode : BranchNode
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
        int failureCounter = 0;
        foreach (BaseNode child in _childNodeList)
        {
            //������������邽�߃R�����g�A�E�g
            // ���łɎ��s���ʂ��o�Ă�����̂̓X�L�b�v
            //if (child.status == NodeStatus.SUCCESS || child.status == NodeStatus.FAILURE)
            //{
            //    if (child.status == NodeStatus.FAILURE)
            //    {
            //        failureCounter++;
            //        _childIndex++;
            //    }
            //    continue;
            //}
            child.OnRunning();
            if (child.status != NodeStatus.SUCCESS)
            {
                _childIndex++;
                if (child.status == NodeStatus.FAILURE)
                {
                    failureCounter++;
                    child.OnFinish();
                }
                continue;
            }
            // 1�ł�Success�܂���Running�Ȃ�A����Wating�ɂ����ďI��
            child.OnFinish();
            result = child.status;
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
        // �S�Ă̎q��failure�Ȃ�failure��Ԃ�
        if (failureCounter >= _childNodeList.Count)
        {
            result = NodeStatus.FAILURE;
        }
        else
        {
            result = NodeStatus.RUNNING;
        }
        return result;
    }
}