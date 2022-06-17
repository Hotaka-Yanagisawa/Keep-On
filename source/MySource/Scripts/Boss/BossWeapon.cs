using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    [SerializeField] public Collider bladeCollider;
    [SerializeField] public Collider handleCollider;
    //[SerializeField] public Collider rHandCollider;
    //[SerializeField] public Collider lHandCollider;
    
    //public EnemyStatus status;
    void Start()
    {
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bladeCollider.enabled = false;
            handleCollider.enabled = false;
            Maeda.Player playern = other.GetComponent<Maeda.Player>();
            //other.gameObject.SendMessage("OnHitEnemyAttack", status.attack);
            playern.OnHitBossAttack(25, other.ClosestPointOnBounds(transform.position));
            other.gameObject.SendMessage("OnKnockBack");
        }
    }
}