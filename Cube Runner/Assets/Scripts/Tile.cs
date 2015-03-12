using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public GameObject artObject = null;
	public bool isTransforming = false;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void OnTriggerEnter()
	{
		if (isTransforming)
		{
			// Determine where the cube is (Up or Down)
			if (transform.position.y > 0.0f)
			{
				// This cube is raised
				Debug.Log("Entered trigger of RAISED tile " + transform.position.x + ", " + transform.position.z);
			}
			else if (transform.position.y < 0.0f)
			{
				// This cube is sunken
				Debug.Log("Entered trigger of SUNKEN tile " + transform.position.x + ", " + transform.position.z);
				//GameManager.Instance.playerObject.GetComponent<PlayerMovement>().stopMovement = true;
			}
		}
	}
}
