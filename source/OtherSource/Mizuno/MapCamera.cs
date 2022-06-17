using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    [SerializeField] GameObject Target;
    [SerializeField] bool Rotate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Target.transform.position.x;
        pos.z = Target.transform.position.z;
        transform.position = pos;

        if(Rotate)
        {
            Vector3 rotate = new Vector3(90f, 0f, 0f);

            rotate.z = -Target.transform.rotation.eulerAngles.y;

            transform.rotation = Quaternion.Euler(rotate);
        }

    }
}
