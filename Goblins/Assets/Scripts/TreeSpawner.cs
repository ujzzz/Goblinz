using UnityEngine;
using System.Collections;

public class TreeSpawner : MonoBehaviour {

	[SerializeField]
	private GameObject treePrefab;
	[SerializeField]
	private float treeAmount = 50f;
	[SerializeField]
	private float spawnArea = 3f;
	[SerializeField]
	private float spawnSize = 5f;
	private float lastX = 0f;
	private float lastZ = 0f;
	[SerializeField]
	private float maxSpawnArea = 10f;
	private float positionX;
	private float positionZ;
	private Vector3 treePosition;

	// Use this for initialization
	void Start () {	}
	
	// Update is called once per frame
	void Update () {
		while (treeAmount > 0) {
			// decides where to spawn, but within range of the last spawn position
			RandomizeSpawn ();

			// if spawn point outside of map
			if (positionX > maxSpawnArea || positionX < -maxSpawnArea || positionZ > maxSpawnArea || positionZ < -maxSpawnArea) {
				// then select new random spawn point
				positionX = Random.Range (-maxSpawnArea, maxSpawnArea);
				positionZ = Random.Range (-maxSpawnArea, maxSpawnArea);
				treePosition = new Vector3 (positionX, 1.43f, positionZ);
			} 
			// otherwise its all good
			else { treePosition = new Vector3 (positionX, 1.43f, positionZ); }

			// stores spawn point to a field which we use to decide next spawn point
			lastX = positionX;
			lastZ = positionZ;

			// creates random size for the tree
			float scaleSize = Random.Range (1f, spawnSize);
			Vector3 treeSize = new Vector3 (scaleSize, scaleSize, scaleSize);
			treePrefab.transform.localScale = treeSize;

			// lowers total amount of tree left to spawn
			treeAmount = treeAmount - scaleSize;

			// create the damn thing
			Instantiate(treePrefab, treePosition, Quaternion.identity);
		}
	}

	private void RandomizeSpawn () {
		positionX = lastX + Random.Range (-spawnArea, spawnArea);
		positionZ = lastZ + Random.Range (-spawnArea, spawnArea);
	}
}
