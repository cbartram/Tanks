using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPowerup : MonoBehaviour {

	public GameObject tankPrefab;
	public GameObject shell;            
	//private ShellExplosion shellExplosion;


	void Start() 
	{
		//shellExplosion = GetComponent<ShellExplosion> ();
	}

	void OnCollisionEnter(Collision collision)
	{

		if (collision.gameObject.name == "RocketPowerup(Clone)") {

//			shellExplosion.explosionRadius = 15f;
//			shellExplosion.maxDamage = 150f;

		}

		//Destroy the rocket after granting the powerup
		Destroy (collision.gameObject);

	}
}
