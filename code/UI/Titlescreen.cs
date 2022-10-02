using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Titlescreen : MonoBehaviour {

	private float score;
	private Text text;

	void Start() {
		text = transform.GetChild(2).gameObject.GetComponent<Text>();

		score = PlayerPrefs.GetFloat("score");
		
		int seconds = (int)score % 60;
		int minutes = (int)score / 60;

		string str = "Highscore:\t\t" + minutes + ":";
		if (seconds < 10) str += "0" + seconds;
		else              str += seconds;

		text.text = str;
	}

	public void Play() {
		SceneManager.LoadScene(1);
	}
	public void Quit() {
		Application.Quit();
	}
}
