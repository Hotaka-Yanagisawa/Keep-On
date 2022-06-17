using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Homare
{
    public class Arc : MonoBehaviour
    {
        float t;
        [SerializeField] float Height;
        [Header("0より大きく")]
        [SerializeField] float tempo;
        Vector3 startPos;
        public Vector3 endPos { set; private get; }
        // Start is called before the first frame update
        void Start()
        {
            t = 0;
            startPos = transform.position;
        }

        private void OnEnable()
        {
            t = 0;
            startPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            t += Time.deltaTime * tempo;

            StartThrow(gameObject, Height, startPos, endPos, 0.1f);

            if (t >= 1)
            {
                t = 0;
                enabled = false;
            }
        }

        public void StartThrow(GameObject self, float height, Vector3 start, Vector3 end, float duration)
        {
            // 中点を求める
            Vector3 half = (start + end) * 0.5f;
            half.y += Vector3.up.y + height;
            self.transform.position = CalcLerpPoint(start, half, end, t);
            // 敵の角度の更新
            // Slerp:現在の向き、向きたい方向、向かせるスピード
            // LookRotation(向きたい方向):
            // transform.rotation =
            //     Quaternion.Slerp(transform.rotation,
            //     Quaternion.LookRotation(endPos),
            //     1);
            Vector3 target = start;
            target.y = transform.position.y;
            transform.forward = Vector3.Lerp(transform.forward, transform.position - target, t);
            // StartCoroutine(LerpThrow(target, start, half, end, duration));
        }

        IEnumerator LerpThrow(GameObject target, Vector3 start, Vector3 half, Vector3 end, float duration)
        {
            float startTime = Time.timeSinceLevelLoad;
            float rate = 0f;
            while (true)
            {
                if (rate >= 1.0f)
                    yield break;

                float diff = Time.timeSinceLevelLoad - startTime;
                rate = diff / (duration / 60f);
                target.transform.position = CalcLerpPoint(start, half, end, t);

                yield return null;
            }
        }

        Vector3 CalcLerpPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            var a = Vector3.Lerp(p0, p1, t);
            var b = Vector3.Lerp(p1, p2, t);
            return Vector3.Lerp(a, b, t);
        }


    }
}