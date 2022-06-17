using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnemy : MonoBehaviour
{
    [SerializeField]GameObject enemy1;
    [SerializeField] GameObject enemy2;
    [SerializeField] GameObject enemy3;
    void Start()
    {
        enemy1.SetActive(true);
        enemy2.SetActive(true);
        enemy3.SetActive(true);
    }
}
