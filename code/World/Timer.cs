using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

	private float timer        =  0;
	private float totalSeconds =  0;
	private bool  alarm        = false;
	
	
	void Start() {}
	void Update() {
		timer        += Time.deltaTime;
		totalSeconds  = timer % 60;

		//* Set alarm off
		if ((int)(totalSeconds % 10) == 0 && !alarm) {
			alarm = true;
			Debug.Log("Fug");
		}
		//* Reset alarm
		if ((int)(totalSeconds % 10) == 1) {
			alarm = false;
		}
	}

	//* Returns time in seconds
	public int GetTime() => (int)totalSeconds;
}
