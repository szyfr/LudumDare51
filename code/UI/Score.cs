using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	private Map   map;
	private Text  text;
	
	void Start() {
		GameObject world = GameObject.Find("WorldController");
		map   = world.GetComponent<Map>();
		text  = GetComponent<Text>();
	}

	void Update() {
		int seconds = (int)map.GetTime() % 60;
		int minutes = (int)map.GetTime() / 60;

		string str = minutes + ":";
		if (seconds < 10) str += "0" + seconds + "\n";
		else              str += seconds + "\n";
		str += 10 - (seconds % 10);

		text.text = str;
	}
}
