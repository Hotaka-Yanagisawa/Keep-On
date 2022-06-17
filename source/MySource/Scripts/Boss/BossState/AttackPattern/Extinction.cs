using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Homare
{
    public class Extinction : MonoBehaviour
    {
        [Header("秒")]
        float createCnt;                                            //カウントダウン
        [SerializeField] float createTime = 0.3f;                   //生成間隔
        [SerializeField] float spawnRange = 20;
        [Header("一回の爆撃")] [SerializeField] int createNum = 500; //一回の爆撃の個数
        int createNumCnt;                                           //一回の爆撃のカウント数
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
            // 無効なオブジェクトから選択して有効化
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