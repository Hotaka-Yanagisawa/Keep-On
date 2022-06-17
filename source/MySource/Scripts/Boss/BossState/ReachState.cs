//////////////////////////////////////////////////
// ����ҁF���򔿋M
// �萻behavior�c���[���g���ɂ����̂ŃX�e�[�g�ɕύX
//
//
//
//
//
//
///////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Homare
{
    public partial class Boss
    {
        //�t�B�[���h�����Ɉړ�����
        public class CenterMoveState : BossStateBase
        {
            public override void OnEnter(Boss owner, BossStateBase prevState)
            {
                owner.animator.SetBool("Attack", false);  
                owner.bossAction = BossAction.Non;
                owner.searchArea.enabled = true;

            }
            public override void OnFixedUpdate(Boss owner)
            {
                //if (owner.animator.GetBool("Stamp")) return;
                //���_�ւ̊p�x
                float Angle = Mathf.Atan2(-owner.transform.position.z,-owner.transform.position.x);
                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));              
                // ���x�x�N�g���̒�����1�b��moveSpeed�����i�ނ悤�ɒ������܂�
                owner.rb.velocity = owner.rb.velocity.normalized * owner.moveSpeed;
                owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);

                // �����ꂩ�̕����Ɉړ����Ă���ꍇ
                if (owner.rb.velocity.magnitude > 0)
                {
                    // �G�̊p�x�̍X�V
                    // Slerp:���݂̌����A�������������A��������X�s�[�h
                    // LookRotation(������������):
                    owner.transform.rotation =
                        Quaternion.Slerp(owner.transform.rotation,
                        Quaternion.LookRotation(owner.rb.velocity),
                        owner.applySpeed);
                }
                //�����ɋ߂Â����玟�̃X�e�[�g��
                if (Vector3.Distance(owner.transform.position, Vector3.zero) < 1)
                {
                    owner.ChangeReachState(bombingState);
                }

            }
        }

        public class BombingState : BossStateBase
        {
            //float actionCnt;
            public override void OnEnter(Boss owner, BossStateBase prevState)
            {
                owner.rb.velocity = Vector3.zero;
                owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);
                owner.animator.SetInteger("AtkType", 0);
                owner.animator.SetBool("Attack", true);
                owner.isAtk = false;
                //actionCnt = 0;
            }
            public override void OnFixedUpdate(Boss owner)
            {
                owner.rb.velocity = Vector3.zero;
                //�֌W���郂�[�V�������ǂ���
                //if (owner.animator.GetCurrentAnimatorStateInfo(0).IsTag("extinction"))
                //{
                //�܂��U���t���O�������ĂȂ��U�����[�V�������Ȃ�
                //if (owner.animator.GetInteger("AtkType") == 1 && !owner.isAtk)
                //{
                //    //�U���J�n�A�U���t���O���Ă�
                //    owner.SetWind(true, 25);
                //    owner.SetExtinction(true);
                //    owner.isAtk = true;
                //    owner.bossAction = BossAction.Atk;
                //    actionCnt = 0;
                //}
                //�U�����Ȃ�J�E���g��i�߂�
                //else if (owner.isAtk) actionCnt += Time.deltaTime;
                //�J�E���g���i�񂾂�end���[�V�����ֈڍs
                //if (actionCnt > 5) owner.animator.SetInteger("AtkType", 2);
                //end���[�V�����ōU���t���O�������Ă�����I��������
                //if (owner.animator.GetInteger("AtkType") == 2 && owner.isAtk)
                //{
                //owner.bossAction = BossAction.Non;
                ////���[�V������ύX
                //owner.animator.SetBool("Attack", false);
                ////�U���I��
                //owner.SetExtinction(false);
                //owner.SetWind(false, 40);
                ////��O�l��
                //actionCnt = -1;
                ////���`�F���W���ɕʂ̂���
                //owner.ChangeStateMachine(BossType.Speed);
                //}

                //}
            }
            
        }

        public class LaserState : BossStateBase
        {
            public override void OnEnter(Boss owner, BossStateBase prevState)
            {
                //owner.animator.SetInteger("AtkType", Random.Range(0, 3));
                owner.animator.SetBool("Attack", true);
                owner.isAtk = false;
                owner.rb.velocity = Vector3.zero;
                owner.bossAction = BossAction.Atk;
            }


            public override void OnFixedUpdate(Boss owner)
            {
                //�U�����[�V�������v���C���[�ɂ�����莲���킹
                if (owner.animator.GetInteger("AtkType") == 2)
                {
                    if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f)
                    {
                        TurnAround(owner);
                    }
                }


                //�I�������v�����̕K�v����
                if (owner.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
                {
                    if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                    {
                        owner.animator.SetBool("Attack", false);
                        //owner.ChangeState(moveState);
                    }
                }
            }

            public override void OnExit(Boss owner, BossStateBase nextState)
            {
                //owner.atkCnt--;
                //if (owner.atkCnt <= 0)
                //{
                //    owner.ChangeStateMachine(BossType.Speed);
                //}
            }
            //�U�����
            void TurnAround(Boss owner)
            {

                float Angle = Mathf.Atan2(owner.player.transform.position.z - owner.transform.position.z,
                 owner.player.transform.position.x - owner.transform.position.x);

                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));
                owner.rb.velocity = owner.rb.velocity.normalized * owner.moveSpeed;
                // �G�̊p�x�̍X�V
                // Slerp:���݂̌����A�������������A��������X�s�[�h
                // LookRotation(������������):
                owner.transform.rotation =
                Quaternion.Slerp(owner.transform.rotation,
                Quaternion.LookRotation(owner.rb.velocity),
                0.1f);
                owner.rb.velocity = Vector3.zero;
            }
        }

        private BossStateBase ReachCurState;
        private static readonly CenterMoveState centerMoveState = new CenterMoveState();
        private static readonly BombingState bombingState = new BombingState();
        private static readonly LaserState laserState = new LaserState();

        /// <summary>
        ///     �X�e�[�g�̕ύX
        /// </summary>
        /// <param name="nextState"></param>
        public void ChangeReachState(BossStateBase nextState)
        {
            if (nextState != ReachCurState)
            {
                ReachCurState.OnExit(this, nextState);
                nextState.OnEnter(this, ReachCurState);
                ReachCurState = nextState;
            }
        }
        public void SetWind(bool active, float Power)
        {
            if (!manage.isResult)
            {
                wind.SetActive(active);
                wind.GetComponent<Wind>().windPower = Power;
            }
            if (active)
            {
                vEffect.Play();
            }
            else
            {
                vEffect.Stop();
            }
        }
        //���U���g���̂݋N��
        public void EndWind()
        {
            if (manage.isResult)
            {
                wind.SetActive(false);
            }
        }

        void SetExtinction(bool exist)
        {
            //extinction.enabled = exist;
            divisionExtinction.enabled = exist;
        }

        public void SetBomb(bool exist)
        {
            if (exist)
            {
                //SetWind(true, 15);
                SetExtinction(true);
                isAtk = true;
                bossAction = BossAction.Atk;
            }
            else
            {
                isAtk = false;
                bossAction = BossAction.Non;
                //���[�V������ύX
                animator.SetBool("Attack", false);
                //�U���I��
                SetExtinction(false);
                //SetWind(false, 10);
                //��O�l��
                //actionCnt = -1;
                //���`�F���W���ɕʂ̂���
                ChangeStateMachine(BossType.Speed);
            }
        }


    }
}