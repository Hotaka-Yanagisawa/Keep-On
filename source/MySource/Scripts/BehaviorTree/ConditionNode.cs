using System;
using UnityEngine;

/// <summary>
/// �����t���m�[�h
/// ���[�t�m�[�h(�q�������Ƃ��ł��Ȃ��m�[�h)
/// </summary>
public class ConditionNode : BaseNode
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
        //null�Ȃ�G���[
        if (_conditionFunc == null)
        {
            Debug.LogError("_conditionFunc is null : " + name);
            return NodeStatus.FAILURE;
        }
        //�p������
        base.OnRunning();
        //�X�e�[�^�X�̍X�V(�����炭�����ŋ�̓I�ȏ������s��)
        status = _conditionFunc();
        return status;
    }
}