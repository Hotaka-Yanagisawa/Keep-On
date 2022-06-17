using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateLanding
// �v���C���[�̗������
// �쐬����	:2021/03/20
// �쐬��	:�O�c����
//---------- �X�V���� ----------
// 2021/03/19   ���n�̃A�j���[�V����
//==================================================================================
#endregion

/// <summary>
/// ���S���̏���
/// ��������ΐ����X�V����
/// </summary>
namespace Maeda
{
    public partial class Player
    {
        public class StateLanding : PlayerStateBase
        {
            float landCnt = 0.5f;
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                owner.animator.SetBool("landing", true);
            }

            public override void OnUpdate(Player owner)
            {
                landCnt -= Time.deltaTime;


                if (!owner.animator.GetBool("landing"))
                {
                    owner.ChangeState(stateStanding);
                }

                if(landCnt <= 0f)
                {
                    owner.animator.SetBool("landing", false);
                    landCnt = 0.5f;
                    owner.ChangeState(stateStanding);
                }
            }

          
        }

        private void OnLandingEnd()
        {
            animator.SetBool("landing", false);
        }
    }
}
