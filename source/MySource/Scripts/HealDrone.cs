/////////////////////////////////////////////////////////////////////////
// 作成日 2021/04/28
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/04/28 作成開始
//              ヒールドローンの動きの部分                 
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

            //円移動
            float x = Mathf.Sin(Angle) * Radius;
            float z = Mathf.Cos(Angle) * Radius;

            rb.velocity = new Vector3(x, 0, z);
            rb.velocity = rb.velocity.normalized * moveSpeed;

            // 敵の角度の更新
            // Slerp:現在の向き、向きたい方向、向かせるスピード
            // LookRotation(向きたい方向):
            transform.rotation =
                    Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(rb.velocity),
                    applySpeed);
        }

        void Escape()
        {
            // PointとDroneの距離
            float distance = Mathf.Abs(Vector3.Distance(transform.position, escapeTransform.position));

            // 体をエスケープ位置へ向かせる
            transform.rotation =
                Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(escapeTransform.position - transform.position),
                applySpeed);

            // 移動速度
            rb.velocity = transform.forward * 5;


            // エスケープ位置まで付いたら待機状態へ、同時にWaitフレームを設定
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
            // ここに初期位置へ戻る処理を記述
            transform.rotation =
                 Quaternion.Slerp(transform.rotation,
                 Quaternion.LookRotation(firstPos - transform.position),
                 10);
            // 初期位置近くへ付いたらGO_AROUND状態へ移行
            float distance = Mathf.Abs(Vector3.Distance(transform.position, firstPos));
            rb.velocity = transform.forward * 5;


            // エスケープ位置まで付いたら巡回状態へ、同時にロックオンを有効化
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
