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
// プレイヤーの死亡状態
// 作成日時	:2021/03/20
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/03/19   特に決まっていないので中身を空にしておく
//==================================================================================
#endregion



/// <summary>
/// 死亡時の処理
/// 何かあれば随時更新する
/// </summary>
namespace Maeda
{
    public partial class Result
    {
        int randScore = 0;                  // ランダムスコア用
        int tortalScore = 0;                // 合計スコア用
        int timeScore = 0;                  // タイムのスコア用
        int comboScore = 0;                 // コンボのスコア用
        int maxCombo = 0;                   // コンボの最大数
        int playMinTime = 0;                // 秒数の「分」の部分
        float deltaTime = 0.5f;             // ランダム生成のインターバル
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


                // プレイ時間を分と秒でそれぞれ取得
                owner.manage.GetPlayTime(owner.playMinTime, owner.playSecTime);
                //int num = 5;
                //var array = num.ConvertToDigitArray();

                // クリア時間によるスコアを取得
                owner.timeScore = owner.GetTimeScore(owner.manage.playTime);

                // コンボの最大数によるスコアを取得
                owner.comboScore = owner.GetComboScore(owner.manage.numCombo);

                // トータルスコア
                owner.tortalScore = owner.timeScore + owner.comboScore;
                if(!owner.manage.isPlayerDead)
                {
                    owner.tortalScore += 3000;
                }

                //if (owner.boss.IsDead)
                //{
                //    // ボスを倒したら+スコア
                //    owner.tortalScore += 1000;
                //}

                //Mizuno.SoundManager.Instance.StopBGM();
                //Mizuno.SoundManager.Instance.PlayBGM("BGM_Result");
            }

            public override void OnUpdate(Result owner)
            {
                // それぞれの取得してきた数値を配列にする
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
                    #region クリア時間とコンボ数の表示
               
                    // 表示のテキストをリセット
                    owner.result[0].GetComponent<TextMeshProUGUI>().text = "";
                    for(int i = 0;i < 2;i++)
                    {
                        // 時間の分単位の部分の表示
                        owner.result[0].GetComponent<TextMeshProUGUI>().text += "<sprite=" + timeMinArray[i] + ">";
                    }

                    owner.result[1].GetComponent<TextMeshProUGUI>().text = "";
                    for (int i = 0; i < 2; i++)
                    {
                        // 時間の秒単位の部分の表示
                        owner.result[1].GetComponent<TextMeshProUGUI>().text += "<sprite=" + timeSecArray[i] + ">";
                    }

                    // 表示のテキストをリセット
                    owner.result[2].GetComponent<TextMeshProUGUI>().text = "";

                    for (int i = 0; i < 3; i++)
                    {
                        // コンボの最大回数の表示
                        owner.result[2].GetComponent<TextMeshProUGUI>().text += "<sprite=" + comboArray[i] + ">";
                    }



                    //owner.result[0].GetComponent<TextMeshProUGUI>().text = array[i].ToString();

                    #endregion

                    owner.StartCoroutine(owner.valueCanvas.FadeInCoroutine(2f, owner.ChangeFlags));
                }


                if (owner.disp == E_DISP_STATE.RANK)
                {
                    #region 試作段階のスコア表示

                    // 時間経過による処理
                    owner.deltaTime -= Time.deltaTime;
                    owner.uiCnt -= Time.deltaTime;

                    // スコアを確定させるか
                    if (owner.uiCnt <= 0)
                    {
                        owner.isDipsScore = true;
                    }

                    #region ランダムで数値を設定
                    if (!owner.isDipsScore)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            owner.randScore = UnityEngine.Random.Range(0, 99999);

                            // int型の数値を配列にそれぞれ文字としてコンバートする
                            var array = owner.randScore.ConvertToDigitArray(5);

                            // 表示のテキストをリセット
                            owner.score[i].GetComponent<TextMeshProUGUI>().text = "";

                            for (int z = 0; z < 5; z++)
                            {
                                owner.score[i].GetComponent<TextMeshProUGUI>().text += "<sprite=" + array[z] + ">";
                            }
                        }
                    }
                    #endregion

                    #region 実際のスコアを表示
                    else
                    {
                        // 一回のみ表示させて更新はさせない
                        if (!owner.dipsOK)
                        {
                            #region タイムのスコア表示                         

                            // 数値を配列にいれる
                            var timescoreArray = owner.timeScore.ConvertToDigitArray(5);

                            // 表示のテキストをリセット
                            owner.score[0].GetComponent<TextMeshProUGUI>().text = "";

                            for (int i = 0; i < 5; i++)
                            {
                                owner.score[0].GetComponent<TextMeshProUGUI>().text += "<sprite=" + timescoreArray[i] + ">";
                            }
                            #endregion

                            #region コンボのスコア表示                          

                            // 数値を配列に入れる
                            var comboScoreArray = owner.comboScore.ConvertToDigitArray(5);

                           // 表示のテキストをリセット
                            owner.score[1].GetComponent<TextMeshProUGUI>().text = "";

                            for (int i = 0; i < 5; i++)
                            {
                                owner.score[1].GetComponent<TextMeshProUGUI>().text += "<sprite=" + comboScoreArray[i] + ">";
                            }

                            #endregion

                            #region スコアの合計表示
                           
                            // 数値を配列に入れる
                            var tortalScoreArray = owner.tortalScore.ConvertToDigitArray(5);

                            // 表示のテキストをリセット
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
        /// 暗転が終わったら次にUIを表示する
        /// </summary>
        private void DarkDispEnd()
        {
            //Mizuno.SoundManager.Instance.PlayBGMWithFade("BGM_Result",2f);
            ChangeFlags = UIDispEnd;
            disp = E_DISP_STATE.UI;
            Mizuno.SoundManager.Instance.PlayBGMWithFade("BGM_Result", 2f);
        }

        /// <summary>
        /// UI表示が終わったら結果表示を行う
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
        /// 最後にランクを表示させる
        /// </summary>
        private void ValueDisEnd()
        {
            ChangeFlags = RankDispEnd;

            //Mizuno.SoundManager.Instance.PlayMenuSe("SE_Rank");

            disp = E_DISP_STATE.RANK;
        }

        /// <summary>
        /// ランク表示を行ったらいったん終わり
        /// </summary>
        private void RankDispEnd()
        {
            int rankNum = GetRank(tortalScore);

            // int型の数値を配列にそれぞれ文字としてコンバートする
            var array = rankNum.ConvertToDigitArray();

            // 表示のテキストをリセット
            rank.GetComponent<TextMeshProUGUI>().text = "";

            rank.GetComponent<TextMeshProUGUI>().text += "<sprite=" + array[0] + ">";

            disp = E_DISP_STATE.MAX_DISP_STATE;
            okClick = true;
        }

        private int GetTimeScore(float time)
        {
            #region テーブルの設定
            // スコアの基準となる時間（秒単位）のテーブル
            float[] assessmentTime = new float[6] { 540f, 570f, 600f, 630f, 660f, 690f };

            // スコアのテーブル
            int[] assessmentScore = new int[6] { 10000, 9000, 8000, 7000, 6000, 5000 };

            int nCnt = 0;
            #endregion

            for (int i =0;i < 6;i++)
            {
                // 基準値より大きい→要素数を代入
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
            #region テーブルの設定
            // スコアの基準となる時間（秒単位）のテーブル
            float[] assessmentCombo = new float[4] {0,10,20,40};

            // スコアのテーブル
            int[] assessmentScore = new int[4] { 1000, 3000, 5000, 7000 };

            int nCnt = 0;
            #endregion

            for (int i = 0; i < 4; i++)
            {
                // 基準値より大きい→要素数を代入
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
            #region テーブルの設定
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
