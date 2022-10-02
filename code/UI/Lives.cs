using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour {
    
	private PlayerController player;
	private Text text;
    
    void Start() {
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		text   = GetComponent<Text>();
	}
    void Update() {
		string str = "";

		for (int i = 0; i < player.GetLives(); i++) {
			str += "I";
		}

		text.text = str;
	}
}
