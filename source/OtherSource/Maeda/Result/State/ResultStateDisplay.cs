using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Mikami;
using Ohira.Auxiliary;
using TMPro;
#region HeaderComent
//==================================================================================
// PlayerStateDead
// �v���C���[�̎��S���
// �쐬����	:2021/03/20
// �쐬��	:�O�c����
//---------- �X�V���� ----------
// 2021/03/19   ���Ɍ��܂��Ă��Ȃ��̂Œ��g����ɂ��Ă���
//==================================================================================
#endregion



/// <summary>
/// ���S���̏���
/// ��������ΐ����X�V����
/// </summary>
namespace Maeda
{
    public partial class Result
    {
        int randScore = 0;                  // �����_���X�R�A�p
        int tortalScore = 0;                // ���v�X�R�A�p
        int timeScore = 0;                  // �^�C���̃X�R�A�p
        int comboScore = 0;                 // �R���{�̃X�R�A�p
        int maxCombo = 0;                   // �R���{�̍ő吔
        int playMinTime = 0;                // �b���́u���v�̕���
        float deltaTime = 0.5f;             // �����_�������̃C���^�[�o��
        float uiCnt = 3;
        float playSecTime = 0f;
        bool isDipsScore = false;
        bool dipsOK = false;
        bool once = false;
        Vector3 assenssmentVector3 = new Vector3(4,4,4);

        public class StateDisplay : ResultStateBase
        {

            public override void OnEnter(Result owner, ResultStateBase prevState)
            {
                owner.ChangeFlags = owner.DarkDispEnd;

                if(owner.disp == E_DISP_STATE.DARK)
                owner.StartCoroutine(owner.panelCanvas.FadeInCoroutine(2f,owner.ChangeFlags));


                // �v���C���Ԃ𕪂ƕb�ł��ꂼ��擾
                owner.manage.GetPlayTime(owner.playMinTime, owner.playSecTime);
                //int num = 5;
                //var array = num.ConvertToDigitArray();

                // �N���A���Ԃɂ��X�R�A���擾
                owner.timeScore = owner.GetTimeScore(owner.manage.playTime);

                // �R���{�̍ő吔�ɂ��X�R�A���擾
                owner.comboScore = owner.GetComboScore(owner.manage.numCombo);

                // �g�[�^���X�R�A
                owner.tortalScore = owner.timeScore + owner.comboScore;
                if(!owner.manage.isPlayerDead)
                {
                    owner.tortalScore += 3000;
                }

                //if (owner.boss.IsDead)
                //{
                //    // �{�X��|������+�X�R�A
                //    owner.tortalScore += 1000;
                //}

                //Mizuno.SoundManager.Instance.StopBGM();
                //Mizuno.SoundManager.Instance.PlayBGM("BGM_Result");
            }

            public override void OnUpdate(Result owner)
            {
                // ���ꂼ��̎擾���Ă������l��z��ɂ���
                var comboArray = owner.manage.numCombo.ConvertToDigitArray(3);
                var timeMinArray = owner.manage.minTime.ConvertToDigitArray(2);
                var timeSecArray = ((int)owner.manage.secTime).ConvertToDigitArray(2);
                

                if (owner.disp == E_DISP_STATE.UI)
                {
                    if(owner.rectSize.x >4.02)
                    owner.baseRect.localScale = new Vector3(owner.rectSize.x -= Time.deltaTime, owner.rectSize.y -= Time.deltaTime, owner.rectSize.z -= Time.deltaTime);
                    else
                    {
                        owner.rectSize = new Vector3(4.01f, 4.01f, 4f);
                        owner.StartCoroutine(owner.uiCanvas.FadeInCoroutine(1f, owner.ChangeFlags));
                        
                    }
                   
                  
                }
                    

                if (owner.disp == E_DISP_STATE.VALUE)
                {
                    #region �N���A���ԂƃR���{���̕\��
               
                    // �\���̃e�L�X�g�����Z�b�g
                    owner.result[0].GetComponent<TextMeshProUGUI>().text = "";
                    for(int i = 0;i < 2;i++)
                    {
                        // ���Ԃ̕��P�ʂ̕����̕\��
                        owner.result[0].GetComponent<TextMeshProUGUI>().text += "<sprite=" + timeMinArray[i] + ">";
                    }

                    owner.result[1].GetComponent<TextMeshProUGUI>().text = "";
                    for (int i = 0; i < 2; i++)
                    {
                        // ���Ԃ̕b�P�ʂ̕����̕\��
                        owner.result[1].GetComponent<TextMeshProUGUI>().text += "<sprite=" + timeSecArray[i] + ">";
                    }

                    // �\���̃e�L�X�g�����Z�b�g
                    owner.result[2].GetComponent<TextMeshProUGUI>().text = "";

                    for (int i = 0; i < 3; i++)
                    {
                        // �R���{�̍ő�񐔂̕\��
                        owner.result[2].GetComponent<TextMeshProUGUI>().text += "<sprite=" + comboArray[i] + ">";
                    }



                    //owner.result[0].GetComponent<TextMeshProUGUI>().text = array[i].ToString();

                    #endregion

                    owner.StartCoroutine(owner.valueCanvas.FadeInCoroutine(2f, owner.ChangeFlags));
                }


                if (owner.disp == E_DISP_STATE.RANK)
                {
                    #region ����i�K�̃X�R�A�\��

                    // ���Ԍo�߂ɂ�鏈��
                    owner.deltaTime -= Time.deltaTime;
                    owner.uiCnt -= Time.deltaTime;

                    // �X�R�A���m�肳���邩
                    if (owner.uiCnt <= 0)
                    {
                        owner.isDipsScore = true;
                    }

                    #region �����_���Ő��l��ݒ�
                    if (!owner.isDipsScore)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            owner.randScore = UnityEngine.Random.Range(0, 99999);

                            // int�^�̐��l��z��ɂ��ꂼ�ꕶ���Ƃ��ăR���o�[�g����
                            var array = owner.randScore.ConvertToDigitArray(5);

                            // �\���̃e�L�X�g�����Z�b�g
                            owner.score[i].GetComponent<TextMeshProUGUI>().text = "";

                            for (int z = 0; z < 5; z++)
                            {
                                owner.score[i].GetComponent<TextMeshProUGUI>().text += "<sprite=" + array[z] + ">";
                            }
                        }
                    }
                    #endregion

                    #region ���ۂ̃X�R�A��\��
                    else
                    {
                        // ���̂ݕ\�������čX�V�͂����Ȃ�
                        if (!owner.dipsOK)
                        {
                            #region �^�C���̃X�R�A�\��                         

                            // ���l��z��ɂ����
                            var timescoreArray = owner.timeScore.ConvertToDigitArray(5);

                            // �\���̃e�L�X�g�����Z�b�g
                            owner.score[0].GetComponent<TextMeshProUGUI>().text = "";

                            for (int i = 0; i < 5; i++)
                            {
                                owner.score[0].GetComponent<TextMeshProUGUI>().text += "<sprite=" + timescoreArray[i] + ">";
                            }
                            #endregion

                            #region �R���{�̃X�R�A�\��                          

                            // ���l��z��ɓ����
                            var comboScoreArray = owner.comboScore.ConvertToDigitArray(5);

                           // �\���̃e�L�X�g�����Z�b�g
                            owner.score[1].GetComponent<TextMeshProUGUI>().text = "";

                            for (int i = 0; i < 5; i++)
                            {
                                owner.score[1].GetComponent<TextMeshProUGUI>().text += "<sprite=" + comboScoreArray[i] + ">";
                            }

                            #endregion

                            #region �X�R�A�̍��v�\��
                           
                            // ���l��z��ɓ����
                            var tortalScoreArray = owner.tortalScore.ConvertToDigitArray(5);

                            // �\���̃e�L�X�g�����Z�b�g
                            owner.score[2].GetComponent<TextMeshProUGUI>().text = "";

                            for (int i = 0; i < 5; i++)
                            {
                                owner.score[2].GetComponent<TextMeshProUGUI>().text += "<sprite=" + tortalScoreArray[i] + ">";
                            }

                            #endregion
                            Mizuno.SoundManager.Instance.PlayMenuSe("SE_Rank");

                            owner.dipsOK = true;
                        }
                        else
                        {
                            // owner.StartCoroutine(owner.valueCanvas.FadeInCoroutine(2f, owner.ChangeFlags));
                        }
                    }
                    #endregion


                    #endregion

                    owner.StartCoroutine(owner.rankCanvas.FadeInCoroutine(3f, owner.ChangeFlags));
                }

            }

            public override void OnExit(Result owner, ResultStateBase nextState)
            {
                owner.disp = E_DISP_STATE.DARK;
            }
        }
        /// <summary>
        /// �Ó]���I������玟��UI��\������
        /// </summary>
        private void DarkDispEnd()
        {
            //Mizuno.SoundManager.Instance.PlayBGMWithFade("BGM_Result",2f);
            ChangeFlags = UIDispEnd;
            disp = E_DISP_STATE.UI;
            Mizuno.SoundManager.Instance.PlayBGMWithFade("BGM_Result", 2f);
        }

        /// <summary>
        /// UI�\�����I������猋�ʕ\�����s��
        /// </summary>
        private void UIDispEnd()
        {
           
            ChangeFlags = ValueDisEnd;
            disp = E_DISP_STATE.VALUE;
            if(!once)
            {
                //Mizuno.SoundManager.Instance.StopBGM();
               
                once = true;
            }
           

        }

        /// <summary>
        /// �Ō�Ƀ����N��\��������
        /// </summary>
        private void ValueDisEnd()
        {
            ChangeFlags = RankDispEnd;

            //Mizuno.SoundManager.Instance.PlayMenuSe("SE_Rank");

            disp = E_DISP_STATE.RANK;
        }

        /// <summary>
        /// �����N�\�����s�����炢������I���
        /// </summary>
        private void RankDispEnd()
        {
            int rankNum = GetRank(tortalScore);

            // int�^�̐��l��z��ɂ��ꂼ�ꕶ���Ƃ��ăR���o�[�g����
            var array = rankNum.ConvertToDigitArray();

            // �\���̃e�L�X�g�����Z�b�g
            rank.GetComponent<TextMeshProUGUI>().text = "";

            rank.GetComponent<TextMeshProUGUI>().text += "<sprite=" + array[0] + ">";

            disp = E_DISP_STATE.MAX_DISP_STATE;
            okClick = true;
        }

        private int GetTimeScore(float time)
        {
            #region �e�[�u���̐ݒ�
            // �X�R�A�̊�ƂȂ鎞�ԁi�b�P�ʁj�̃e�[�u��
            float[] assessmentTime = new float[6] { 540f, 570f, 600f, 630f, 660f, 690f };

            // �X�R�A�̃e�[�u��
            int[] assessmentScore = new int[6] { 10000, 9000, 8000, 7000, 6000, 5000 };

            int nCnt = 0;
            #endregion

            for (int i =0;i < 6;i++)
            {
                // ��l���傫�����v�f������
                if(time >=assessmentTime[i])
                {
                    nCnt = i;
                }
            }

            if(manage.isPlayerDead)
            {
                return 1000;
            }

            return assessmentScore[nCnt];
        }
        
        private int GetComboScore(int combo)
        {
            #region �e�[�u���̐ݒ�
            // �X�R�A�̊�ƂȂ鎞�ԁi�b�P�ʁj�̃e�[�u��
            float[] assessmentCombo = new float[4] {0,10,20,40};

            // �X�R�A�̃e�[�u��
            int[] assessmentScore = new int[4] { 1000, 3000, 5000, 7000 };

            int nCnt = 0;
            #endregion

            for (int i = 0; i < 4; i++)
            {
                // ��l���傫�����v�f������
                if (combo >= assessmentCombo[i])
                {
                    nCnt = i;
                }
            }

            return assessmentScore[nCnt];
        }

        private int GetDefeatEnemy()
        {
            return 0;
        }

        private int GetRank(int score)
        {
            #region �e�[�u���̐ݒ�
            int[] assessmentScore = new int[5] { 0, 5000, 10000, 14000, 17000 };

            int nCnt = 0;
            #endregion 

            for (int i =0;i <5;i++ )
            {
                if(score >= assessmentScore[i])
                {
                    nCnt = i;
                }
            }

            return nCnt;
        }
    }
}
