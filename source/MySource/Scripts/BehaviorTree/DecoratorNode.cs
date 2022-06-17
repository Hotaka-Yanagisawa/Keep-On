using System;
using UnityEngine;

/// <summary>
/// �����𖞂����Ă���Ȃ�q��status���A�����łȂ��Ȃ�Failure��Ԃ�
/// �q��1���������Ƃ��o���Ȃ�
/// �u�����`�m�[�h
/// </summary>
public class DecoratorNode : BranchNode
{
    // ���肷�鏈��
    private Func<NodeStatus> _conditionFunc = null;

    /// <summary>
    /// ���肷�鏈���̐ݒ�
    /// </summary>
    /// <param name="func">���肷�鏈��</param>
    public void SetConditionFunc(Func<NodeStatus> func)
    {
        _conditionFunc = func;
    }

    /// <summary>
    /// �m�[�h���s���̃X�e�[�^�X��Ԃ�
    /// </summary>
    /// <returns>status</returns>
    public override NodeStatus OnRunning()
    {
        //���̊֐��̏������s��
        base.OnRunning();
        //���X�g��2�ȏ�Ȃ�G���[
        if (_childNodeList.Count > 1)
        {
            Debug.LogError("DecoratorNode can only have one");
            return NodeStatus.FAILURE;
        }
        //���\�b�h���Ȃ��Ȃ�G���[
        if (_conditionFunc == null)
        {
            Debug.LogError("_conditionFunc is null : " + name);
            return NodeStatus.FAILURE;
        }
        status = EvaluateChild();
        if (status == NodeStatus.RUNNING)
        {
            OnRunning();
        }
        return status;
    }

    /// <summary>
    /// �q�̃X�e�[�^�X��]������
    /// </summary>
    /// <returns>status</returns>
    protected override NodeStatus EvaluateChild()
    {
        NodeStatus result = NodeStatus.WAITING;
        status = _conditionFunc();
        // ���肪�ʂ�Ȃ������狭���I��Failure��Ԃ�
        if (status == NodeStatus.FAILURE)
        {
            result = NodeStatus.FAILURE;
            return result;
            // �q��status��Ԃ�
        }
        //�q��1���������Ƃ��o���Ȃ����߂O������
        else if (status == NodeStatus.SUCCESS) result = _childNodeList[0].OnRunning();
        //if (result == NodeStatus.SUCCESS || result == NodeStatus.FAILURE)  _childNodeList[0].OnFinish();
        //if (result == NodeStatus.SUCCESS || result == NodeStatus.FAILURE)
        //{
        //    _childNodeList[0].status = NodeStatus.RUNNING;
        //}
        //���U���g��RUNNING�Ȃ疳�����[�v����H
        return result;
    }
}