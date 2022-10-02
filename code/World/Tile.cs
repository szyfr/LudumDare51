using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    
	private float health   = max_health;
	private bool  selected = false;

	const float max_health = 2000;
    
    void Start() { }
    void Update() {
		Color col = gameObject.GetComponent<MeshRenderer>().material.color;
		
		if (selected) gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
		else {
			gameObject.GetComponent<MeshRenderer>().material.color = new Color(
				Color.white.r * ((health / max_health)),
				Color.white.g * ((health / max_health)),
				Color.white.b * ((health / max_health)),
				Color.white.a
			);
		}

		if (health <= 0) Destroy(gameObject);
	}

	public void TileSelected()    => selected = true;
	public void DecrementHealth() => health--;
	public void SevereDecrementHealth() => health -= 100;
}
