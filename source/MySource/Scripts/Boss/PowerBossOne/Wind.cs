/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/04/03
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/04/03 �쐬�J�n
//            ���̂悤�Ȃ��̂Ńv���C���[�𐁂���΂�(�G�̍U��)
// 
//
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour
{
    [SerializeField] float coefficient;   // ��C��R�W��
    [SerializeField] SphereCollider sphere;
    Vector3 velocity;                       // ����
    [SerializeField] public float windPower = 5;       //����

    //private void Update()
    //{
    //    sphere.transform.localScale += new Vector3(1, 1, 1);
    //}

    void OnTriggerStay(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        if (col.GetComponent<Rigidbody>() == null)
        {
            return;
        }

        float Angle = Mathf.Atan2(col.transform.position.z - transform.position.z,
                                  col.transform.position.x - transform.position.x);

        Vector3 moveDistance = transform.position - col.transform.position;
        float disWindPower = windPower;
        //�v���C���[�Ƃ̋����ŕ��̗͂��ς��
        if (sphere.radius * 0.8f < moveDistance.magnitude)
        {
            disWindPower = windPower * 0.5f;
        }
        if (sphere.radius * 0.5f < moveDistance.magnitude)
        {
            disWindPower = windPower * 0.8f;
        }

        velocity = new Vector3(Mathf.Cos(Angle) * disWindPower, 0, Mathf.Sin(Angle) * disWindPower);
        var playerVelocity = col.GetComponent<Rigidbody>().velocity;
        var tempVelocity = new Vector3(playerVelocity.x, 0, playerVelocity.z);

        // ���Α��x�v�Z
        var relativeVelocity = velocity - playerVelocity;

        // ��C��R��^����
        //if (tempVelocity.magnitude > 10)
        if (col.GetComponent<Maeda.Player>().isStep)
        {
            col.GetComponent<Rigidbody>().velocity *= 0.99f;
            //col.GetComponent<Rigidbody>().AddForce()
        }
        else
        {
            col.GetComponent<Rigidbody>().AddForce(coefficient * relativeVelocity);
        }
    }
}