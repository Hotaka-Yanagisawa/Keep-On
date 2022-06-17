using UnityEngine.InputSystem;
using UnityEngine;

#region HeaderComent
//==================================================================================
// Player
//	プレイヤークラスの本体　partialでアクション別など複数のスクリプトに分けている　
//  おおもとになるスクリプト
// 作成日時	:2021/03/19
// 作成者	:前田理玖
//---------- 更新履歴 ----------
// 2021/03/19	参考動画のサンプルを基にプレイヤーのステートマシン制作開始         参考動画(https://www.youtube.com/watch?v=PbtJt5tnnI8&t=1181s)
// 2021/03/19   InputSystemの導入                                                  参考動画(https://www.youtube.com/watch?v=pRSZr6CFcpQ&t=1080s)
//      :               :                                                          参考資料(http://tsubakit1.hateblo.jp/entry/2019/10/13/143530)  未だ勉強中・・・
// 2021/03/20	プレイヤーの基本的な動き(移動、ジャンプ、回避)を実装 ← 遷移するときスムーズに遷移しない
// 2021/03/20   状態遷移するときにスムーズに遷移するように修正
// 2021/03/21   コメントアウトをしていなかったのでしました。日付等は若干適当ゆるして。
// 2021/04/01   アニメーションに伴い攻撃のステートを追加
//==================================================================================
#endregion

namespace Maeda
{
    public partial class Player
    {
        #region ステートのインスタンス
        private static readonly StateStanding   stateStanding   = new StateStanding();      // 待機
        private static readonly StateMoving     stateMoving     = new StateMoving();        // 移動
        private static readonly StateJumping    stateJumping    = new StateJumping();       // ジャンプ
        private static readonly StateFalldown   stateFalldown   = new StateFalldown();      // 落下
        private static readonly StateLanding    stateLanding    = new StateLanding();       // 着地
        private static readonly StateDead       stateDead       = new StateDead();          // 死亡    
        private static readonly StateDodging    stateDodging    = new StateDodging();       // ステップ 
        private static readonly StateStun       stateStun       = new StateStun();          // 硬直
        private static readonly StateAttack     stateAttack     = new StateAttack();        // 攻撃
        private static readonly StateKnockBack  stateKnockBack  = new StateKnockBack();     // ノックバック
  
   
        #endregion

        #region インプットアクション
        private InputAction move, jump, dodge,attack;
        #endregion

        #region プライベート変数       
        private Vector3 oldPos;
        private Vector3 cameraForward;
        private Vector3 moveForward;
        private int frameCnt;
        private int stunCnt;
        private bool okStep = true;
        private float stepIntervalTime;
        private float invincTime;
        private float speed;
        private bool once = false;
        private bool resultOff = false;

        #endregion

        #region パブリック変数
        public bool isDead = false;
        public bool isStep = false;
        #endregion

        #region プライベート関数

        /// <summary>
        /// InputSystemで入力を受け取る
        /// </summary>
        private Vector2 Velocity { get; set; }


        /// <summary>
        /// 現在のステート
        /// </summary>
        private PlayerStateBase currentState = stateStanding;           // 立ち状態から始まる


        /// <summary>
        /// Start()から呼ばれる
        /// </summary>
        private void OnStart()
        {
            input.Enable();
           // 
            baseParam.currentHavingWeapon = 0.0f;

            // HPとスタミナの初期化
            baseParam.currentHp = baseParam.maxHp;
            baseParam.currentSp = baseParam.maxSp;

            okStep = true;
            once = false;
            resultOff = false;

            currentState.OnEnter(this, null);
        }


        /// <summary>
        /// Update()から呼ばれる
        /// </summary>
        private void OnUpdate()
        {
            #region プレイヤーの前方向の決定処理
            //Velocity = input.Player.Move.ReadValue<Vector2>();
            //Debug.Log(input.Player.Move.ReadValue<Vector2>());
            if(!manage.isResult)
            {
                cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
                moveForward = cameraForward * Velocity.y + Camera.main.transform.right * Velocity.x;
            }
          
            #endregion

            #region スタミナ関係の処理
            // スタミナ回復
            if (!animator.GetBool("step"))
            {
                if (baseParam.currentSp <= baseParam.maxSp)
                    baseParam.currentSp += 0.35f;
            }
           
            // スタミナが最大値以上なら最大値にする
            if (baseParam.currentSp >= baseParam.maxSp)
                baseParam.currentSp = baseParam.maxSp;
            #endregion

            #region コンボ関係の処理
            // スタイルがノーマル以外かつ時間が0になった時
            if (baseParam.style.style != Style.E_Style.NORMAL && baseParam.currentHavingWeapon <= 0.0f)
            {
                baseParam.currentHavingWeapon = 0.0f;
                baseParam.satellite.SendMessage("ResetSatelliteStyle");
            }

            // 素体以外の時に武器の継続時間を減らす
            if (baseParam.style.style != Style.E_Style.NORMAL)
            {
                if(isDecraseComboGauge)
                baseParam.currentHavingWeapon -= Time.deltaTime;
            }
            #endregion

            #region ステップ間のインターバル処理
            if(stepIntervalTime > 0)
            {
                stepIntervalTime -= Time.deltaTime;
            }
            else if(stepIntervalTime <= 0)
            {
                okStep = true;
            }

            #endregion

            #region 入力の優先度
            // X と Y の入力値を比較して優先度を決める
            if (Mathf.Abs(Velocity.x) < Mathf.Abs(Velocity.y))
            {
                animator.SetFloat("speed", Mathf.Abs(Velocity.y));
            }
            else
            {
                animator.SetFloat("speed", Mathf.Abs(Velocity.x));
            }
            #endregion

            #region プレイヤーのステート処理
            currentState.OnUpdate(this);
            #endregion

            #region カメラ関係の処理
            // カメラの回転
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
                // Debug.Log(moveForward);
            }
            #endregion

            #region 死亡関係の処理

            // HPが0以下になったら
            if (baseParam.currentHp <=0)
            {
                OnDeath();
            }

            // 自殺ボタン
            if (input.Player.Dead.triggered)
            {
                baseParam.currentHp = 0;
                OnDeath();
            }

            #endregion

            #region 復活の呪文
            if(input.Player.Resporn.triggered)
            {
                ReBorn();
            }
            #endregion

            #region UI関係の処理
            // UIと連携
            hpmaterial.SetFloat("_Value", baseParam.currentHp / baseParam.maxHp);                           // HPのUI
            spmaterial.SetFloat("_Value", baseParam.currentSp / baseParam.maxSp);                           // スタミナのUI
            combomaterial.SetFloat("_Value", (baseParam.currentHavingWeapon) / (baseParam.maxHavingWeapon));    // 武器の継続時間のUI
            #endregion

            #region リザルト画面時の処理
            if (manage.isResult)
            {
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyWeapon"), true);
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
                if (!once)
                {
                    once = true;
                    input.Disable();
                   
                    if (resultOff)
                    {
                        GetComponent<Rigidbody>().useGravity = false;
                        Velocity = Vector3.zero;
                        GetComponent<CapsuleCollider>().enabled = false;
                    }
                }
            }
            #endregion

        }

        /// <summary>
        /// FixedUpdate()から呼ばれる
        /// </summary>
        private void OnFixedUpdate()
        {
            #region 無敵時間の処理
            
            if (invincTime > 0)
            {
               // Debug.Log("前田" + invincTime);
                invincTime -= Time.fixedDeltaTime;

                if (invincTime <= 0f)
                {
                    Debug.Log("前田オン");
                    Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyWeapon"), false);
                    invincTime = 0;
                }
            }
            #endregion
            currentState.OnFixUpdate(this);
        }

        /// <summary>
        /// ステートの変更
        /// </summary>
        private void ChangeState(PlayerStateBase nextState)
        {
            if (currentState != nextState)
            {
                currentState.OnExit(this, nextState);
                nextState.OnEnter(this, currentState);
                currentState = nextState;
            }
        }
        

        /// <summary>
        /// 死亡したときに呼ばれる関数
        /// </summary>
        private void OnDeath()
        {
            isDead = true;
            ChangeState(stateDead);                     // 死亡状態へ
        }


		/// <summary>
		/// 復活
		/// </summary>
		public void ReBorn()
		{
			isDead = false;

			baseParam.currentHp = baseParam.maxHp;
			currentState = stateStanding;
			sotai.SetActive(true);
		}


        /// <summary>
        /// 地面との衝突
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            if(animator.GetBool("jump"))
            {
                if (collision.gameObject.tag == "Stage")
                {
                    //Debug.Log("前田地面についた");
                   // animator.SetBool("landing", true);
                   //if(manage.isResult)
                   // {
                   //     resultOff = true;
                   // }

                    animator.SetBool("jump", false);
                    ChangeState(stateLanding);
                }
            }                
        }

        private void OnCollisionStay(Collision collision)
        {
            if (manage.isResult)
            {
                if (collision.gameObject.tag == "Stage")
                {
                    resultOff = true;
                }
            }
        }

        /// <summary>
        /// 無敵時間の関数
        /// 引数は秒数単位 float   
        /// </summary>
        /// <param name="time"></param>
        private void SetInvinc(float time)
        {
            invincTime = time;
            Debug.Log("前田　オフ");
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyWeapon"), true);
        }
        
        private void SetLandingOff()
        {
            animator.SetBool("landing", false);
        }

        public void PlaySE(string se_name)
        {
            var soundManager = Mizuno.SoundManager.Instance;
            
            if(!isDead)
            {
                if (se_name == "SE_PlayerFoot")
                {
                    se_name += Random.Range(1, 9).ToString();
                }
            }
           

            soundManager.PlayMenuSe(se_name);
        }


		public void InputDisable()
		{
			input.Disable();
		}


		public void InputEnable()
		{
			input.Enable();
		}

        #endregion
    }
}