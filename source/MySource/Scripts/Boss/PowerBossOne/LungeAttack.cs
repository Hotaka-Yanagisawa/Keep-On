/////////////////////////////////////////////////////////////////////////
// 作成日 2021/04/02
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/04/02 作成開始
//            突進攻撃用のコライダーにつけるscript
// 
//
//
//////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungeAttack : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * 90.0f, ForceMode.Impulse);
            GetComponent<BoxCollider>().enabled = false;
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
    }
}
