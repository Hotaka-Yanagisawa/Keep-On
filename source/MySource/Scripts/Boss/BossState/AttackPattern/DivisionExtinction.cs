using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Homare
{
    public class DivisionExtinction : MonoBehaviour
    {
        [Header("�b")]
        float createCnt;                                            //�J�E���g�_�E��
        float createCnt2;                                            //�J�E���g�_�E��
        float createCnt3;                                            //�J�E���g�_�E��

        [SerializeField] float createTime = 0.3f;                   //�����Ԋu
        [SerializeField] float createTime2 = 0.3f;                   //�����Ԋu
        [SerializeField] float createTime3 = 0.3f;                   //�����Ԋu

        [SerializeField] float spawnRange = 20;
        [Header("���̔���")] [SerializeField] int createNum = 500; //���̔����̌�
        int createNumCnt;                                           //���̔����̃J�E���g��
        [SerializeField] GameObject beam;
        [SerializeField] GameObject zone;
        [SerializeField] float radius;
        //[SerializeField] float maxShotTime;
        // Start is called before the first frame update
        void Start()
        {
            createCnt = createTime;
            createCnt2 = createTime2;
            createCnt3 = createTime3;

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
            createCnt2 -= Time.deltaTime;
            createCnt3 -= Time.deltaTime;

            // �����ȃI�u�W�F�N�g����I�����ėL����
            if (createCnt <= 0)
            {
                createNumCnt++;
                createCnt = createTime;

                for (int i = 0; i < 8; i++)
                {
                    CreateBeam(CreateFallZone(i, 1));
                }
            }
            // �����ȃI�u�W�F�N�g����I�����ėL����
            if (createCnt2 <= 0)
            {
                createNumCnt++;
                createCnt2 = createTime2;
                for (int i = 0; i < 8; i++)
                {
                    CreateBeam(CreateFallZone(i, 1.75f));
                }
            }
            // �����ȃI�u�W�F�N�g����I�����ėL����
            if (createCnt3 <= 0)
            {
                createNumCnt++;
                createCnt3 = createTime3;
                for (int i = 0; i < 8; i++)
                {
                    CreateBeam(CreateFallZone(i, 2.5f));
                }
            }
        }

        GameObject CreateFallZone(int num, float createPos)
        {
            Transform tr;
            Vector3 divisionPos = Vector3.zero;
            if (createPos != 1.75f)
            {
                switch (num)
                {
                    case 0:
                        divisionPos = Vector3.forward;
                        break;
                    case 1:
                        divisionPos = Vector3.forward + Vector3.right;
                        break;
                    case 2:
                        divisionPos = Vector3.right;
                        break;
                    case 3:
                        divisionPos = Vector3.back + Vector3.right;
                        break;
                    case 4:
                        divisionPos = Vector3.back;
                        break;
                    case 5:
                        divisionPos = Vector3.back + Vector3.left;
                        break;
                    case 6:
                        divisionPos = Vector3.left;
                        break;
                    case 7:
                        divisionPos = Vector3.forward + Vector3.left;
                        break;
                }
            }
            else
            {
                switch (num)
                {
                    case 0:
                        divisionPos = Vector3.forward + Vector3.right * 0.5f;
                        break;
                    case 1:
                        divisionPos = Vector3.forward + Vector3.right * 2;
                        break;
                    case 2:
                        divisionPos = Vector3.right + Vector3.back *0.5f;
                        break;
                    case 3:
                        divisionPos = Vector3.right + Vector3.back * 2;
                        break;
                    case 4:
                        divisionPos = Vector3.back + Vector3.left * 0.5f;
                        break;
                    case 5:
                        divisionPos = Vector3.back + Vector3.left * 2;
                        break;
                    case 6:
                        divisionPos = Vector3.forward + Vector3.left * 0.5f;
                        break;
                    case 7:
                        divisionPos = Vector3.forward + Vector3.left * 2;
                        break;
                }
            }
            divisionPos = divisionPos.normalized * radius * createPos;
            //if (createPos == 1.75f)
            //{
            //    Vector2 vec2;
            //    vec2.x = Mathf.Cos(45.0f*3.14f/180.0f);
            //    vec2.y = Mathf.Sin(45.0f * 3.14f / 180.0f);
            //    divisionPos.x += vec2.x *4;
            //    divisionPos.z += vec2.y *4;
            //}
            Vector3 randomPos = new Vector3(
               divisionPos.x + Random.Range(-spawnRange, spawnRange),
                0.1f,
                divisionPos.z + Random.Range(-spawnRange, spawnRange));

            for (int i = 0; i < transform.childCount; i++)
            {
                tr = transform.GetChild(i);
                if (tr.gameObject.name != "FallZone") continue;

                if (tr.gameObject.activeSelf) continue;

                tr.SetPositionAndRotation(randomPos, Quaternion.identity);
                tr.gameObject.SetActive(true);
                return tr.gameObject;
            }

            GameObject newZone = Instantiate(zone, randomPos, Quaternion.identity, transform);
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
            ExtinctionBeam extinctionBeam = newBeam.GetComponent<ExtinctionBeam>();
            extinctionBeam.SetTargetPos(obj);
            //extinctionBeam.shootingTime = Random.Range(0 ,maxShotTime);
        }
    }
}