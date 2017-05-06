using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketAnimations : MonoBehaviour {

	public Vector3 rotationAngle;
	public float rotationSpeed = 1.3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
			transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
	}

}
