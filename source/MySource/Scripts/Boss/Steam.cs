using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Steam : MonoBehaviour
{
    [SerializeField] VisualEffect vEffect;
    public static Homare.Boss bossCS;
    Style.E_Style style = Style.E_Style.POWER;
    float curVal=0;
    float oldVal = 0;

    float t = 0;
    bool isUpdate =false;

    // Start is called before the first frame update
    void Start()
    {
        curVal = 0;
        oldVal = 0;
        t = 0;

        isUpdate = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isUpdate && bossCS.styleHolder.style.style == style)
        {
            t = 0;
            oldVal = curVal;
            style = bossCS.styleHolder.style.style;
        }
        else
        {
            style = bossCS.styleHolder.style.style;
            if (!isUpdate)
            {
                switch (style)
                {
                    case Style.E_Style.POWER:
                        curVal = 0;
                        //vEffect.SetFloat("Value", 0);
                        break;
                    case Style.E_Style.MOBILITY:
                        curVal = 0.66f;

                        //vEffect.SetFloat("Value", 0.66f);

                        break;
                    case Style.E_Style.REACH:
                        curVal = 0.33f;
                        //vEffect.SetFloat("Value", 0.33f);

                        break;
                }
            }
            isUpdate = true;
            t += Time.deltaTime/3;
            if (t >= 1)
            {
                isUpdate = false;

            }
            
            var val = Mathf.Lerp(oldVal, curVal, t);
            vEffect.SetFloat("Value", val);
        }
    }
}
