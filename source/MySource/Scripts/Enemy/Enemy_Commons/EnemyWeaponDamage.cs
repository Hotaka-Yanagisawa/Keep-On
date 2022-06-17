using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponDamage : MonoBehaviour
{
    [SerializeField] public MeshCollider Collider;
    public EnemyStatus status;
    // Start is called before the first frame update
    void Start()
    {
        //meshCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collider.enabled = false;
            Maeda.Player playern = other.GetComponent<Maeda.Player>();
            //other.gameObject.SendMessage("OnHitEnemyAttack", status.attack);
            playern.OnHitEnemyAttack(status.attack, other.ClosestPointOnBounds(transform.position));
            other.gameObject.SendMessage("OnKnockBack");
        }
    }
}
