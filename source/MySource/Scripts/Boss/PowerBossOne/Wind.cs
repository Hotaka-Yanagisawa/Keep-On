/////////////////////////////////////////////////////////////////////////
// 作成日 2021/04/03
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/04/03 作成開始
//            風のようなものでプレイヤーを吹き飛ばす(敵の攻撃)
// 
//
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour
{
    [SerializeField] float coefficient;   // 空気抵抗係数
    [SerializeField] SphereCollider sphere;
    Vector3 velocity;                       // 風速
    [SerializeField] public float windPower = 5;       //風力

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
        //プレイヤーとの距離で風の力が変わる
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

        // 相対速度計算
        var relativeVelocity = velocity - playerVelocity;

        // 空気抵抗を与える
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