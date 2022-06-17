/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/18
// �쐬�� ���򔿋M
//
// Enemy_Common.cs
// �G�l�~�[�̐e�N���X
// ���ʂ��鏈���������Ɏ���
// �p�����[�^�A�v���p�e�B��Enemy.cs�ɕ������Ď���
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/03/18 �쐬�J�n
//            �G�̃X�e�[�g�ȊO�̏������s��
//     /03/31 �A�j���[�^�ǉ�
//
// 2021/04/22 �č\�zby�O��
//
//////////////////////////////////////////////////////////////////////////


using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System;

namespace Homare
{
    // �X�e�[�g�ȊO�̏�������
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
            //��񂾂��s���悤�ɂ��Ă݂�
            if (creater == null)
            {
                creater = transform.root.gameObject.GetComponent<EnemyCreater>();
                firstPos = transform.root.gameObject.transform.position;
            }
            //�������͎���������Ă���
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            //�r�w�C�r�A�c���[���쐬�A������
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
        /// ���X�|�[�����ɌĂяo��������
        /// </summary>
        protected virtual void Init()
        {
            Hp = enemyStatus.maxHp;
            isLanding = false;
            isWeapon = true;
            isEscape = false;
            alpha = 1;
            //�X�|�[�����󒆂Ȃ̂Ŏ���������Ă���
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(0).gameObject.GetComponent<SphereCollider>().enabled = true;
            transform.GetChild(1).gameObject.GetComponent<SphereCollider>().enabled = false;

            Physics.IgnoreCollision(myCollider, wallCollider, false);
        }

        // ���S
        protected virtual void Death()
        {
            Mizuno.SoundManager.Instance.PlayMenuSe("SE_Break");
            Ohira.CameraController.Instance.RemoveLockOn(gameObject);
            transform.GetChild(0).gameObject.GetComponent<SphereCollider>().enabled = false;
            transform.GetChild(1).gameObject.GetComponent<SphereCollider>().enabled = false;
        }
        /// <summary>
        /// ����������B
        /// �I�[�o�[���C�h����Enemy�̕���������āA�X�e�[�g�̕ύX���s��
        /// </summary>
        protected virtual void Escape()
        {
            //�����X�|�[�����Ē��n���true�ɖ߂�
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);

            Physics.IgnoreCollision(myCollider, wallCollider, true);
        }

        /// <summary>
        /// �v���C���[�̍U�����󂯂鏈��
        /// �_���[�WText�̏������s����
        /// �K�X�I�[�o�[���C�h���Ă�������
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
        /// �����D��ꂽ�ۂ̃t���O�̕ύX(����script�ŕ���D��ꂽ�������I�ɌĂ΂�܂�)
        /// </summary>
        public virtual void OnHitPlayerSteal()
        {
            isWeapon = false;
        }

        /// <summary>
        ///     �X�e�[�g�̕ύX
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
        ///   ����Enemy��script�ŃI�[�o�[���C�h���Ă�������
        ///   ���e�Ƃ��Ă͎��������n��ɂ������X�e�[�g�ɕύX�Ȃ�
        /// </summary>
        protected virtual void Landing()
        {
            gameObject.GetComponent<EffectOperate>().CreateEffect(
                2, transform.position, 0.1f);

            //PowerEnemy�̏ꍇ�ł�
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
        /// ���n�������̔���
        /// Landing�͊e�X�ŃI�[�o�[���C�h���Ă�������
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
        //�@�T�[�`����p�x�\��
        private void OnDrawGizmos()
        {
            Handles.color = new Color(0, 0, 0.7f, 0.05f);
            Handles.DrawSolidDisc(firstPos, Vector3.up, enemyStatus.moveRange);
        }
#endif
    }
}