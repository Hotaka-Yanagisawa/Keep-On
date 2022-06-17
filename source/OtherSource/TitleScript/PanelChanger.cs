#region �w�b�_�R�����g
// PanelChanger.cs
// �p�l���؂�ւ��N���X
//
//�@2021/04/09 : �O��D�l
#endregion


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Mikami
{
    public class PanelChanger : MonoBehaviour
    {
        [SerializeField] private Selectable openSelect;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Open()
        {
            canvasGroup.Show();
            openSelect?.Select();
        }

        public void Close()
        {
            canvasGroup.Hide();
        }
    }
}