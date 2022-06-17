#region ヘッダコメント
// ReachEnemyStateEscape.cs
// 範囲型雑魚敵の逃走状態クラス
//
// 2021/05/06 : 三上優斗
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

                float tmpDis = 0;           //距離用一時変数
                float nearDis = 0;          //最も近いオブジェクトの距離
                                            //string nearObjName = "";    //オブジェクト名称
                GameObject targetObj = null; //オブジェクト

                //タグ指定されたオブジェクトを配列で取得する
                foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Escape"))
                {
                    //自身と取得したオブジェクトの距離を取得
                    tmpDis = Vector3.Distance(obs.transform.position, owner.transform.position);

                    //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
                    //一時変数に距離を格納
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
                // 立ち尽くす
                if (owner.actionCnt > 2)
                {
                    if (owner.transform.position.y > 0.8f)
                    {
                        //ずらしの0.1f
                        owner.rb.velocity = new Vector3(3, Physics.gravity.y, 3);
                    }
                    else
                    {
                        owner.rb.velocity = Vector3.zero;
                    }
                }
                //消える方向を向く
                else if (owner.actionCnt > 1)
                {
                    // 敵の角度の更新
                    // Slerp:現在の向き、向きたい方向、向かせるスピード
                    // LookRotation(向きたい方向):
                    owner.transform.rotation =
                Quaternion.Slerp(owner.transform.rotation,
                Quaternion.LookRotation(escapePos - owner.transform.position),
                owner.enemyStatus.applySpeed);
                }
                //移動する
                else if (Distance > 1)
                {
                    float Angle = Mathf.Atan2(
                        escapePos.z - owner.transform.position.z,
                     escapePos.x - owner.transform.position.x);

                    owner.rb.velocity =
                        new Vector3(Mathf.Cos(Angle), 0, Mathf.Sin(Angle));

                    // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
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