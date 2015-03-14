using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHUD_Events : MonoBehaviour {

	private int score = 0;
	public Text scoreText;

	// Use this for initialization
	void Start ()
	{
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
}
