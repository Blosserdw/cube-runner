using UnityEngine;
using System.Collections;

public class MenuCube : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void ChangeMenu()
	{
		Debug.Log("ANIMATION EVENT HIT");
		UIManager.Instance.SwitchMenu();
	}
}
