using System;
using UnityEngine;

/// <summary>
/// ���ۂ̍s��������m�[�h
/// ���[�t�m�[�h
/// </summary>
public class ActionNode : BaseNode
{
    // ���s���鏈��
    private Func<NodeStatus> _runningFunc = null;

    /// <summary>
    /// ���s���鏈���̐ݒ�
    /// </summary>
    /// <param name="func">���s���鏈��</param>
    public void SetRunningFunc(Func<NodeStatus> func)
    {
        _runningFunc = func;
    }

    /// <summary>
    /// �m�[�h���s���̃X�e�[�^�X��Ԃ�
    /// </summary>
    /// <returns>status</returns>
    public override NodeStatus OnRunning()
    {
        //�G���[�`�F�b�N
        if (_runningFunc == null)
        {
            Debug.LogError("_runningFunc is null : " + name);
            return NodeStatus.FAILURE;
        }
        //�e�N���X��OnRunning���s��
        base.OnRunning();
        //�X�e�[�^�X�̍X�V
        status = _runningFunc();
        //�X�e�[�^�X�̏�Ԃ�Ԃ�
        return status;
    }
}