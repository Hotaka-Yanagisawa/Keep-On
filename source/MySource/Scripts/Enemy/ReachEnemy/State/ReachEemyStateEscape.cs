#region �w�b�_�R�����g
// ReachEnemyStateEscape.cs
// �͈͌^�G���G�̓�����ԃN���X
//
// 2021/05/06 : �O��D�l
#endregion


using UnityEngine;
using Homare;


namespace Mikami
{
    public partial class ReachEnemy
    {
        private class StateEscape : EnemyStateBase
        {
            private const int ESCAPE_TIME_COUNT = 8;
            
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                owner.actionCnt = ESCAPE_TIME_COUNT;
                owner.rb.velocity = Vector3.zero;

                float tmpDis = 0;           //�����p�ꎞ�ϐ�
                float nearDis = 0;          //�ł��߂��I�u�W�F�N�g�̋���
                                            //string nearObjName = "";    //�I�u�W�F�N�g����
                GameObject targetObj = null; //�I�u�W�F�N�g

                //�^�O�w�肳�ꂽ�I�u�W�F�N�g��z��Ŏ擾����
                foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Escape"))
                {
                    //���g�Ǝ擾�����I�u�W�F�N�g�̋������擾
                    tmpDis = Vector3.Distance(obs.transform.position, owner.transform.position);

                    //�I�u�W�F�N�g�̋������߂����A����0�ł���΃I�u�W�F�N�g�����擾
                    //�ꎞ�ϐ��ɋ������i�[
                    if (nearDis == 0 || nearDis > tmpDis)
                    {
                        nearDis = tmpDis;
                        //nearObjName = obs.name;
                        targetObj = obs;
                    }
                }
                owner.escapeTransform = targetObj.transform;
            }
            public override void OnUpdate(Enemy owner)
            {
                owner.actionCnt -= Time.fixedDeltaTime;
                Vector3 escapePos = owner.escapeTransform.position;

                escapePos.y = owner.transform.position.y;
                float Distance = Vector3.Distance(owner.transform.position, escapePos);
                // �����s����
                if (owner.actionCnt > 2)
                {
                    if (owner.transform.position.y > 0.8f)
                    {
                        //���炵��0.1f
                        owner.rb.velocity = new Vector3(3, Physics.gravity.y, 3);
                    }
                    else
                    {
                        owner.rb.velocity = Vector3.zero;
                    }
                }
                //���������������
                else if (owner.actionCnt > 1)
                {
                    // �G�̊p�x�̍X�V
                    // Slerp:���݂̌����A�������������A��������X�s�[�h
                    // LookRotation(������������):
                    owner.transform.rotation =
                Quaternion.Slerp(owner.transform.rotation,
                Quaternion.LookRotation(escapePos - owner.transform.position),
                owner.enemyStatus.applySpeed);
                }
                //�ړ�����
                else if (Distance > 1)
                {
                    float Angle = Mathf.Atan2(
                        escapePos.z - owner.transform.position.z,
                     escapePos.x - owner.transform.position.x);

                    owner.rb.velocity =
                        new Vector3(Mathf.Cos(Angle), 0, Mathf.Sin(Angle));

                    // ���x�x�N�g���̒�����1�b��moveSpeed�����i�ނ悤�ɒ������܂�
                    owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed;
                    //owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);

                    owner.transform.rotation =
                        Quaternion.Slerp(owner.transform.rotation,
                        Quaternion.LookRotation(escapePos - owner.transform.position),
                        owner.enemyStatus.applySpeed);
                    owner.rb.constraints = RigidbodyConstraints.FreezeRotationX |
                   RigidbodyConstraints.FreezeRotationY |
                   RigidbodyConstraints.FreezeRotationZ |
                   RigidbodyConstraints.FreezePositionY;
                }
                else
                {
                    owner.rb.constraints = RigidbodyConstraints.FreezeRotationX |
                                      RigidbodyConstraints.FreezeRotationY |
                                      RigidbodyConstraints.FreezeRotationZ |
                                      RigidbodyConstraints.FreezePositionX |
                                      RigidbodyConstraints.FreezePositionY |
                                      RigidbodyConstraints.FreezePositionZ;
                    Ohira.CameraController.Instance.RemoveLockOn(owner.gameObject);
                    owner.rb.velocity = Vector3.zero;
                    if (!owner.isEscape)
                    {
                        owner.isEscape = true;
                        owner.alpha = 1f;
                    }

                    owner.alpha -= Time.fixedDeltaTime * 3.0f;
                    if (owner.alpha < 0f)
                    {
                        owner.transform.position = new Vector3(0, 20, 0);

                        owner.transform.parent.gameObject.SetActive(false);
                        owner.alpha = 1f;
                        owner.GetComponent<MaterialSwap>().SetMaterial(1);
                    }
                    else
                        owner.GetComponent<MaterialSwap>().SetMaterial(0).SetFloat("_Alpha", owner.alpha);

                }

            }
        }
    }
}