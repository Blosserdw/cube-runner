using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CubePattern
{
	Single,
	Double,
	Triple,
	FourSquare
}

public class Map : MonoBehaviour {

	public GameObject[,] Tiles;
	public int numTilesX = 10;
	public int numTilesZ = 10;
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
				transformableTiles.Add(Tiles[i, j]);
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
	public List<GameObject> transformableTiles = new List<GameObject>();
	private bool foundTile = false;

	public void StartCubeTransformations()
	{
		// Choose the cube to transform
		GameObject randomStartTile = transformableTiles[Random.Range(0, transformableTiles.Count)];
		int tilesToCheck = 0;

		// Is this cube transformation going up or down?
		bool cubeGoingUp;
		if (Random.value >= 0.5f)
			cubeGoingUp = true;
		else
			cubeGoingUp = false;

		// Which cube transformation is it?
		CubePattern thisPattern = (CubePattern)Random.Range(0, (int)CubePattern.Triple + 1);

		List<GameObject> tileGroup = new List<GameObject>();

		// Grab all the cubes connected in the pattern
		if (thisPattern == CubePattern.Single)
		{
			//===============================||
			// SINGLE CUBE
			//===============================||

			// Add to the group that will get transformed
			tileGroup.Add(randomStartTile);
			tilesToCheck = 1;

			// Start to blink this tile
			randomStartTile.GetComponentInChildren<Animation>().Play("tileColorBlink");

			// Then remove it from the list so it can't get chosen again in the meantime
			transformableTiles.Remove(randomStartTile);

			Debug.Log("transformed a SINGLE tile");
		}
		else if (thisPattern == CubePattern.Double)
		{
			//===============================||
			// DOUBLE CUBES
			//===============================||

			// Add to transforming tile group
			tileGroup.Add(randomStartTile);
			tilesToCheck = 1;

			// Find the other tile in this configuration
			// Choose Up/Down or Left/Right to check first
			bool checkUpDown;
			if (Random.value >= 0.5f)
				checkUpDown = true;
			else
				checkUpDown = false;

			foundTile = false;
			// Check the direction for another cube
			if (checkUpDown)
			{
				// See if there's one above/below this cube that isn't transformed at the moment
				if (CheckUpDownFirst(randomStartTile) != null)
				{
					tileGroup.Add(CheckUpDownFirst(randomStartTile));
					tilesToCheck++;
				}

			}
			else // Check Left/Right for another cube
			{
				// See if there's one to the left/right of this cube that isn't transformed at the moment
				if (CheckLeftRightFirst(randomStartTile) != null)
				{
					tileGroup.Add(CheckLeftRightFirst(randomStartTile));
					tilesToCheck++;
				}
			}

			Debug.Log("transformed a DOUBLE tile");
		}
		else if (thisPattern == CubePattern.Triple)
		{
			//===============================||
			// TRIPLE CUBES
			//===============================||
			
			// Add to transforming tile group
			tileGroup.Add(randomStartTile);
			tilesToCheck = 1;
			
			// Find the other tile in this configuration
			// Choose Up/Down or Left/Right to check first
			bool checkUpDown;
			if (Random.value >= 0.5f)
				checkUpDown = true;
			else
				checkUpDown = false;
			
			foundTile = false;
			// Check the direction for another cube
			if (checkUpDown)
			{
				for (int i = 0; i < (int)CubePattern.Triple; i++)
				{
					if (CheckUpDownFirst(tileGroup[i]) != null)
					{
						tileGroup.Add(CheckUpDownFirst(tileGroup[i]));
						tilesToCheck++;
					}
				}
			}
			else // Check Left/Right for another cube
			{
				// See if there's one to the left/right of this cube that isn't transformed at the moment
				for (int i = 0; i < (int)CubePattern.Triple; i++)
				{
					if (CheckLeftRightFirst(tileGroup[i]) != null)
					{
						tileGroup.Add(CheckLeftRightFirst(tileGroup[i]));
						tilesToCheck++;
					}
				}
			}

			Debug.Log("transformed a TRIPLE tile");
		}
		else
		{
			// Wrong pattern?
		}

		// Run operations on all tiles in the group now
		Debug.Log("Number of tiles in tile group that should play the blink anim is: " + tileGroup.Count);
		Debug.Log("But the tiles to check count is: " + tilesToCheck);
		for (int i = 0; i < tilesToCheck; i++)
		{
			Debug.Log("Tile" + i + " is: " + tileGroup[i]);

			// Start to blink this tile
			tileGroup[i].GetComponentInChildren<Animation>().Play("tileColorBlink");

			// Then remove it from the list so it can't get chosen again in the meantime
			transformableTiles.Remove(tileGroup[i]);
		}

		StartCoroutine(ActivateTransformation(tileGroup, cubeGoingUp));


	}

	public IEnumerator ActivateTransformation(List<GameObject> tileGroup, bool cubeIsGoingUp)
	{
		yield return new WaitForSeconds(0.8f);

		for (int i = 0; i < tileGroup.Count; i++)
		{
			tileGroup[i].GetComponent<Tile>().isTransforming = true;

			Vector3 destination;
			if (cubeIsGoingUp)
				destination = new Vector3(tileGroup[i].transform.position.x, 1.0f, tileGroup[i].transform.position.z);
			else
				destination = new Vector3(tileGroup[i].transform.position.x, -1.0f, tileGroup[i].transform.position.z);

			iTween.MoveTo(tileGroup[i], destination, cubeMovementTime);
			StartCoroutine(ReturnCubeToDefaultPosition(tileGroup, cubeIsGoingUp));
		}

		Invoke("StartCubeTransformations", 1.0f);
	}

	public IEnumerator ReturnCubeToDefaultPosition(List<GameObject> tileGroup, bool cubeWentUp)
	{
		yield return new WaitForSeconds(cubeTransformTime);

		for (int i = 0; i < tileGroup.Count; i++)
		{
			Vector3 destination = new Vector3(tileGroup[i].transform.position.x, 0f, tileGroup[i].transform.position.z);
			if (cubeWentUp)
			{
				// Cubes that go up must come down
				destination = new Vector3(tileGroup[i].transform.position.x, 0.0f, tileGroup[i].transform.position.z);
				iTween.MoveTo(tileGroup[i], destination, cubeMovementTime);
				StartCoroutine(ResetTileStatus(tileGroup[i]));
			}
			else
			{
				// Cubes that go down never come back up
				//destination = new Vector3(thisTile.transform.position.x, 1.0f, thisTile.transform.position.z);
			}
		}
	}

	public IEnumerator ResetTileStatus(GameObject thisTile)
	{
		yield return new WaitForSeconds(cubeMovementTime);

		// Tile is done moving, reset it's status
		thisTile.GetComponent<Tile>().isTransforming = false;
		// Add it back to the list for tiles available for transformation
		transformableTiles.Add(thisTile);
	}

	public GameObject CheckUpDownFirst(GameObject fromThisTile)
	{
		if ((int)fromThisTile.transform.position.x + 1 <= 9
		    && !Tiles[(int)fromThisTile.transform.position.x + 1, (int)fromThisTile.transform.position.z].GetComponent<Tile>().isTransforming)
		{
			// One is above
			foundTile = true;
			return(Tiles[(int)fromThisTile.transform.position.x + 1, (int)fromThisTile.transform.position.z]);
		}
		else if ((int)fromThisTile.transform.position.x - 1 >= 0
		         && !Tiles[(int)fromThisTile.transform.position.x - 1, (int)fromThisTile.transform.position.z].GetComponent<Tile>().isTransforming)
		{
			// One is below
			foundTile = true;
			return(Tiles[(int)fromThisTile.transform.position.x - 1, (int)fromThisTile.transform.position.z]);
		}
		
		if (!foundTile)
		{
			// If we didn't find one above or below, check left/right
			// See if there's one to the left/right of this cube that isn't transformed at the moment
			if ((int)fromThisTile.transform.position.z - 1 >= 0
			    && !Tiles[(int)fromThisTile.transform.position.x, (int)fromThisTile.transform.position.z - 1].GetComponent<Tile>().isTransforming)
			{
				// One is left
				foundTile = true;
				return(Tiles[(int)fromThisTile.transform.position.x, (int)fromThisTile.transform.position.z - 1]);
			}
			else if ((int)fromThisTile.transform.position.z + 1 <= 9
			         && !Tiles[(int)fromThisTile.transform.position.x, (int)fromThisTile.transform.position.z + 1].GetComponent<Tile>().isTransforming)
			{
				// One is right
				foundTile = true;
				return(Tiles[(int)fromThisTile.transform.position.x, (int)fromThisTile.transform.position.z + 1]);
			}
		}

		return null;
	}

	public GameObject CheckLeftRightFirst(GameObject fromThisTile)
	{
		if ((int)fromThisTile.transform.position.z - 1 >= 0
		    && !Tiles[(int)fromThisTile.transform.position.x, (int)fromThisTile.transform.position.z - 1].GetComponent<Tile>().isTransforming)
		{
			// One is left
			foundTile = true;
			return(Tiles[(int)fromThisTile.transform.position.x, (int)fromThisTile.transform.position.z - 1]);
		}
		else if ((int)fromThisTile.transform.position.z + 1 <= 9
		         && !Tiles[(int)fromThisTile.transform.position.x, (int)fromThisTile.transform.position.z + 1].GetComponent<Tile>().isTransforming)
		{
			// One is right
			foundTile = true;
			return(Tiles[(int)fromThisTile.transform.position.x, (int)fromThisTile.transform.position.z + 1]);
		}
		
		if (!foundTile)
		{
			// If we didn't find one left or right, check up/down
			// See if there's one above/below this cube that isn't transformed at the moment
			if ((int)fromThisTile.transform.position.x + 1 <= 9
			    && !Tiles[(int)fromThisTile.transform.position.x + 1, (int)fromThisTile.transform.position.z].GetComponent<Tile>().isTransforming)
			{
				// One is above
				foundTile = true;
				return(Tiles[(int)fromThisTile.transform.position.x + 1, (int)fromThisTile.transform.position.z]);
			}
			else if ((int)fromThisTile.transform.position.x - 1 >= 0
			         && !Tiles[(int)fromThisTile.transform.position.x - 1, (int)fromThisTile.transform.position.z].GetComponent<Tile>().isTransforming)
			{
				// One is below
				foundTile = true;
				return(Tiles[(int)fromThisTile.transform.position.x - 1, (int)fromThisTile.transform.position.z]);
			}
		}

		return null;
	}
}






