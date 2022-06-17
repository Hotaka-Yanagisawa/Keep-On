using UnityEngine;
using UnityEngine.UI;
namespace Maeda
{

    public class TitleButton : MonoBehaviour
    {

        public int arrayNum;
        [SerializeField] private GameObject[] obj = new GameObject[3];
        private RectTransform[] image = new RectTransform[3];
        private bool isBig;
        private bool isUp;
        private float a;

        private void Start()
        {
            //obj = new GameObject[arrayNum];
            //image = new RectTransform[arrayNum];

            for(int i = 0;i < arrayNum;i++)
            image[i] = obj[i].GetComponent<RectTransform>();


            isBig = true;
            isUp = true;

            a = 0;
        }


        private void Update()
        {
            // Title歯車回転
            a -= 0.5f;
            image[1].rotation = Quaternion.Euler(0f, 0f, a);



            if (isBig)
                image[0].localScale += image[0].localScale * (Time.deltaTime * 0.4f);
            else
                image[0].localScale -= image[0].localScale * (Time.deltaTime * 0.2f);

            // チェック
            if (image[0].localScale.x > 3)
                isBig = false;
            else if (image[0].localScale.x < 2)
                isBig = true;

        }
    }
}

