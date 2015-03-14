using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GameObject titleScreenObject = null;
	public GameObject creditsScreenObject = null;
	public GameObject rotatingCube = null;

	// Use this for initialization
	void Start ()
	{
		titleScreenObject.SetActive(true);
		creditsScreenObject.SetActive(false);
		rotatingCube = GameObject.Find("RotatingCube");
		rotatingCube.SetActive(true);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void OnGameStart()
	{
		Debug.Log("Starting level...");

		UIManager.Instance.switchTo = ScreenSwitch.Game;

		// Play menu cube animation to switch back
		rotatingCube.GetComponent<Animation>().Play("cubeMenuWipe");
	}

	public void OnCredits()
	{
		//Application.LoadLevel("mainScene");
		Debug.Log("Displaying credits...");

		UIManager.Instance.switchTo = ScreenSwitch.Credits;

		// Play menu cube animation to switch to credits
		rotatingCube.GetComponent<Animation>().Play("cubeMenuWipe");
	}

	public void OnBackFromCredits()
	{
		UIManager.Instance.switchTo = ScreenSwitch.Title;

		// Play menu cube animation to switch back
		rotatingCube.GetComponent<Animation>().Play("cubeMenuWipe");
	}

	public void ChangeMenuToCredits()
	{
		// change visibility to switch to credits here
		titleScreenObject.SetActive(false);
		creditsScreenObject.SetActive(true);
	}

	public void ChangeMenuToTitle()
	{
		// change visibility to switch to title here
		titleScreenObject.SetActive(true);
		creditsScreenObject.SetActive(false);
	}
}
