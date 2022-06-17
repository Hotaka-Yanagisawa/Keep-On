using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Homare
{
    public class Extinction : MonoBehaviour
    {
        [Header("�b")]
        float createCnt;                                            //�J�E���g�_�E��
        [SerializeField] float createTime = 0.3f;                   //�����Ԋu
        [SerializeField] float spawnRange = 20;
        [Header("���̔���")] [SerializeField] int createNum = 500; //���̔����̌�
        int createNumCnt;                                           //���̔����̃J�E���g��
        [SerializeField] GameObject beam;
        [SerializeField] GameObject zone;
        // Start is called before the first frame update
        void Start()
        {
            createCnt = createTime;
            createNumCnt = 0;
        }

        private void OnEnable()
        {
            createNumCnt = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (createNumCnt > createNum)
            {
                this.enabled = false;

                return;
            }
            createCnt -= Time.deltaTime;
            // �����ȃI�u�W�F�N�g����I�����ėL����
            if (createCnt <= 0)
            {
                createNumCnt++;
                createCnt = createTime;

                CreateBeam(CreateFallZone());
            }
        }

        GameObject CreateFallZone()
        {
            Vector3 randomPos;
            Transform tr;
            //foreach (GameObject childZone in tr)
            for (int i = 0; i < transform.childCount; i++)
            {
                tr = transform.GetChild(i);
                if (tr.gameObject.name != "FallZone") continue;

                if (tr.gameObject.activeSelf) continue;
                randomPos = new Vector3(Random.Range(-spawnRange, spawnRange), 0.1f, Random.Range(-spawnRange, spawnRange));
                tr.SetPositionAndRotation(tr.position + randomPos, Quaternion.identity);
                tr.gameObject.SetActive(true);
                return tr.gameObject;
            }

            randomPos = new Vector3(Random.Range(-spawnRange, spawnRange), 0.1f, Random.Range(-spawnRange, spawnRange));
            GameObject newZone = Instantiate(zone, transform.position + randomPos, Quaternion.identity, transform);
            newZone.name = zone.name;
            return newZone;
        }

        void CreateBeam(GameObject obj)
        {
            Vector3 pos;
            Transform tr;

            //foreach (GameObject childZone in tr)
            for (int i = 0; i < transform.childCount; i++)
            {
                tr = transform.GetChild(i);
                if (tr.gameObject.name != "extinction") continue;
                if (tr.gameObject.activeSelf) continue;
                pos = new Vector3(obj.transform.position.x, 30, obj.transform.position.z);
                tr.SetPositionAndRotation(pos, tr.rotation);
                tr.gameObject.SetActive(true);
                tr.gameObject.GetComponent<ExtinctionBeam>().SetTargetPos(obj);
                return;
            }

            GameObject newBeam = Instantiate(beam, transform);
            newBeam.name = beam.name;
            pos = new Vector3(obj.transform.position.x, 30, obj.transform.position.z);
            newBeam.transform.SetPositionAndRotation(pos, newBeam.transform.rotation);
            newBeam.GetComponent<ExtinctionBeam>().SetTargetPos(obj);
        }
    }
}