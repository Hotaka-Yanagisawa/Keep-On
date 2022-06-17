using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BaseNode���p�����Ă��肳���
/// �u�����`�ƂȂ��m�[�h�Ɍp������
/// </summary>
public class BranchNode : BaseNode
{
    //�q�m�[�h�̃��X�g
    protected List<BaseNode> _childNodeList = new List<BaseNode>();
    //�q�m�[�h�̔ԍ�
    protected int _childIndex = 0;

    /// <summary>
    /// �m�[�h�N��������
    /// </summary>
    public override void OnStart()
    {
        //�p�����̏���
        base.OnStart();
        //�C���f�b�N�X�̏�����
        _childIndex = 0;
        //���X�g����Ȃ�G���[
        if (_childNodeList.Count == 0)
        {
            Debug.LogError("not child");
            return;
        }
    }

    /// <summary>
    /// �q�̃X�e�[�^�X��]������
    /// </summary>
    /// <returns>status</returns>
    protected virtual NodeStatus EvaluateChild()
    {
        //�������̂Ō��
        return NodeStatus.WAITING;
    }

    /// <summary>
    /// �q��ǉ�����
    /// </summary>
    /// <param name="child">�ǉ�����q�m�[�h</param>
    public virtual void AddChild(BaseNode child)
    {
        _childNodeList.Add(child);
    }

    /// <summary>
    /// �q�m�[�h���X�g���擾
    /// </summary>
    /// <returns>_childNodeList</returns>
    public virtual List<BaseNode> GetChildList()
    {
        return _childNodeList;
    }
}