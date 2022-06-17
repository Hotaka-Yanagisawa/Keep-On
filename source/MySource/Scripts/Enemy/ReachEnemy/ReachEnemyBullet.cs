using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachEnemyBullet : MonoBehaviour
{
    private SphereCollider collider;
    private Rigidbody rb;
    private int damage;
    private int activeCnt;
    private AudioSource audioSource;

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        damage = 0;
        activeCnt = 0;
    }

    private void Update()
    {
        if (--activeCnt < 0)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collider.enabled = false;
            Maeda.Player playern = other.GetComponent<Maeda.Player>();
            //other.gameObject.SendMessage("OnHitEnemyAttack", status.attack);
            playern.OnHitEnemyAttack(damage, other.ClosestPointOnBounds(transform.position));
            other.gameObject.SendMessage("OnKnockBack");
        }
    }

    public void Shot(Vector3 dir,int damagePower)
    {
        rb.AddForce(Vector3.Normalize(dir) * 40f, ForceMode.Impulse);
        damage = damagePower;
        activeCnt = 120;
        audioSource.Play();
        Mizuno.SoundManager.Instance.PlayMenuSe("SE_Beam");
    }
}
