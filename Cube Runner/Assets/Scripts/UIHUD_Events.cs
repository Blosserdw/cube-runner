using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHUD_Events : MonoBehaviour {

	private int score = 0;
	public Text scoreText;
	public GameObject tryAgainScreen = null;

	// Use this for initialization
	void Start ()
	{
		tryAgainScreen.SetActive(false);

		// Initialize score label
		if (GameManager.Instance != null)
			scoreText.text = "0 / " + (GameManager.Instance.pickupsPerSide * 6).ToString();
	}

	public void AddThisToScore(int numToAdd)
	{
		score += numToAdd;
		scoreText.text = score.ToString() + " / " + (GameManager.Instance.pickupsPerSide * 6).ToString();
		UIManager.Instance.CheckScore(score);
	}

	public void ResetScore()
	{
		score = 0;
		scoreText.text = score.ToString() + " / " + (GameManager.Instance.pickupsPerSide * 6).ToString();
	}

	public void TryAgain()
	{
		tryAgainScreen.SetActive(false);
		GameManager.Instance.GameOver();
	}
}
