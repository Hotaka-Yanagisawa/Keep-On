///////////////////////////////////////////////////////////////////////////
//// 作成日 2021/05/19
//// 作成者 柳沢帆貴
///////////////////////////////////////////////////////////////////////////
////
///////////////////////////////////////////////////////////////////////////
//// 概要
//// ボスの攻撃範囲
////
//// 更新日時
////
//// 2021/05/19 作成開始
////            視野の持たせるためのscript
////
//// 
////
////
////////////////////////////////////////////////////////////////////////////

//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.Events;

//namespace Homare
//{
//    public class BossAtkSearch : MonoBehaviour
//    {
//        /// <summary>
//        /// プレイヤー攻撃時の処理
//        /// </summary>
//        //[SerializeField] UnityEvent OnAttackSearch;
//        /// <summary>
//        /// プレイヤーを攻撃範囲から見失った時の処理の処理
//        /// </summary>
//        // [SerializeField] UnityEvent OnAttackSearchExit;
//        [SerializeField] Boss getBoss;
//        [SerializeField]
//        private SphereCollider searchArea;
//        [SerializeField]
//        [Tooltip("視野の半径")]
//        private float sphereRadius;
//        /// <summary>
//        /// searchAngle * 2 が視野になる
//        /// </summary>
//        [Header("searchAngle * 2 = 視野の角度")]
//        [SerializeField]
//        private float searchAngle = 180f;

//        private void Start()
//        {
//            searchArea.radius = sphereRadius;
//        }
//        private void OnTriggerEnter(Collider other)
//        {
//            if (other.tag != "Player") return;

//            //　主人公の方向
//            Vector3 playerDirection = other.transform.position - transform.position;
//            //　敵の前方からの主人公の方向
//            float angle = Vector3.Angle(transform.forward, playerDirection);
//            //　サーチする角度内だったら発見
//            if (angle <= searchAngle)
//            {
//                //攻撃状態へ
//            }
//        }
//        //private void OnTriggerStay(Collider other)
//        //{
//        //    if (other.tag != "Player") return;

//        //    //　主人公の方向
//        //    Vector3 playerDirection = other.transform.position - transform.position;
//        //    //　敵の前方からの主人公の方向
//        //    float angle = Vector3.Angle(transform.forward, playerDirection);
//        //    //　サーチする角度内だったら発見
//        //    if (angle <= searchAngle)
//        //    {
//        //        //攻撃状態へ
//        //        //OnAttackSearch.Invoke();
//        //        //oneMoreArea.enabled = false;
//        //    }
//        //}

//        //private void OnTriggerExit(Collider other)
//        //{
//            //if (other.tag != "Player") return;

//            //見逃したので他の状態へ
//            //OnAttackSearchExit.Invoke();
//            //oneMoreArea.enabled = true;
//            //searchArea.enabled = false;
//        //}
//#if UNITY_EDITOR
//        //　サーチする角度表示
//        private void OnDrawGizmos()
//        {
//            if (searchArea.enabled)
//            {
//                Handles.color = new Color(0.9f, 0.3f, 0, 0.05f);
//                Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
//            }
//        }
//#endif
//    }
//}