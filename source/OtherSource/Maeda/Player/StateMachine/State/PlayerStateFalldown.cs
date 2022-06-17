using UnityEngine;

#region HeaderComent
//==================================================================================
// PlayerStateDead
// �v���C���[�̗������
// �쐬����	:2021/03/20
// �쐬��	:�O�c����
//---------- �X�V���� ----------
// 2021/03/19   �����̃A�j���[�V����
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
        public class StateFalldown : PlayerStateBase
        {
            public override void OnEnter(Player owner, PlayerStateBase prevState)
            {
                owner.animator.SetBool("falldown", true);
            }

            public override void OnUpdate(Player owner)
            {
                #region ���͂ɂ�镪��


                //#region �f��
                //if (owner.baseParam.style.style == Style.E_Style.NORMAL)
                //{
                //    if (owner.input.Player.Dodge.triggered)
                //        owner.ChangeState(stateDodging);
                //}
                //#endregion

                //#region �Η͌^
                //if (owner.baseParam.style.style == Style.E_Style.POWER)
                //{

                //    if (owner.input.Player.Dodge.triggered)
                //        owner.ChangeState(stateDodging);
                //}
                //#endregion

                //#region �@���^
                //if (owner.baseParam.style.style == Style.E_Style.MOBILITY)
                //{

                //    if (owner.input.Player.Dodge.triggered)
                //        owner.ChangeState(stateDodging);
                //}
                //#endregion

                //#region �͈͌^
                //if (owner.baseParam.style.style == Style.E_Style.REACH)
                //{
                //    if (owner.input.Player.Dodge.triggered)
                //        owner.ChangeState(stateDodging);
                //}
                //#endregion


                #endregion

                //if (!owner.animator.GetBool("falldown"))
                //{
                //    owner.ChangeState(stateLanding);
                //}
            }

            public override void OnExit(Player owner, PlayerStateBase nextState)
            {
                owner.animator.SetBool("falldown", false);
            }
        }
    }
}
 