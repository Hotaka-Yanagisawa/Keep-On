#region ƒwƒbƒ_ƒRƒƒ“ƒg
// ReachEnemyStateShot.cs
// ”ÍˆÍŒ^G‹›“G‚ÌËŒ‚ó‘ÔƒNƒ‰ƒX
//
// 2021/04/26 : Oã—D“l
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
                // —\‘ªü‚Ì–¾–Åˆ—
                ReachEnemy reachEnemy = owner as ReachEnemy;
                float alpha = owner.actionCnt % 4 / 4f;
                reachEnemy.aimEffectL?.SetFloat("Alpha", alpha);
                reachEnemy.aimEffectR?.SetFloat("Alpha", alpha);

                if (--owner.actionCnt <= 0)
                {
                    // UŒ‚ˆ—‹Lq
                    // ’e‚ğËo/—LŒø‰»AËo•ûŒü‚Ìw’è
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