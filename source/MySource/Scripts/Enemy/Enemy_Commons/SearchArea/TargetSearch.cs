/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/21
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
//
/////////////////////////////////////////////////////////////////////////
// �T�v
// �G�̍U���͈�
//
// �X�V����
//
// 2021/03/21 �쐬�J�n
//            ����̎������邽�߂�script
//
// 
//
//
//////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class TargetSearch : MonoBehaviour
{
    /// <summary>
    /// �v���C���[�U�����̏���
    /// </summary>
    [SerializeField] UnityEvent OnAttackSearch;
    /// <summary>
    /// �v���C���[���U���͈͂��猩���������̏����̏���
    /// </summary>
    [SerializeField] UnityEvent OnAttackSearchExit;
    [SerializeField]
    private SphereCollider oneMoreArea;
    [SerializeField]
    private SphereCollider searchArea;
    [SerializeField] [Tooltip("����̔��a")]
    private float sphereRadius;
    /// <summary>
    /// searchAngle * 2 ������ɂȂ�
    /// </summary>
    [Header("searchAngle * 2 = ����̊p�x")]
    [SerializeField]
    private float searchAngle = 180f;

    private void Start()
    {
        searchArea.radius = sphereRadius;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;

        //�@��l���̕���
        Vector3 playerDirection = other.transform.position - transform.position;
        //�@�G�̑O������̎�l���̕���
        float angle = Vector3.Angle(transform.forward, playerDirection);
        //�@�T�[�`����p�x���������甭��
        if (angle <= searchAngle)
        {
            //�U����Ԃ�
            OnAttackSearch.Invoke();
            oneMoreArea.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        //���������̂ő��̏�Ԃ�
        OnAttackSearchExit.Invoke();
        oneMoreArea.enabled = true;
        searchArea.enabled = false;
    }
#if UNITY_EDITOR
    //�@�T�[�`����p�x�\��
    private void OnDrawGizmos()
    {
        if (searchArea.enabled)
        {
            Handles.color = new Color(0.9f, 0.3f, 0, 0.05f);
            Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
        }
    }
#endif
}