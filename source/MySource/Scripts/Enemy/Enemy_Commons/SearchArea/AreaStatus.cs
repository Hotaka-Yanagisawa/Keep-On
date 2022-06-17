//不要なら消す存在です

using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/AreaStatus")]
public class AreaStatus : ScriptableObject
{
    /// <summary>
    /// 攻撃、索敵範囲用のコライダー
    /// </summary>
    public SphereCollider sphereCollider;

    /// <summary>
    /// コライダーの半径
    /// </summary>
    public float sphereRadius;

    /// <summary>
    /// コライダーの半径
    /// </summary>
    [Header("searchAngle * 2 = 視野の角度")]
    public float searchAngle = 180f;
}

