#region �w�b�_�R�����g
// ReachEnemyStateShot.cs
// �͈͌^�G���G�̎ˌ���ԃN���X
//
// 2021/04/26 : �O��D�l
#endregion


using UnityEngine;
using Homare;


namespace Mikami
{
    public partial class ReachEnemy
    {
        private class StateShot : EnemyStateBase
        {
            private const int SHOT_TIME_COUNT = 60;

            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                owner.actionCnt = SHOT_TIME_COUNT;
                
                ReachEnemy reachEnemy = owner as ReachEnemy;
                //reachEnemy.aimEffectL?.SetBool("Fire", true);
                reachEnemy.aimEffectL.enabled = true;
                reachEnemy.aimEffectL?.SetFloat("Alpha", 1f);
                //reachEnemy.aimEffectR?.SetBool("Fire", true);
                reachEnemy.aimEffectR.enabled = true;        
                reachEnemy.aimEffectR?.SetFloat("Alpha", 1f);
                reachEnemy.reachLockOnImage.LockOn(player.transform.Find("Icon"));
                reachEnemy.ShotEffect(90);

                Mizuno.SoundManager.Instance.PlayMenuSe("SE_ReachLockOn");
            }

            public override void OnUpdate(Enemy owner)
            {
                // �\�����̖��ŏ���
                ReachEnemy reachEnemy = owner as ReachEnemy;
                float alpha = owner.actionCnt % 4 / 4f;
                reachEnemy.aimEffectL?.SetFloat("Alpha", alpha);
                reachEnemy.aimEffectR?.SetFloat("Alpha", alpha);

                if (--owner.actionCnt <= 0)
                {
                    // �U�������L�q
                    // �e���ˏo/�L�����A�ˏo�����̎w��
                    reachEnemy.Shot();

                    reachEnemy.shotCoolCnt = SHOT_COOL_TIME;
                    owner.ChangeState(stateBattleMove);
                }
            }

            public override void OnExit(Enemy owner, EnemyStateBase nextState)
            {
                ReachEnemy reachEnemy = owner as ReachEnemy;
                //reachEnemy.aimEffectL?.SetBool("Fire", false);
                reachEnemy.aimEffectL.enabled = false;
                reachEnemy.aimEffectL?.SetFloat("Alpha", 0f);
                //reachEnemy.aimEffectR?.SetBool("Fire", false);
                reachEnemy.aimEffectR.enabled = false;
                reachEnemy.aimEffectR?.SetFloat("Alpha", 0f);
            }
        }
    }
}