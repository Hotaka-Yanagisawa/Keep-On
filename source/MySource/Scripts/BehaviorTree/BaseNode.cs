using UnityEngine;

/// <summary>
/// �e�m�[�h�̃x�[�X�ƂȂ����
/// </summary>
public class BaseNode
{
    // �X�e�[�^�X
    private NodeStatus _status = NodeStatus.WAITING;
    public NodeStatus status
    {
        get { return _status; }
        set { _status = value; }
    }
    // ���O
    public string name { get; set; }


    /// <summary>
    /// �m�[�h�N��������
    /// </summary>
    public virtual void OnStart()
    {
        //�܂��N���Ȃ�
        if (_status != NodeStatus.WAITING)
        {
            Debug.LogError("Status is not waiting : " + name);
            return;
        }
        //�ҋ@��������s���փX�e�[�^�X�ύX
        _status = NodeStatus.RUNNING;
        //Debug.Log("OnStart : " + name + ", status : " + _status);
    }

    /// <summary>
    /// �m�[�h���s���̃X�e�[�^�X��Ԃ�
    /// </summary>
    /// <returns>status</returns>
    public virtual NodeStatus OnRunning()
    {
        //�ҋ@���Ȃ���s���ɂ���
        if (_status == NodeStatus.WAITING)   OnStart();
        
        Debug.Log("OnRunning : " + name + ", status : " + _status);



        // ���������s������I��������
        //if (_status == NodeStatus.SUCCESS || _status == NodeStatus.FAILURE)    OnFinish();
        
        //�m�[�h�̏�Ԃ�Ԃ�
        return _status;
    }

    /// <summary>
    /// �m�[�h���s����������
    /// </summary>
    public virtual void OnFinish()
    {
        //�ҋ@�܂��͎��s���ŃG���[
        if (_status != NodeStatus.SUCCESS && _status != NodeStatus.FAILURE)
        {
            Debug.LogError("Not finished yet : " + name);
            return;
        }
        Debug.Log("OnFinish : " + name + ", status : " + _status);
    }
}

/// <summary>
/// �m�[�h�̃X�e�[�^�X
/// </summary>
public enum NodeStatus
{
    // �ҋ@��
    WAITING,
    // ����
    SUCCESS,
    // ���s
    FAILURE,
    // ���s��
    RUNNING,
}