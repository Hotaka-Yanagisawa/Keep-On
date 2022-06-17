//////////////////////////////
// SpeedEnemyStateKnockback.cs
//----------------------------
// �쐬��:2021/4/25 
// �쐬��:�v�c���M
//----------------------------
// �X�V�����E���e
//  �E�X�N���v�g�쐬
//
//
//////////////////////////////
using UnityEngine;
using Homare;

namespace Hisada
{
    public partial class SpeedEnemy
    {
        /// <summary>
        /// �m�b�N�o�b�N���
        /// </summary>
        class StateKnockback : EnemyStateBase
        {
            int Cnt = 60;
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("Speed:�m�b�N�o�b�N");

            }

            public override void OnUpdate(Enemy owner)
            {
                Cnt--;
                if (Cnt <= 0)
                {
                    owner.ChangeState(stateWaiting);
                }
            }
        }
    }
}
