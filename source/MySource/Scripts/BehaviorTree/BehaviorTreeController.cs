using UnityEngine;

public class BehaviorTreeController
{
    // ���ݑ����Ă���m�[�h
    private BaseNode _rootNode = null;
    // �ŏI���ʂ̃X�e�[�^�X
    private NodeStatus _resultStatus = NodeStatus.WAITING;

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="node">rootNode</param>
    public void Initialize(BaseNode node)
    {
        //�m�[�h�̏����ݒ�
        //BaseNode���p�����Ă���m�[�h����������B���Ԑ�
        _rootNode = node;
    }

    /// <summary>
    /// BehaviorTree�̋N��
    /// </summary>
    public void OnStart()
    {
        Debug.Log("BehaviorTree start");
        if (_resultStatus != NodeStatus.WAITING)
        {
            Debug.LogError("status is waiting");
            return;
        }
        _resultStatus = NodeStatus.RUNNING;
        // �m�[�h�N������
        _rootNode.OnStart();
    }

    /// <summary>
    /// BehaviorTree�̍X�V����
    /// </summary>
    public void OnRunning()
    {
        // BehaviorTree�̎��s���������Ă��� ���ꂽ�Ԃ񂢂�Ȃ�
        //if (_resultStatus == NodeStatus.SUCCESS || _resultStatus == NodeStatus.FAILURE) return;
        // �ҋ@���̓G���[
        //if (_resultStatus == NodeStatus.WAITING)
        //{
        //    Debug.LogError("status is waiting");
        //    return;
        //}
        // �m�[�h�J��Ԃ��N������
        _resultStatus = _rootNode.OnRunning();
        // BehaviorTree�̎��s���������Ă���
        //if (_resultStatus == NodeStatus.SUCCESS || _resultStatus == NodeStatus.FAILURE)
        //{
        //    _rootNode.OnFinish();
        //    OnFinish();
        //    Debug.Log("BehaviorTree result : " + _rootNode.status);
        //}
    }

    /// <summary>
    /// BehaviorTree�̊�������
    /// </summary>
    public void OnFinish()
    {
        //�G���[�`�F�b�N���Ԃ񂢂�Ȃ�
        if (_resultStatus != NodeStatus.SUCCESS && _resultStatus != NodeStatus.FAILURE)
        {
            Debug.LogError("unexpected results are coming back. status is : " + _resultStatus);
            return;
        }
        Debug.Log("BehaviorTree finish");
        OnStart();
        return;
    }
}