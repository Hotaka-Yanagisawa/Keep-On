//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Homare
{

	public class DamageText : MonoBehaviour
	{
		RectTransform rectTransform = null;
		[SerializeField] TextMeshProUGUI textMesh;
		float drawCnt;                      //�`��I���J�E���g
		[SerializeField] float drawTime = 60;    //�`�悷�鎞��
		Vector3 pos;
		float alpha;
		[SerializeField] float fadeOutSpeed;
		[SerializeField] float moveSpeed;

		void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		private void Start()
		{
			alpha = 1;
			drawCnt = drawTime;
		}

		void LateUpdate()
		{
			pos += Vector3.up * moveSpeed * Time.deltaTime;
			rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);

			drawCnt -= Time.fixedDeltaTime;


			//�@�����Â����ɂ��Ă���
			alpha -= fadeOutSpeed * Time.deltaTime;
			//�@�e�L�X�g��color��ݒ�
			textMesh.color = new Color(1f, 0f, 0f, alpha);


			//�J�E���g�I���Ŕ�A�N�e�B�u
			if (drawCnt < 0)
			{
				textMesh.text = "";
				gameObject.SetActive(false);
			}
			//�g�O�ɂł����A�N�e�B�u
			if (rectTransform.position.x < 0)
			{
				textMesh.text = "";

				gameObject.SetActive(false);
			}
			//�g�O�ɂł����A�N�e�B�u
			if (rectTransform.position.x > 800.0f * 1.3f)
			{
				textMesh.text = "";

				gameObject.SetActive(false);
			}
		}


		private void OnEnable()
		{
			alpha = 1;
		}
		public void GetDrawPos(Vector3 drawPos, float damage)
		{
			//textMesh.enabled = true;
			pos = drawPos;
			drawCnt = drawTime;
			damage *= 10;
			textMesh.text = ((int)damage).ToString();

		}
	}
}