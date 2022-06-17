/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/04/28
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/04/28 �쐬�J�n
//              �q�[���h���[���̓����̕���                 
//
//////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homare;
using UnityEngine.VFX;

namespace Hisada
{
    public class HealDrone : MonoBehaviour
    {
        [SerializeField] Rigidbody rb;
        [SerializeField] float moveSpeed = 1;
        [SerializeField] float applySpeed = 1;
        [SerializeField] float Radius;
        [SerializeField] Transform escapeTransform;
        [SerializeField] GameObject HealEffect;
        
        Transform lockOnPoint;
        Vector3 firstPos;
        Quaternion firstQuaternion;
        float Angle;
        HealDrone_Style style;
        int waitCount = 0;

        private enum HealDrone_Style
        {
            GO_AROWND,
            ESCAPE,
            SPAWN,
            WAIT,
        }

        void Start()
        {
            firstPos = transform.position;
            firstQuaternion = transform.rotation;
            lockOnPoint = transform.Find("LockOnPoint");
            Angle = 0;
            style = HealDrone_Style.GO_AROWND;
        }

        void FixedUpdate()
        {
            if (style == HealDrone_Style.GO_AROWND)
                Go_Around();
            else if (style == HealDrone_Style.ESCAPE)
                Escape();
            else if (style == HealDrone_Style.WAIT)
                Wait();
            else if (style == HealDrone_Style.SPAWN)
                Spawn();
        }

        void Go_Around()
        {
            Angle += 0.01f;

            //�~�ړ�
            float x = Mathf.Sin(Angle) * Radius;
            float z = Mathf.Cos(Angle) * Radius;

            rb.velocity = new Vector3(x, 0, z);
            rb.velocity = rb.velocity.normalized * moveSpeed;

            // �G�̊p�x�̍X�V
            // Slerp:���݂̌����A�������������A��������X�s�[�h
            // LookRotation(������������):
            transform.rotation =
                    Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(rb.velocity),
                    applySpeed);
        }

        void Escape()
        {
            // Point��Drone�̋���
            float distance = Mathf.Abs(Vector3.Distance(transform.position, escapeTransform.position));

            // �̂��G�X�P�[�v�ʒu�֌�������
            transform.rotation =
                Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(escapeTransform.position - transform.position),
                applySpeed);

            // �ړ����x
            rb.velocity = transform.forward * 5;


            // �G�X�P�[�v�ʒu�܂ŕt������ҋ@��ԂցA������Wait�t���[����ݒ�
            if (distance < 1)
            {
                rb.velocity = Vector3.zero;
                style = HealDrone_Style.WAIT;
                waitCount = 1200;
            }
            Ohira.CameraController.Instance.RemoveLockOn(gameObject);
        }


        void Wait()
        {
            if (--waitCount < 0)
            {
                style = HealDrone_Style.SPAWN;
                HealEffect.SetActive(true);
            }
            Ohira.CameraController.Instance.RemoveLockOn(gameObject);
        }

        void Spawn()
        {
            // �����ɏ����ʒu�֖߂鏈�����L�q
            transform.rotation =
                 Quaternion.Slerp(transform.rotation,
                 Quaternion.LookRotation(firstPos - transform.position),
                 10);
            // �����ʒu�߂��֕t������GO_AROUND��Ԃֈڍs
            float distance = Mathf.Abs(Vector3.Distance(transform.position, firstPos));
            rb.velocity = transform.forward * 5;


            // �G�X�P�[�v�ʒu�܂ŕt�����珄���ԂցA�����Ƀ��b�N�I����L����
            if (distance < 1)
            {
                rb.velocity = Vector3.zero;
                style = HealDrone_Style.GO_AROWND;
                lockOnPoint.gameObject.SetActive(true);
                transform.rotation = firstQuaternion;
                Angle = 0f;
            }
            Ohira.CameraController.Instance.RemoveLockOn(gameObject);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name != "Satellite") return;

            style = HealDrone_Style.ESCAPE;
            Ohira.CameraController.Instance.RemoveLockOn(gameObject);
            lockOnPoint.gameObject.SetActive(false);
            HealEffect.SetActive(false);
        }


    }
}
