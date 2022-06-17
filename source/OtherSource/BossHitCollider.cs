using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitCollider : MonoBehaviour
{
	public static Homare.Boss bossCS;


	public void OnHitPlayerAttack(float damage, Vector3 hitPos)
	{
		bossCS.OnHitPlayerAttack(damage, hitPos);
	}


	public bool OnHitPlayerSteal()
	{
		return bossCS.OnHitPlayerSteal();
	}
}
