using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawnManager : MonoBehaviour {

	public List<Vector3> healthSpawn;
	public GameObject heartPrefab;
	public GameObject tankPrefab;

	//Initialization
	void Start () {
		
	}


	public void removeAllHealth() {
		GameObject[] health = GameObject.FindGameObjectsWithTag("Health");

		foreach(GameObject item in health)
		{
			Destroy(item);
		}
			
	}

	public void Spawn() {
		//4 spawn points
		healthSpawn.Add(new Vector3 (23f, 1f, -39f));
		healthSpawn.Add(new Vector3 (-20f, 1f, -32f));
		healthSpawn.Add(new Vector3(-35f, 1f, 38.2f));
		healthSpawn.Add(new Vector3(37.68f, 1, 31.69f));

		//Get random in from within the List
		int randomIndex = Random.Range (0, 3);

		//Spawn and rotate the object
		GameObject spawnOne = Instantiate(heartPrefab, healthSpawn[randomIndex], tankPrefab.transform.rotation);
		spawnOne.transform.Rotate(new Vector3(-90f, 0f, 0f));

		//Remove the spawned item from the list so two objects cant be spawned in the same spot
		healthSpawn.RemoveAt(randomIndex);


		int randomIndexTwo = Random.Range (0, 2); //Calculate a new random index with the smaller array List

		//Spawn health number two
		GameObject spawnTwo = Instantiate(heartPrefab, healthSpawn[randomIndexTwo], tankPrefab.transform.rotation);
		spawnTwo.transform.Rotate(new Vector3(-90f, 0f, 0f));

		healthSpawn.RemoveAt (randomIndexTwo);
	}

}
