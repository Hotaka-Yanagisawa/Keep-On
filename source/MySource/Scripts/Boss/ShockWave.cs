using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    //[SerializeField] public Collider footCollider;
    [SerializeField] public Collider groundCollider;
    float t;
    public EnemyStatus status;
    [SerializeField] float speed = 5;
    [SerializeField] Vector3 start;
    [SerializeField] Vector3 end;

    void Start()
    {
        //groundCollider.enabled = false;
      
    }

    void Update()
    {
        t += Time.deltaTime * speed;
        transform.localScale = Vector3.Lerp(start, end, t);
        if (t >= 1)
        {
            enabled = false;
            groundCollider.enabled = false;

        }
    }

    public void Init()
    {
        groundCollider.enabled = true;
        t = 0;
        transform.localScale = start;
    }

    private void OnEnable()
    {
        //groundCollider.enabled = true;
        //t = 0;
        //transform.localScale = start;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerFoot"))
        {
            t = 0;
            transform.localScale = start;
            if (other.transform.position.y > 0.3f)
            {
                groundCollider.enabled = false;

                return;
            }
            groundCollider.enabled = false;
        }

        if (other.CompareTag("Player"))
        {
            t = 0;
            transform.localScale = start;
            if (other.transform.position.y > 0.3f)
            {
                groundCollider.enabled = false;

                return;
            }
            groundCollider.enabled = false;
            Maeda.Player playern = other.GetComponent<Maeda.Player>();
            //other.gameObject.SendMessage("OnHitEnemyAttack", status.attack);
            playern.OnHitBossAttack(status.attack, other.ClosestPointOnBounds(transform.position));
            other.gameObject.SendMessage("OnKnockBack");
        }
    }
}