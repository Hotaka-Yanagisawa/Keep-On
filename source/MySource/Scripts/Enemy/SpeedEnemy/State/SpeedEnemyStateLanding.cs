//////////////////////////////
// SpeedEnemyStateFalling.cs
//----------------------------
// �쐬��:2021/5/7 
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
        /// �ҋ@���
        /// </summary>
        private class StateLanding : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                owner.rb.velocity = Vector3.zero;

            }


            //public override void OnFixedUpdate(Enemy owner)
            //{
            //    base.OnFixedUpdate(owner);
            //}
        }
    }
}
