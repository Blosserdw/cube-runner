using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void OnGameStart()
	{
		Debug.Log("Loading level...");
		UIManager.Instance.DestroyMainMenu();
		Application.LoadLevelAdditive("mainScene");
		UIManager.Instance.LoadHUD();
	}

	public void OnCredits()
	{
		//Application.LoadLevel("mainScene");
		Debug.Log("Displaying credits...");
	}
}
