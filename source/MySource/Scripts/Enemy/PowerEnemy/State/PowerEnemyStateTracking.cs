/////////////////////////////////////////////////////////////////////////
// 作成日 2021/03/18
// 作成者 柳沢帆貴
/////////////////////////////////////////////////////////////////////////
// 更新日時
//
// 2021/03/18 作成開始
//            追跡状態の処理を行う
// 
// 2021/04/22 再構築by三上
//
//////////////////////////////////////////////////////////////////////////


using UnityEngine;


namespace Homare
{
    public partial class PowerEnemy
    {
        /// <summary>
        /// 追跡状態
        /// </summary>
        public class StateTracking : EnemyStateBase
        {
            public override void OnEnter(Enemy owner, EnemyStateBase prevState)
            {
                Debug.Log("追跡");

                //float Angle = Mathf.Atan2(player.transform.position.z - owner.transform.position.z,
                // player.transform.position.x - owner.transform.position.x);

                //owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));

                //owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed;


                //owner.transform.rotation =
                //        Quaternion.Slerp(owner.transform.rotation,
                //        Quaternion.LookRotation(owner.rb.velocity),
                //        owner.enemyStatus.applySpeed);
            }

            public override void OnFixedUpdate(Enemy owner)
            {
                float Angle = Mathf.Atan2(player.transform.position.z - owner.transform.position.z,
                                player.transform.position.x - owner.transform.position.x);

                owner.rb.velocity = new Vector3(Mathf.Cos(Angle), owner.rb.velocity.y, Mathf.Sin(Angle));
                //owner.velocity.x = Mathf.Cos(Angle);
                //owner.velocity.z = Mathf.Sin(Angle);
                // 速度ベクトルの長さを1秒でmoveSpeedだけ進むように調整します
                owner.rb.velocity = owner.rb.velocity.normalized * owner.enemyStatus.moveSpeed;

                owner.animator.SetFloat("Speed", owner.rb.velocity.magnitude);

                // いずれかの方向に移動している場合
                if (owner.rb.velocity.magnitude > 0)
                {
                    // 敵の角度の更新
                    // Slerp:現在の向き、向きたい方向、向かせるスピード
                    // LookRotation(向きたい方向):
                    owner.transform.rotation =
                        Quaternion.Slerp(owner.transform.rotation,
                        Quaternion.LookRotation(owner.rb.velocity),
                        owner.enemyStatus.applySpeed);

                    //owner.rb.velocity = owner.velocity;
                }
            }
        }
    }
}