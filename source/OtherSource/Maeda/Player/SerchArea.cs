// _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//  SerchArea.cs
// _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//�@�쐬�����F2021/02/27
//  �쐬�ҁ@�F�O�c����
// _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//  �X�V����
//    �F�X�N���v�g�쐬                         2021/02/27
//    �F���G�͈͂̏����쐬                     2021/02/28
//    �F�v���C���[�ւ̒ǐՏ����쐬             2021/02/28
// _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SerchArea : MonoBehaviour
{
    public GameObject player;                       // �v���C���[�I�u�W�F�N�g
    public float moveSpeed = 5.0f;                  // �A�C�e���̈ړ����x
   
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        // ���G�͈͗p�̋�̃I�u�W�F�N�g�̃|�W�V������ݒ�
        this.transform.position = player.transform.position;  
    }


    // ���G�͈͓��Ƀv���C���[�������Ă��Ă��邩
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "ItemRed")
        {           
            // �ǐՂ�������̐ݒ�
            Vector3 dir = (player.transform.position - other.transform.position).normalized;

            // ���̕����֎w�肵�����x�Ői��
            float vx = dir.x * moveSpeed;
            float vz = dir.z * moveSpeed;
            other.transform.Translate(vx / 50, 0, vz / 50);
        }

        if (other.gameObject.tag == "ItemBlue")
        {        
            // �ǐՂ�������̐ݒ�
            Vector3 dir = (player.transform.position - other.transform.position).normalized;

            // ���̕����֎w�肵�����x�Ői��
            float vx = dir.x * moveSpeed;
            float vz = dir.z * moveSpeed;
            other.transform.Translate(vx / 50, 0, vz / 50);
        }

        if (other.gameObject.tag == "ItemGreen")
        {
            // �ǐՂ�������̐ݒ�
            Vector3 dir = (player.transform.position - other.transform.position).normalized;

            // ���̕����֎w�肵�����x�Ői��
            float vx = dir.x * moveSpeed;
            float vz = dir.z * moveSpeed;
            other.transform.Translate(vx / 50, 0, vz / 50);
        }
    }

    
}