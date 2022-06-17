/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/18
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
//
/////////////////////////////////////////////////////////////////////////
// �T�v
// 
// 
//
// �X�V����
//
// 2021/03/18 �쐬�J�n
//            �d�l���܂��s���̂��߂Ƃ肠���������_������
//
// 2020/05/09 �������A�����p�x���O������ύX�ł���悤�ɍ쐬
//
//////////////////////////////////////////////////////////////////////////


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Homare
{
    [System.Serializable]
    public class EnemyCreater : MonoBehaviour
    {
        private const int MAX_POOL = 20;
        
        [Header("��������Ώۃ��X�g")]
        [SerializeField] private List<GameObject> spawnEnemy;
        [Header("�ő�o����"),Range(0,MAX_POOL)]
        [SerializeField] private int maxSpawnNum;
        [Header("�����Ԋu"),Range(0f,1000f)]
        [SerializeField] private float generateInterval = 2;
        [Header("�����͈�")]
        [SerializeField] float spawnRange;
        
        private List<List<GameObject>> enemyInstancePool = new List<List<GameObject>>();
        private float generateCnt;
        private Vector3[] spawnPosArray;


        public int MaxSpawnNum
        {
            set => maxSpawnNum = Mathf.Min(value, MAX_POOL * spawnEnemy.Count);
            get => maxSpawnNum;
        }

        public float GenerateInteval
        {
            set => generateInterval = Mathf.Max(0f, value);
            get => generateInterval;
        }


        private void Awake()
        {
            spawnPosArray = new Vector3[9];
            Vector3 basePos = transform.position;

            for (int i = 0; i < 9; ++i)
                spawnPosArray[i] = basePos;

            spawnPosArray[1] += new Vector3( spawnRange, 0f,  0f);
            spawnPosArray[2] += new Vector3(-spawnRange, 0f,  0f);
            spawnPosArray[3] += new Vector3( 0f, 0f,  spawnRange);
            spawnPosArray[4] += new Vector3( 0f, 0f, -spawnRange);
            spawnPosArray[5] += new Vector3( spawnRange * 0.5f, 0f,  spawnRange * 0.5f);
            spawnPosArray[6] += new Vector3( spawnRange * 0.5f, 0f, -spawnRange * 0.5f);
            spawnPosArray[7] += new Vector3(-spawnRange * 0.5f, 0f,  spawnRange * 0.5f);
            spawnPosArray[8] += new Vector3( spawnRange * 0.5f, 0f, -spawnRange * 0.5f);

            // �C���X�^���X�����O����
            enemyInstancePool = new List<List<GameObject>>();
            for (int j = 0; j < spawnEnemy.Count; ++j)
            {
                List<GameObject> workList = new List<GameObject>(MAX_POOL);
                for (int i = 0; i < MAX_POOL; ++i)
                {
                    GameObject workObj = Instantiate(spawnEnemy[j], basePos, Quaternion.identity, transform);
                    workObj.SetActive(false);
                    workList.Add(workObj);
                }
                enemyInstancePool.Add(workList);
            }
            
            generateCnt = generateInterval;
        }

        private void FixedUpdate()
        {
            // �L���ȃI�u�W�F�N�g���J�E���g���A�ő吶���������Ȃ琶�������
            int activeEnemyNum = 0;
            enemyInstancePool.ForEach(pool => pool.ForEach(enemy => { if (enemy.activeSelf) ++activeEnemyNum; }));
            if (activeEnemyNum >= maxSpawnNum) return;

            //��莞�Ԍo�߂��邽�тɓG�𐶐�
            generateCnt -= 1.0f / 60.0f;
            if (generateCnt <= 0)
            {
                EnemySpawn();
                generateCnt = generateInterval;
            }
        }

        private void EnemySpawn()
        {
            // �ݒ肳�ꂽ�����Ώۂ��烉���_���ɐ���
            int select = Random.Range(0, spawnEnemy.Count);
            int spawnPosIndex = Random.Range(0, 9);

            // �����ȃI�u�W�F�N�g����I�����ėL����
            for (int i = 0; i < spawnEnemy.Count; ++i)
            {
                foreach (GameObject enemy in enemyInstancePool[select])
                {
                    if (enemy.activeSelf) continue;
                    enemy.transform.SetPositionAndRotation(spawnPosArray[spawnPosIndex], Quaternion.identity);
                    enemy.SetActive(true);
                    return;
                }
                select = ++select % spawnEnemy.Count;
            }
        }
    }
}