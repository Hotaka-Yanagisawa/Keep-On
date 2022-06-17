using System.Collections;
//////////////////////////////
// SpeedEnemyAttackDamage.cs
//----------------------------
// 作成日:2021/5/12 
// 作成者:久田律貴
//----------------------------
// 更新日時・内容
//  ・スクリプト作成
//
//
//////////////////////////////

using UnityEngine;

namespace Hisada
{
    public class SpeedEnemyAttackDamage : MonoBehaviour
    {
        [SerializeField] public CapsuleCollider Collider;
        public EnemyStatus status;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Collider.enabled = false;
                Maeda.Player playern = other.GetComponent<Maeda.Player>();
                playern.OnHitEnemyAttack(status.attack, other.ClosestPointOnBounds(transform.position));
                other.gameObject.SendMessage("OnKnockBack");
            }
        }
    }
}