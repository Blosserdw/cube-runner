using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public float rotateXSpeed = 1.0f;
	public float rotateYSpeed = 1.0f;
	public float rotateZSpeed = 1.0f;
	

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		gameObject.transform.Rotate(new Vector3(rotateXSpeed, rotateYSpeed, rotateZSpeed));
	}
}
