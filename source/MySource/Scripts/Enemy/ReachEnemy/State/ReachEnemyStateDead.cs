#region ƒwƒbƒ_ƒRƒƒ“ƒg
// ReachEnemyStateEscape.cs
// ”ÍˆÍŒ^G‹›“G‚Ì€–Só‘ÔƒNƒ‰ƒX
//
// 2021/05/06 : Oã—D“l
#endregion


using UnityEngine;
using Homare;


namespace Mikami
{
    public partial class ReachEnemy
    {
        private class StateDead : EnemyStateBase
        {
            private const float DEAD_TIME_COUNT = 2;

            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                owner.actionCnt = DEAD_TIME_COUNT;
                owner.GetComponent<EffectOperate>().CreateEffect(0, owner.transform.position, 1.6f);
                owner.rb.velocity = Vector3.zero;
            }

            public override void OnUpdate(Enemy owner)
            {
                owner.rb.velocity = Vector3.zero;
                owner.actionCnt -= Time.fixedDeltaTime;
                if (owner.actionCnt <= 0)
                {
                    owner.transform.position = new Vector3(0, 20, 0);

                    owner.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }
}