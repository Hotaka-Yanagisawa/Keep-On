/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/03/18
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/03/18 �쐬�J�n
//            �X�e�[�g�̒��ۃN���X
// 
//
//
//////////////////////////////////////////////////////////////////////////


namespace Homare
{
    /// <summary>
    /// State�̒��ۃN���X
    /// </summary>
    public abstract class EnemyStateBase
    {
        /// <summary>
        /// �X�e�[�g���J�n�������ɌĂ΂��
        /// </summary>
        public virtual void OnEnter(Enemy owner, EnemyStateBase prevState) { }
        /// <summary>
        /// ���t���[���Ă΂��
        /// </summary>
        public virtual void OnUpdate(Enemy owner) { }
        /// <summary>
        /// ���t���[�����Ƃ�
        /// </summary>
        public virtual void OnFixedUpdate(Enemy owner) { }
        /// <summary>
        /// �X�e�[�g���I���������ɌĂ΂��
        /// </summary>
        public virtual void OnExit(Enemy owner, EnemyStateBase nextState) { }
    }
}