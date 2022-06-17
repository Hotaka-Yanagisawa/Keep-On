using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
namespace Homare
{
    public class ExtinctionBeam : MonoBehaviour
    {
        //直線軌道の処理作る
        //Fallzone消す処理作る
        private GameObject targetObj;
        [SerializeField] float moveSpeed;
        [SerializeField] Rigidbody rb;
        //[SerializeField] GameObject pop;
        //public EnemyStatus status;
        [SerializeField] int damage=10;

        //[System.NonSerialized] public float shootingTime = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            float step = Time.deltaTime * moveSpeed;
            Vector3 vector = Vector3.MoveTowards(
                                transform.position,
                                targetObj.transform.position + Vector3.down * 5,
                                step);

            transform.position = vector;
            // 敵の角度の更新
            // Slerp:現在の向き、向きたい方向、向かせるスピード
            // LookRotation(向きたい方向):
            transform.rotation =
                Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(Vector3.up),
                0.1f);

        }

        public void SetTargetPos(GameObject obj)
        {
            targetObj = obj;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                targetObj.SetActive(false);
                gameObject.SetActive(false);

                //ダメージ与える
                Maeda.Player playern = other.GetComponent<Maeda.Player>();
                //other.gameObject.SendMessage("OnHitEnemyAttack", status.attack);
                playern.OnHitBossAttack(damage, other.ClosestPointOnBounds(transform.position));
                other.gameObject.SendMessage("OnKnockBack");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Stage"))
            {
                targetObj.SetActive(false);

                gameObject.SetActive(false);
                //GameObject newPop = Instantiate(pop, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                GetComponent<EffectOperate>().CreateEffect(4, new Vector3(transform.position.x, 0, transform.position.z), 2f);
            }
        }
    }
}