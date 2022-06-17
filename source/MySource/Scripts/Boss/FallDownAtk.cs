using System.Collections.Generic;
using UnityEngine;

namespace Homare
{

    public class FallDownAtk : MonoBehaviour
    {
        [SerializeField] public Collider footCollider;
        [SerializeField] GameObject shockWave;
        [SerializeField] Boss boss;

        //[SerializeField] public Collider groundCollider;

        public EnemyStatus status;
        void Start()
        {
            //footCollider.enabled = false;
            //shockWave = GameObject.Find("shockWave");
        }

        void Update()
        {

        }

        private void OnEnable()
        {
            footCollider.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                footCollider.enabled = false;
                enabled = false;
                gameObject.SetActive(false);
                Maeda.Player playern = other.GetComponent<Maeda.Player>();
                //other.gameObject.SendMessage("OnHitEnemyAttack", status.attack);
                playern.OnHitBossAttack(status.attack, other.ClosestPointOnBounds(transform.position));
                other.gameObject.SendMessage("OnKnockBack");
                //Mizuno.SoundManager.Instance.PlayMenuSe("SE_Explosion2");
                boss.audio.PlayOneShot(boss.audioClip);

            }

            if (other.CompareTag("Stage"))
            {
                footCollider.enabled = false;
                enabled = false;
                gameObject.SetActive(false);

                shockWave.SetActive(true);
                ShockWave shock = shockWave.GetComponent<ShockWave>();
                shock.enabled = true;
                shockWave.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                shock.Init();
                // Mizuno.SoundManager.Instance.PlayMenuSe("SE_Explosion2");
                boss.audio.PlayOneShot(boss.audioClip);

                GetComponent<EffectOperate>().CreateEffect(9, transform.position, 1f);
            }
        }
    }
}