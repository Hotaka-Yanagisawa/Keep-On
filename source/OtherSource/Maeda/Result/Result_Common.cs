using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Homare;

#region HeaderComent
//==================================================================================
// Result_Common
//	�X�e�[�g�ȊO�̏����̕���
// �쐬����	:2021/03/20
// �쐬��	:�O�c����
//---------- �X�V���� ----------
// 2021/03/20   �T���v������K�v�Ȃ����̂ɃR�����g�A�E�g    
//              �Q�l�ɂȂ邩������Ȃ��̂ŏ����Ȃ��ł���
// 2021/03/31   �C���X�y�N�^�[����ҏW�ł���悤�ɒǉ�
//==================================================================================
#endregion

namespace Maeda
{
    /// <summary>
    /// �X�e�[�g�ȊO�̏����@Start(),Update()�ⓖ���蔻��֐����͂���
    /// </summary>
    public partial class Result : MonoBehaviour
    {

        #region �C���X�y�N�^�[
        [Tooltip ("Manager")]
        [SerializeField] private ManageGame manage;

        [Tooltip("Panel")]
        [SerializeField] private GameObject panel;

        [Tooltip("UI")]
        [SerializeField] private GameObject baseUi;
        [SerializeField] private GameObject baseFrame;

        [Tooltip("Value")]
        [SerializeField] private GameObject value;

        [Tooltip("Rank")]
        [SerializeField] private GameObject rank;

        [Tooltip("SpriteNum")]
        [SerializeField] private GameObject num;

        [SerializeField] private GameObject[] score;

        [SerializeField] private GameObject[] result;

        [SerializeField] private TextMeshProUGUI text;

        [SerializeField] private Boss boss;

        [SerializeField] private GameObject button;
     


        #endregion

        #region �v���C�x�[�g�ϐ�
        private int nDefeatedEnemies;               // �G�̌��j��
        private int nTotalCombo;                    // ���v�R���{��
        private int nMaxCombo;                      // �ő�A���R���{��
        private int nComparisonMaxCombo;            // �ő�A���R���{���̔�r�p

        private CanvasGroup panelCanvas;                   // �p�l���̃��l�ύX�p
        private CanvasGroup uiCanvas;                      // UI�\���̃��l�ύX�p
        private CanvasGroup valueCanvas;                   // �X�R�A�̃��l�ύX�p
        private CanvasGroup rankCanvas;                    // �����N�̃��l�ύX�p
        private RectTransform baseRect;                    // �x�[�X�̃T�C�Y�ύX�p
        private RectTransform buttonRect;                  // �{�^���̃T�C�Y�ύX�p
        #endregion

        #region �p�u���b�N�ϐ�
        public PlayerControler input;

        #endregion

        #region �v���C�x�[�g�֐�

        // �A�N�V����
        public event Action ChangeFlags;

        /// <summary>
        /// �ŏ��ɌĂ΂��֐�
        /// </summary>
        private void Start()
        {
            panelCanvas = panel.GetComponent<CanvasGroup>();
            uiCanvas = baseFrame.GetComponent<CanvasGroup>();
            valueCanvas = value.GetComponent<CanvasGroup>();
            rankCanvas = rank.GetComponent<CanvasGroup>();

            baseRect = baseUi.GetComponent<RectTransform>();
            buttonRect = button.GetComponent<RectTransform>();
            input = new PlayerControler();
            input.Enable();
            OnStart();
        }


        /// <summary>
        /// ���t���[���Ă΂��֐�
        /// </summary>
        private void Update()
        {
            OnUpdate();
        }


        /// <summary>
        /// ���t���[�����ƌĂ΂��֐�
        /// </summary>
        private void FixedUpdate()
        {
            OnFixedUpdate();
        }

        #endregion

    
    }
}
