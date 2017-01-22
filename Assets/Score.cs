using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	
	public Slider slider;
	
	[HideInInspector]
	public float[] playerScores;
	[HideInInspector]
	public float scoreRatio;
	
	private float score = 5f;
	
	void Start () {
		playerScores = new float[2]{10,10};
	}
	
	public void UpdateScore (int team, float s) {
		
		if (team == 0)
		{
			score += s;
		}
		if (team == 1)
		{
			score -= s;
		}
		
		Debug.Log(s);
		float v = score / 10;
		
		scoreRatio = v;
		slider.value = v;
		
		if (scoreRatio <= 0)
		{
			Debug.Log("A Team One");
		}
		
		if (scoreRatio >= 1)
		{
			Debug.Log("Bees");
		}
		
		Debug.Log(v);
	}
	
	// public void UpdateScore (int team, float score) {
		// // Debug.Log(playerScores.Length);
		// playerScores[team] += score;
		
		// // 25 75 = 0.33
		// // 75 25 = 0.66
		
		
		
		// // float v = playerScores[0] / playerScores[1];
		// float v = playerScores[1] / playerScores[0];
		
		// // if (playerScores[0] > playerScores[1])
		// if (playerScores[1] > playerScores[0])
		// {
			// v = playerScores[0] / playerScores[1];
			// // v = playerScores[1] / playerScores[0];
		// }
		// Debug.Log(v);
		
		// v = 1 - (v/2);
		
		// scoreRatio = v;
		// slider.value = v;
		
		// if (scoreRatio <= 0)
		// {
			// Debug.Log("A Team One");
		// }
		
		// if (scoreRatio >= 1)
		// {
			// Debug.Log("Bees");
		// }
		
		// Debug.Log(v);
	// }
}
