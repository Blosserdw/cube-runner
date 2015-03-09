using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public GameObject[,] Tiles;
	public int numTilesX = 2;
	public int numTilesZ = 2;
	public GameObject tilePrefab;
	public GameObject playerPrefab;

	// Use this for initialization
	void Start ()
	{
		Tiles = new GameObject[numTilesX, numTilesZ];
		
		GenerateMap();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public void GenerateMap()
	{
		for (int i = 0; i < numTilesX; i++)
		{
			for (int j = 0; j < numTilesZ; j++)
			{
				// Set up random height
				float height = 0.0f;
//				int randomHeight = Random.Range(0, 100);
//				if (randomHeight <= 20)
//					height = 0.0f;
//				else if (randomHeight > 20 && randomHeight <= 70)
//					height = 0.2f;
//				else if (randomHeight > 70 && randomHeight <= 90)
//					height = 0.4f;
//				else
//					height = 0.6f;


				// Create Tiles
				Debug.Log("Instantiating a tile at: " + i + ", " + j);
				GameObject newTile = GameObject.Instantiate(tilePrefab, new Vector3((float)i, height, (float)j), Quaternion.identity) as GameObject;
				
				// Set up this individual tile when placed
				newTile.transform.parent = this.gameObject.transform;
				newTile.layer = 8; // Ground layer for A*
				Tiles[i, j] = newTile;
			}
		}

		//GenerateObstacles();
		
		// Scan the graph before adding the player
		//GameManager.Instance.aStarObject.GetComponent<AstarPath>().Scan();
		
		// Add the player
		SetupPlayer();
	}

	public Vector3 playerStartPosition = new Vector3(0.0f, 0.0f, 4.0f);
	
	public void SetupPlayer()
	{
		// Later should use a starting position to pass into the vector here
		GameObject newPlayer = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		newPlayer.transform.position = Tiles[0,0].transform.position + playerStartPosition;
		//GameManager.Instance.SetUpPlayer(newPlayer, this);

		// Start the environment changes after the player has been running around for a bit
		Invoke("StartCubeTransformations", 3.0f);
	}

	public float cubeMovementTime = 2.0f;
	public float cubeTransformTime = 4.0f;

	public void StartCubeTransformations()
	{
		// Choose the cube to transform
		int randomX = Random.Range(0, numTilesX);
		int randomZ = Random.Range(0, numTilesZ);
		bool cubeGoingUp;
		if (Random.value >= 0.5f)
			cubeGoingUp = true;
		else
			cubeGoingUp = false;

		//Debug.Log("Going to move Cube " + randomX + ", " + randomZ + " " + cubeGoingUp);
		// Start to blink this tile
		Tiles[randomX, randomZ].GetComponentInChildren<Animation>().Play("tileColorBlink");
		StartCoroutine(ActivateTransformation(Tiles[randomX, randomZ], cubeGoingUp));
	}

	public IEnumerator ActivateTransformation(GameObject thisTile, bool cubeIsGoingUp)
	{
		yield return new WaitForSeconds(0.8f);

		// Designate the tile as transforming for collision purposes
		thisTile.GetComponent<Tile>().isTransforming = true;

		Vector3 destination = new Vector3(thisTile.transform.position.x, 0f, thisTile.transform.position.z);
		if (cubeIsGoingUp)
			destination = new Vector3(thisTile.transform.position.x, 1.0f, thisTile.transform.position.z);
		else
			destination = new Vector3(thisTile.transform.position.x, -1.0f, thisTile.transform.position.z);

		iTween.MoveTo(thisTile, destination, cubeMovementTime);

		StartCoroutine(ReturnCubeToDefaultPosition(thisTile, cubeIsGoingUp));
		Invoke("StartCubeTransformations", 1.0f);
	}

	public IEnumerator ReturnCubeToDefaultPosition(GameObject thisTile, bool cubeIsGoingUp)
	{
		yield return new WaitForSeconds(cubeTransformTime);

		Vector3 destination = new Vector3(thisTile.transform.position.x, 0f, thisTile.transform.position.z);
		if (cubeIsGoingUp)
			destination = new Vector3(thisTile.transform.position.x, -1.0f, thisTile.transform.position.z);
		else
			destination = new Vector3(thisTile.transform.position.x, 1.0f, thisTile.transform.position.z);
		
		iTween.MoveTo(thisTile, destination, cubeMovementTime);

		StartCoroutine(ResetTileStatus(thisTile));

	}

	public IEnumerator ResetTileStatus(GameObject thisTile)
	{
		yield return new WaitForSeconds(cubeMovementTime);

		// Tile is done moving, reset it's status
		thisTile.GetComponent<Tile>().isTransforming = false;
	}
}






