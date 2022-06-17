// _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//  SerchArea.cs
// _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//　作成日時：2021/02/27
//  作成者　：前田理玖
// _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
//  更新履歴
//    ：スクリプト作成                         2021/02/27
//    ：索敵範囲の処理作成                     2021/02/28
//    ：プレイヤーへの追跡処理作成             2021/02/28
// _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SerchArea : MonoBehaviour
{
    public GameObject player;                       // プレイヤーオブジェクト
    public float moveSpeed = 5.0f;                  // アイテムの移動速度
   
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        // 索敵範囲用の空のオブジェクトのポジションを設定
        this.transform.position = player.transform.position;  
    }


    // 索敵範囲内にプレイヤーが入ってきているか
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "ItemRed")
        {           
            // 追跡する方向の設定
            Vector3 dir = (player.transform.position - other.transform.position).normalized;

            // その方向へ指定した速度で進む
            float vx = dir.x * moveSpeed;
            float vz = dir.z * moveSpeed;
            other.transform.Translate(vx / 50, 0, vz / 50);
        }

        if (other.gameObject.tag == "ItemBlue")
        {        
            // 追跡する方向の設定
            Vector3 dir = (player.transform.position - other.transform.position).normalized;

            // その方向へ指定した速度で進む
            float vx = dir.x * moveSpeed;
            float vz = dir.z * moveSpeed;
            other.transform.Translate(vx / 50, 0, vz / 50);
        }

        if (other.gameObject.tag == "ItemGreen")
        {
            // 追跡する方向の設定
            Vector3 dir = (player.transform.position - other.transform.position).normalized;

            // その方向へ指定した速度で進む
            float vx = dir.x * moveSpeed;
            float vz = dir.z * moveSpeed;
            other.transform.Translate(vx / 50, 0, vz / 50);
        }
    }

    
}