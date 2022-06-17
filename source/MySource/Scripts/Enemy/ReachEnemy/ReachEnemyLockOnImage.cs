using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReachEnemyLockOnImage : MonoBehaviour
{
    private RectTransform rect;
    private Image image;
    private Transform targetTransform;
    
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetTransform) return;
        
        rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, targetTransform.transform.position);
    }

    public void LockOn(Transform target)
    {
        targetTransform = target;
        image.enabled = true;
        StartCoroutine(LockOnCoroutine());
    }

    private IEnumerator LockOnCoroutine()
    {
        const float DEFAULT_SIZE = 100f;
        rect.sizeDelta = new Vector2(DEFAULT_SIZE, DEFAULT_SIZE);

        for (int i = 0; i < 20; ++i)
        {
            rect.sizeDelta = new Vector2(DEFAULT_SIZE - i * 4, DEFAULT_SIZE - i * 4);
            yield return null;
        }

        for (int i = 0; i < 40; ++i)
        {
            yield return null;
        }

        targetTransform = null;
        image.enabled = false;
    }

}
