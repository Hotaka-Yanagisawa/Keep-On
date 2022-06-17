/////////////////////////////////////////////////////////////////////////
// �쐬�� 2021/05/19
// �쐬�� ���򔿋M
/////////////////////////////////////////////////////////////////////////
// �X�V����
//
// 2021/05/19 �쐬�J�n
//            �L���I�[�g�}�g���ł͕s�ւȂ��Ƃ�����̂�
// 
//
//
//////////////////////////////////////////////////////////////////////////


namespace Homare
{
    /// <summary>
    /// State�̒��ۃN���X
    /// </summary>
    public abstract class BossStateBase
    {
        /// <summary>
        /// �X�e�[�g���J�n�������ɌĂ΂��
        /// </summary>
        public virtual void OnEnter(Boss owner, BossStateBase prevState) { }
        /// <summary>
        /// ���t���[���Ă΂��
        /// </summary>
        public virtual void OnUpdate(Boss owner) { }
        /// <summary>
        /// ���t���[�����Ƃ�
        /// </summary>
        public virtual void OnFixedUpdate(Boss owner) { }
        /// <summary>
        /// �X�e�[�g���I���������ɌĂ΂��
        /// </summary>
        public virtual void OnExit(Boss owner, BossStateBase nextState) { }
    }
}