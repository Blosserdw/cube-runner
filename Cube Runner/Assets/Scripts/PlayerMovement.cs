﻿using UnityEngine;
using System.Collections;
//using Pathfinding;

public class PlayerMovement : MonoBehaviour {

	//private AstarAI astarAIScript = null;
	
	//The calculated path
	//public Path path;
	
	//The AI's speed per second
	public float speed;
	
	public Vector3 direction = new Vector3(0f, 0f, 0f);

	private Rigidbody mRigidBody = null;
	public SphereCollider mySphereCollider = null;

	public bool stopMovement = false;
	public bool isTransferring = false;
	private bool transferred = false;

	public TransferDirection transferThisDirection;

	// Use this for initialization
	void Start ()
	{
		// Initialize variable, why is it saving???...
		stopMovement = false;

		// Grab the rigidbody
		mRigidBody = GetComponent<Rigidbody>();

		// Initialize the direction going up
		direction = new Vector3(speed, mRigidBody.velocity.y, 0f);
	}
	
	// Update is called once per frame
	void Update ()
	{

		//=MOVEMENT CONTROLS WITH WASD====================================================||
		if (GameManager.Instance.currentMap != null)
		{
			if (!isTransferring)
			{
				GameManager.Instance.playerObject.GetComponent<Rigidbody>().useGravity = true;
				// Move Right
				if (Input.GetKeyDown(KeyCode.D))
				{
					// Change direction
					//Debug.Log("Moving Right... (Z Negative)");
					direction.z = -speed;
					direction.x = 0.0f;
				}
				
				// Move left
				if (Input.GetKeyDown(KeyCode.A))
				{
					// Change direction
					//Debug.Log("Moving Left... (Z Positive)");
					direction.z = speed;
					direction.x = 0.0f;
				}
				
				// Move up
				if (Input.GetKeyDown(KeyCode.W))
				{
					// Change direction
					//Debug.Log("Moving Up... (X Positive)");
					direction.x = speed;
					direction.z = 0.0f;
				}
				
				// Move down
				if (Input.GetKeyDown(KeyCode.S))
				{
					// Change direction
					//Debug.Log("Moving Down... (X Negative)");
					direction.x = -speed;
					direction.z = 0.0f;
				}



				// X Check, make sure we don't go off the edge (MIGHT be used later for cube side transferring
				if (transform.position.x >= 5.0f)
				{
					//direction.x = -speed;
					//direction.z = 0.0f;
					isTransferring = true;
					transferThisDirection = TransferDirection.Front;
				}
				else if (transform.position.x <= -5.0f)
				{
					//direction.x = speed;
					//direction.z = 0.0f;
					isTransferring = true;
					transferThisDirection = TransferDirection.Back;
				}
				
				// Z Check
				if (transform.position.z >= 5.0f)
				{
					//direction.z = -speed;
					//direction.x = 0.0f;
					isTransferring = true;
					transferThisDirection = TransferDirection.Left;
				}
				else if (transform.position.z <= -5.0f)
				{
					//direction.z = speed;
					//direction.x = 0.0f;
					isTransferring = true;
					transferThisDirection = TransferDirection.Right;
				}
				
				// After determining direction, move the player
				//mRigidBody.AddForce(direction); // Too slow with momentum
				mRigidBody.velocity = direction;
			}
			else
			{
				if (!transferred)
				{
					transferred = true;
					gameObject.GetComponent<CharacterController>().enabled = false;
					gameObject.GetComponentInChildren<SphereCollider>().enabled = false;
					GameManager.Instance.playerObject.GetComponent<Rigidbody>().useGravity = false;
					
					//Debug.Log("STOOOOOOPPPP!!!!");
					
					// Movement is stopped
					direction.x = 0.0f;
					direction.y = 0.0f;
					direction.z = 0.0f;
					mRigidBody.velocity = direction;
					
					mySphereCollider.enabled = false;
					
					GameManager.Instance.ProcessTransfer(transferThisDirection);
				}
			}
		}
	}

	public void RestartPlayer(TransferDirection thisDirection)
	{
		Debug.Log("RESTARTING PLAYER!!");

		if (thisDirection == TransferDirection.Front)
		{
			direction.x = speed;
			direction.z = 0.0f;
		}
		else if (thisDirection == TransferDirection.Back)
		{
			direction.x = -speed;
			direction.z = 0.0f;
		}
		else if (thisDirection == TransferDirection.Left)
		{
			direction.z = speed;
			direction.x = 0.0f;
		}
		else
		{
			direction.z = -speed;
			direction.x = 0.0f;
		}

		mySphereCollider.enabled = true;
		isTransferring = false;
		transferred = false;
	}
}
