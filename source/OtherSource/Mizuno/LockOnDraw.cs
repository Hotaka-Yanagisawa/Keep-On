using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOnDraw : MonoBehaviour
{
    private RectTransform rect;
    private Image image;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        var cameraController = Ohira.CameraController.Instance;
        if (cameraController.IsLocking())
        {
            image.enabled = true;
            rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, cameraController.lockOnTransform.position);

            var mat = GetComponent<Image>().material;
            //var enem = cameraController.secondaryTarget.GetComponent<Homare.Enemy>();
            var enem = cameraController.enemyInfo;
            if (enem.kind != EnemyInfo.Kind.HEAL)
            {
                mat.SetFloat("_Value", enem.currentHP / enem.maxHP);

                if(cameraController.enableSteal)
                    mat.SetColor("_Color", Color.red);
                else
                    mat.SetColor("_Color", Color.blue);
            }
            else
            {
                // 回復エネミー
                mat.SetFloat("_Value", 1f);
                mat.SetColor("_Color", Color.black);
            }
        }
        else
            image.enabled = false;
    }
}
