#region ÉwÉbÉ_ÉRÉÅÉìÉg
// ReachEnemy.cs
// îÕàÕå^éGãõìGÉNÉâÉX
//
// 2021/04/25 : éOè„óDìl
#endregion


using UnityEngine;
using UnityEngine.VFX;
using System.Collections.Generic;
using Homare;
using System.Collections;

namespace Mikami
{
    public partial class ReachEnemy : Enemy
    {
        private static readonly StateIdle stateIdle = new StateIdle();
        private static readonly StatePatrol statePatrol = new StatePatrol();
        private static readonly StateReturn stateReturn = new StateReturn();
        private static readonly StateBattleMove stateBattleMove = new StateBattleMove();
        private static readonly StateAim stateAim = new StateAim();
        private static readonly StateShot stateShot = new StateShot();
        private static readonly StateEscape stateEscape = new StateEscape();
        private static readonly StateFalling stateFalling = new StateFalling();
        private static readonly StateDead stateDead = new StateDead();
        

        [SerializeField] private GameObject weaponL;
        [SerializeField] private GameObject weaponR;
        private VisualEffect aimEffectL;
        private VisualEffect aimEffectR;
        [SerializeField] private VisualEffect shotEffectL;
        [SerializeField] private VisualEffect shotEffectR;
        [SerializeField] private GameObject bullet;
        [SerializeField] private GameObject lockOnImage;
        private ReachEnemyLockOnImage reachLockOnImage;

        private GameObject bulletL;
        private GameObject bulletR;

        private const int SHOT_COOL_TIME = 180;
        private int shotCoolCnt = 0;

        protected override void Awake()
        {
            base.Awake();

            aimEffectL = weaponL.GetComponent<VisualEffect>();
            aimEffectR = weaponR.GetComponent<VisualEffect>();

            currentState = stateFalling;
            aimEffectL.enabled = false;
            aimEffectR.enabled = false;
            
            //aimEffectL?.SetBool("Fire", false);
            //aimEffectR?.SetBool("Fire", false);
            shotEffectL.enabled = false;
            shotEffectR.enabled = false;

            var obj = Instantiate(lockOnImage, canvas.transform);
            reachLockOnImage = obj.GetComponent<ReachEnemyLockOnImage>();

            bulletL = Instantiate(bullet, weaponL.transform.position, weaponL.transform.rotation);
            bulletL.SetActive(false);
            bulletR = Instantiate(bullet, weaponR.transform.position, weaponR.transform.rotation);
            bulletR.SetActive(false);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ChangeState(stateFalling);
            rb.velocity = Vector3.zero;
            //GetComponent<EffectOperate>().CreateEffect(1, transform.position, 1.7f);
        }

        protected override void Escape()
        {
            base.Escape();
            ChangeState(stateEscape);
            weaponL.SetActive(false);
            weaponR.SetActive(false);
        }

        protected override void Landing()
        {
            base.Landing();
            transform.GetChild(0).gameObject.SetActive(true);
            if (currentState != stateEscape)
                ChangeState(stateIdle);
        }

        protected override void Death()
        {
            base.Death();
            ChangeState(stateDead);
        }

        public void OnFindPlayer()
        {
            ChangeState(stateBattleMove);
        }

        public void OnLostPlayer()
        {
            ChangeState(stateIdle);
        }

        public void ShotEffect(int cnt)
        {
            StartCoroutine(ShotCoroutine(cnt));
        }

        public void Shot()
        {
            bulletL.SetActive(true);
            bulletL.transform.SetPositionAndRotation(weaponL.transform.position, weaponL.transform.rotation);
            bulletL.GetComponent<ReachEnemyBullet>().Shot(weaponL.transform.forward, enemyStatus.attack);
            bulletR.SetActive(true);
            bulletR.transform.SetPositionAndRotation(weaponR.transform.position, weaponR.transform.rotation);
            bulletR.GetComponent<ReachEnemyBullet>().Shot(weaponR.transform.forward, enemyStatus.attack);
        }

        private IEnumerator ShotCoroutine(int cnt)
        {
            shotEffectL.enabled = true;
            shotEffectR.enabled = true;

            for (int i = 0; i < cnt; ++i)
                yield return null;

            shotEffectL.enabled = false;
            shotEffectR.enabled = false;
        }
    }
}