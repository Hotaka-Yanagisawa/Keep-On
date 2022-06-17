/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/18
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
//
/////////////////////////////////////////////////////////////////////////
// �T�v
// �G�̎�����Ƀv���C���[���N������ƒǐՏ�Ԃֈڍs����
// ����O�ɏo��Ƒҋ@��Ԃֈڍs����
//
// �X�V����
//
// 2021/03/18 �쐬�J�n
//            ����̎������邽�߂�script
//      03/27 �ǐՏ�Ԃ̎��v���C���[�����G����͈͂�傫������
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

public class OnlyForwardSearch : MonoBehaviour
{
    /// <summary>
    /// �v���C���[�������̏���
    /// </summary>
    [SerializeField] UnityEvent OnSearch;
    [SerializeField] UnityEvent OnSearchEnter;
    /// <summary>
    /// �v���C���[�������������̏����̏���
    /// </summary>
    [SerializeField] UnityEvent OnSearchExit;
    [SerializeField]
    private SphereCollider oneMoreArea;
    [SerializeField]
    private SphereCollider searchArea;
    [SerializeField] [Tooltip("����̔��a")]
    private float sphereRadius;
    [SerializeField][Tooltip("�ǐՏ�Ԏ�����̔��a")]
    private float stateTrackingSphereRadius;
    /// <summary>
    /// searchAngle * 2 ������ɂȂ�
    /// </summary>
    [Header("searchAngle * 2 = ����̊p�x")]
    [SerializeField]
    private float searchAngle = 130f;
    [SerializeField]
    [Header("�����Ə����y���A�Ⴂ�Ɛ��x����")]
    private int maxCnt;
    private int cnt;
    private bool isOnOff;
    private void Start()
    {
        searchArea.radius = sphereRadius;
        cnt = maxCnt;
        isOnOff = true;
    }

    private void OnEnable()
    {
        searchArea.radius = sphereRadius;
        cnt = maxCnt;
        isOnOff = true;
    }

    private void FixedUpdate()
    {
        //��莞�Ԃ̊Ԃ̂ݎ��삪���݂���i�������y�����邽�߁j
        if (isOnOff)
        {
            cnt--;
            if (cnt <= 0)
            {
                cnt = maxCnt;
                searchArea.enabled ^= true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        var playerDirection = other.transform.position - transform.position;
        //�@�G�̑O������̎�l���̕���
        var angle = Vector3.Angle(transform.forward, playerDirection);
        //�@�T�[�`����p�x���������甭��
        if (angle <= searchAngle)
            OnSearchEnter?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;

        //�@��l���̕���
        var playerDirection = other.transform.position - transform.position;
        //�@�G�̑O������̎�l���̕���
        var angle = Vector3.Angle(transform.forward, playerDirection);
        //�@�T�[�`����p�x���������甭��
        if (angle <= searchAngle)
        {
            isOnOff = false;
            searchArea.enabled = true;
            searchArea.radius = stateTrackingSphereRadius;
            //Debug.Log("��l������: " + angle);
            //�ǐՏ�Ԃ�
            OnSearch?.Invoke();
            if(oneMoreArea != null)
                oneMoreArea.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            searchArea.radius = sphereRadius;
            isOnOff = true;
            //���������̂őҋ@��Ԃ�
            OnSearchExit?.Invoke();
            if (oneMoreArea != null)
                oneMoreArea.enabled = false;
        }
    }
#if UNITY_EDITOR
    //�@�T�[�`����p�x�\��
    private void OnDrawGizmos()
    {
        if (searchArea.enabled)
        {
            Handles.color = new Color(0.7f, 0, 0, 0.1f);
            Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
        }
    }
#endif
}