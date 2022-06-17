#region HeaderComent
//==================================================================================
// ResultStateBase
// ���U���g�̃X�e�[�g�̒��ۃN���X
// �쐬����	:2021/03/20
// �쐬��	:�O�c����
//---------- �X�V���� ----------
// 2021/03/19   OnFixUpdate��ǉ�  ���ɈӖ��͂Ȃ��̂Ō���OnUpdate()�Ɠ����������s���Ă���
//==================================================================================
#endregion


/// <summary>
/// State�̒��ۃN���X
/// ��������~�����Ȃ����@�\��ǉ��ł���
/// ��jOn�`�`(Player owner){ } 
/// ���N���X���Ǝv��...
/// </summary>
namespace Maeda
{
    public abstract class ResultStateBase
    {
        #region �p�u���b�N�֐�


        /// <summary>
        /// �X�e�[�g���J�n�������ɌĂ΂��
        /// </summary>
        public virtual void OnEnter(Result owner, ResultStateBase prevState) { }


        /// <summary>
        /// ���t���[���Ă΂��
        /// </summary>
        public virtual void OnUpdate(Result owner) { }


        /// <summary>
        /// ���t���[�����ɌĂ΂��
        /// </summary>
        public virtual void OnFixUpdate(Result owner) { }


        /// <summary>
        /// �X�e�[�g���I���������ɌĂ΂��
        /// </summary>
        public virtual void OnExit(Result owner, ResultStateBase nextState) { }


        #endregion
    }
}
