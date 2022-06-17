using System.Collections;
//////////////////////////////
// SpeedEnemyAttackDamage.cs
//----------------------------
// �쐬��:2021/5/12 
// �쐬��:�v�c���M
//----------------------------
// �X�V�����E���e
//  �E�X�N���v�g�쐬
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