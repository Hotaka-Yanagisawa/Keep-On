//�s�v�Ȃ�������݂ł�

using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/AreaStatus")]
public class AreaStatus : ScriptableObject
{
    /// <summary>
    /// �U���A���G�͈͗p�̃R���C�_�[
    /// </summary>
    public SphereCollider sphereCollider;

    /// <summary>
    /// �R���C�_�[�̔��a
    /// </summary>
    public float sphereRadius;

    /// <summary>
    /// �R���C�_�[�̔��a
    /// </summary>
    [Header("searchAngle * 2 = ����̊p�x")]
    public float searchAngle = 180f;
}

