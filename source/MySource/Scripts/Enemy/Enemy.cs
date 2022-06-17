/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/16
// �쐬�� ���򔿋M
//
// Enemy.cs
// �G�l�~�[�̐e�N���X
// �G�l�~�[�ɋ��ʂ���p�����[�^�A�v���p�e�B�������Ɏ���
// ���ʂ��鏈����Enemy_Common.cs�ɕ������Ď���
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/03/16 �쐬�J�n
// 2021/03/18 �\�[�X�R�[�h�𕪊����č쐬�A �X�e�[�g�p�^�[���ŏ�ԊǗ�
// 2021/03/18 �ҋ@�A�ړ���ԍ쐬
// 
// 2021/04/22 �č\�zby�O��
//
//////////////////////////////////////////////////////////////////////////


using UnityEngine;


namespace Homare
{
    public partial class Enemy
    {
        // �p����ŏ�������or���g��p�ӂ���K�v�̂���ϐ�
        protected EnemyStateBase currentState;
        protected BehaviorTreeController behavior;
        public virtual bool IsDead { set; get; }

        //public GameObject creater;
        [System.NonSerialized]public EnemyCreater creater;

        // ��Ԃ������i�\��)����̂ɕK�v�ȕϐ�
        public EnemyStatus enemyStatus;
        public static GameObject player { get; private set; }
        public Rigidbody rb { get; private set; }
        public Animator animator { get; private set; }
        public AudioSource audioSource { get; private set; }
        [System.NonSerialized] public float actionCnt;
        [System.NonSerialized] public Vector3 firstPos;
        [System.NonSerialized] public bool isEscape;      //�G�X�P�[�v�]�[���ɗ�����t���O���Ă�
        [System.NonSerialized] public bool isLanding;
        [System.NonSerialized] public Transform escapeTransform;
        public int attackTime;
        public float alpha = 1;    //�G�X�P�[�v���ɏ����鎞�̓��ߒl

        private float _Hp;
        public float Hp
        {
            get => _Hp;
            set
            {
                _Hp = Mathf.Min(value, enemyStatus.maxHp);
                if (_Hp <= 0)
                {
                    _Hp = 0;
                    Death();
                }
            }
        }

        private bool _isWeapon;
        public bool isWeapon
        {
            get => _isWeapon;
            set
            {
                _isWeapon = value;
                if (!_isWeapon)
                {
                    Escape();
                }
            }
        }
    }
}