/////////////////////////////////////////////////////////////////////////
// 作成日 2021/03/18
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
//
/////////////////////////////////////////////////////////////////////////
// 概要
// 敵の視野内にプレイヤーが侵入すると追跡状態へ移行する
// 視野外に出ると待機状態へ移行する
//
// 更新日時
//
// 2021/03/18 作成開始
//            視野の持たせるためのscript
//      03/27 追跡状態の時プレイヤーを索敵する範囲を大きくした
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
    /// プレイヤー発見時の処理
    /// </summary>
    [SerializeField] UnityEvent OnSearch;
    [SerializeField] UnityEvent OnSearchEnter;
    /// <summary>
    /// プレイヤーを見失った時の処理の処理
    /// </summary>
    [SerializeField] UnityEvent OnSearchExit;
    [SerializeField]
    private SphereCollider oneMoreArea;
    [SerializeField]
    private SphereCollider searchArea;
    [SerializeField] [Tooltip("視野の半径")]
    private float sphereRadius;
    [SerializeField][Tooltip("追跡状態時視野の半径")]
    private float stateTrackingSphereRadius;
    /// <summary>
    /// searchAngle * 2 が視野になる
    /// </summary>
    [Header("searchAngle * 2 = 視野の角度")]
    [SerializeField]
    private float searchAngle = 130f;
    [SerializeField]
    [Header("高いと処理軽く、低いと精度悪く")]
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
        //一定時間の間のみ視野が存在する（処理を軽くするため）
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
        //　敵の前方からの主人公の方向
        var angle = Vector3.Angle(transform.forward, playerDirection);
        //　サーチする角度内だったら発見
        if (angle <= searchAngle)
            OnSearchEnter?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;

        //　主人公の方向
        var playerDirection = other.transform.position - transform.position;
        //　敵の前方からの主人公の方向
        var angle = Vector3.Angle(transform.forward, playerDirection);
        //　サーチする角度内だったら発見
        if (angle <= searchAngle)
        {
            isOnOff = false;
            searchArea.enabled = true;
            searchArea.radius = stateTrackingSphereRadius;
            //Debug.Log("主人公発見: " + angle);
            //追跡状態へ
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
            //見逃したので待機状態へ
            OnSearchExit?.Invoke();
            if (oneMoreArea != null)
                oneMoreArea.enabled = false;
        }
    }
#if UNITY_EDITOR
    //　サーチする角度表示
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