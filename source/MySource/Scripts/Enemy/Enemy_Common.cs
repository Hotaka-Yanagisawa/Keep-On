/////////////////////////////////////////////////////////////////////////
// 作成日 2021/03/18
// 作成者 柳沢帆貴
//
// Enemy_Common.cs
// エネミーの親クラス
// 共通する処理をここに実装
// パラメータ、プロパティはEnemy.csに分割して実装
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/03/18 作成開始
//            敵のステート以外の処理を行う
//     /03/31 アニメータ追加
//
// 2021/04/22 再構築by三上
//
//////////////////////////////////////////////////////////////////////////


using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System;

namespace Homare
{
    // ステート以外の処理部分
    public partial class Enemy : MonoBehaviour
    {
        protected GameObject canvas;
        DamageText damageText;
        static Collider wallCollider = null;
        [SerializeField] Collider myCollider;

        protected virtual void Awake()
        {
            player = GameObject.Find("Player");
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            canvas = GameObject.Find("DamageUICanvas");
            audioSource = GetComponent<AudioSource>();
            if (wallCollider == null)
            {
                wallCollider = GameObject.Find("Wall").GetComponent<MeshCollider>();
            }
        }

        protected virtual void Start()
        {
            Hp = enemyStatus.maxHp;
            isWeapon = true;
            isEscape = false;
            alpha = 1;
            //一回だけ行うようにしてみる
            if (creater == null)
            {
                creater = transform.root.gameObject.GetComponent<EnemyCreater>();
                firstPos = transform.root.gameObject.transform.position;
            }
            //落下中は視野を消しておく
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            //ビヘイビアツリーを作成、初期化
            behavior?.OnStart();
            currentState?.OnEnter(this, null);
        }

        private void Update()
        {
            //behavior.OnRunning();
            currentState?.OnUpdate(this);
        }
        private void FixedUpdate()
        {
            behavior?.OnRunning();
            currentState?.OnFixedUpdate(this);
        }

        protected virtual void OnEnable()
        {
            Init();
        }

        /// <summary>
        /// リスポーン時に呼び出す初期化
        /// </summary>
        protected virtual void Init()
        {
            Hp = enemyStatus.maxHp;
            isLanding = false;
            isWeapon = true;
            isEscape = false;
            alpha = 1;
            //スポーン時空中なので視野を消しておく
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(0).gameObject.GetComponent<SphereCollider>().enabled = true;
            transform.GetChild(1).gameObject.GetComponent<SphereCollider>().enabled = false;

            Physics.IgnoreCollision(myCollider, wallCollider, false);
        }

        // 死亡
        protected virtual void Death()
        {
            Mizuno.SoundManager.Instance.PlayMenuSe("SE_Break");
            Ohira.CameraController.Instance.RemoveLockOn(gameObject);
            transform.GetChild(0).gameObject.GetComponent<SphereCollider>().enabled = false;
            transform.GetChild(1).gameObject.GetComponent<SphereCollider>().enabled = false;
        }
        /// <summary>
        /// 視野を消す。
        /// オーバーライドしてEnemyの武器を消して、ステートの変更を行う
        /// </summary>
        protected virtual void Escape()
        {
            //次リスポーンして着地後にtrueに戻す
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);

            Physics.IgnoreCollision(myCollider, wallCollider, true);
        }

        /// <summary>
        /// プレイヤーの攻撃を受ける処理
        /// ダメージTextの処理も行われる
        /// 適宜オーバーライドしてください
        /// </summary>
        /// <param name="damage"></param>
        public virtual void OnHitPlayerAttack(float damage)
        {
            Hp -= damage;

            foreach (Transform t in canvas.transform)
            {
                if (!t.gameObject.activeSelf)
                {
                    damageText = t.GetComponent<DamageText>();
                    t.gameObject.SetActive(true);
                    break;
                }
            }
            damageText?.GetDrawPos(transform.position + new Vector3(0, 1.5f, 0), damage);
        }
        /// <summary>
        /// 武器を奪われた際のフラグの変更(他のscriptで武器奪われた時自動的に呼ばれます)
        /// </summary>
        public virtual void OnHitPlayerSteal()
        {
            isWeapon = false;
        }

        /// <summary>
        ///     ステートの変更
        /// </summary>
        /// <param name="nextState"></param>
        public void ChangeState(EnemyStateBase nextState)
        {
            if (nextState != currentState)
            {
                currentState.OnExit(this, nextState);
                nextState.OnEnter(this, currentState);
                currentState = nextState;
            }
        }
        /// <summary>
        ///   ○○Enemyのscriptでオーバーライドしてください
        ///   内容としては自分が着地後にしたいステートに変更など
        /// </summary>
        protected virtual void Landing()
        {
            gameObject.GetComponent<EffectOperate>().CreateEffect(
                2, transform.position, 0.1f);

            //PowerEnemyの場合です
            //base.Landing();
            //transform.GetChild(0).gameObject.SetActive(true);
            //transform.GetChild(1).gameObject.SetActive(true);
            //ChangeState(stateWaiting);
        }

        protected virtual void PlaySE(string se_name)
        {
            Mizuno.SoundManager.Instance.PlayMenuSe(se_name);
        }

        /// <summary>
        /// 着地した時の判定
        /// Landingは各々でオーバーライドしてください
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            if (isLanding) return;
            if (collision.gameObject.CompareTag("Stage"))
            {
                isLanding = true;
                Landing();
            }
        }
      



#if UNITY_EDITOR
        //　サーチする角度表示
        private void OnDrawGizmos()
        {
            Handles.color = new Color(0, 0, 0.7f, 0.05f);
            Handles.DrawSolidDisc(firstPos, Vector3.up, enemyStatus.moveRange);
        }
#endif
    }
}