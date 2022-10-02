using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	
	private Rigidbody rb;
	private GameObject shadow;
	private int extraLives = 2;

	private bool canJump  = false;
	private bool grounded = false;

	const float move_multipler =  15;
	const float jump_velocity  = 100;

	
	
	//= Unity callbacks
	void Start() {
		rb     = GetComponent<Rigidbody>();
		shadow = GameObject.Find("Shadow");
	}
	void Update() {
		PlayerControls();
		DropShadow();
	}
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Powerup") {
			Powerup.PUType power = other.gameObject.GetComponent<Powerup>().putype;
			switch (power) {
				case Powerup.PUType.extralife:
					extraLives++;
					break;
			}
			GameObject.Find("WorldController").GetComponent<Map>().DestroyPowerup();
		}
		if (other.gameObject.tag == "Death") {
			if (extraLives == 0) {
				float currentScore     = GameObject.Find("WorldController").GetComponent<Map>().GetTime();
				float currentHighscore = PlayerPrefs.GetFloat("score");

				if (currentScore > currentHighscore) PlayerPrefs.SetFloat("score",currentScore);
				SceneManager.LoadScene(0);
			}

			Vector3 newPosition = GameObject.Find("WorldController").GetComponent<Map>().GrabUnselected();
			transform.position  = new Vector3(
				newPosition.x,
				2,
				newPosition.z
			);
			extraLives--;
		}
	}
	void OnCollisionStay(Collision other) {
		if (other.gameObject.tag == "Ground") {
			canJump  = true;
			grounded = true;
		}
	}
	void OnCollisionExit(Collision other) {
		if (other.gameObject.tag == "Ground") {
			grounded = false;
		}
	}

	//= Functions
	private void PlayerControls() {
		//* Movement
		float moveX = Input.GetAxis("Horizontal");
		float moveY = Input.GetAxis("Vertical");

		if (moveX != 0 || moveY != 0) {
			//? Maybe have wind as a map variable that is checked here?
			Vector3 currentPosition = new Vector3(
				transform.position.x + (moveX / move_multipler),
				transform.position.y,
				transform.position.z + (moveY / move_multipler)
			);
			rb.MovePosition(currentPosition);
		}

		//* Jumping
		if (Input.GetAxis("Jump") != 0 && canJump) {
			canJump  = false;

			if (!grounded) {
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
				rb.AddForce(new Vector3(0.0f, jump_velocity + 200, 0.0f));
			} else {
				rb.AddForce(new Vector3(0.0f, jump_velocity,       0.0f));

				RaycastHit hit;
				bool result = Physics.Raycast(
					transform.position,
					Vector3.down,
					out hit,
					Mathf.Infinity,
					~(1 << 6)
				);
				if (result) hit.collider.gameObject.GetComponent<Tile>().SevereDecrementHealth();
			}
		}
	}

	private void DropShadow() {
		//* Move shadow under player
		shadow.transform.position = new Vector3(
			transform.position.x,
			0.126f,
			transform.position.z
		);

		//* Check if over tile
		RaycastHit hit;
		bool result = Physics.Raycast(
			transform.position,
			Vector3.down,
			out hit,
			Mathf.Infinity,
			~(1 << 6)
		);
		shadow.SetActive(result);

		//* Damaging tile while on it
		if (result && grounded && hit.collider.gameObject.GetComponent<Tile>() != null) hit.collider.gameObject.GetComponent<Tile>().DecrementHealth();
	}

	//= Public functions
	public void Cease() {
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}
	public int GetLives() => extraLives;
}
