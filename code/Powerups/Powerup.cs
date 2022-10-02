using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
    
	public enum PUType {
		dash,
		extralife,
		teleport,
		forcetile,
		createtile,
	}
	public PUType putype;
    
    void Start() {}
    void Update() {}

	public void Init(int putype) {
		this.putype = (PUType)putype;
		
		switch ((PUType)putype) {
			case PUType.dash:
				gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
				break;
			case PUType.extralife:
				gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
				break;
			case PUType.teleport:
				gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
				break;
			case PUType.forcetile:
				gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
				break;
			case PUType.createtile:
				gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
				break;
		}
	}
	public void Init() {
		int choice  = Random.Range(0, 5);
	//	this.putype = (PUType)choice;
		this.putype = PUType.extralife;
		
		switch (putype) {
			case PUType.dash:
				gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
				break;
			case PUType.extralife:
				gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
				break;
			case PUType.teleport:
				gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
				break;
			case PUType.forcetile:
				gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
				break;
			case PUType.createtile:
				gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
				break;
		}
	}
}
