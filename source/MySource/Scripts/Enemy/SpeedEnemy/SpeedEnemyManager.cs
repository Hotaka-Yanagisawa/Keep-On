using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEnemyManager : MonoBehaviour
{
    [SerializeField] private Collider playerCollider;

    private void Start()
    {
        Hisada.SpeedEnemy.playerCollider = playerCollider;
    }
}
