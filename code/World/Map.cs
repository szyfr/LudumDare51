using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	//= Public variables
	public GameObject tilePrefab;
	public GameObject powerupPrefab;

	//= Private variables
	//* Map
	private GameObject player;
	private GameObject[] tiles;
	public GameObject powerup;
	private int currentMap = 0;
	private int totalTiles = 0;
	private int[] tilesDest;
	private Texture2D texture;
	private bool mapEnd = false;

	//* Timer
	private float timer        =  0;
	private float totalSeconds =  0;
	private bool  alarm        = false;
	
	//* Constants
	const int alarm_time  = 10;
	const int tile_divide = 5;
	const int total_maps  = 5;
	
	//= Unity callbacks
	void Start() {
		player = GameObject.Find("Player");
		TestGenerator();
		tilesDest = new int[tiles.Length / tile_divide];
	}
	void Update() {
		timer        += Time.deltaTime;
		totalSeconds  = timer;

		//* Set alarm off
		if ((int)(totalSeconds % alarm_time) == 0 && !alarm) {
			alarm = true;
			AlarmSetOff();
		}
		//* Reset alarm
		if ((int)(totalSeconds % alarm_time) == 1) alarm = false;

		//* Generate new map
		if (mapEnd) {
			int choice = Random.Range(1, total_maps);

			ClearMap();
			LoadMap(choice);
			mapEnd = false;
		}
	}

	//= Private Functions
	//* Generate map based on input texture number
	private void LoadMap(int map) {
		currentMap = map;
		texture    = Resources.Load<Texture2D>("maps/map_" + map);

		int sizeX  = texture.width;
		int sizeY  = texture.height;
		int total  = sizeX * sizeY;
		tiles      = new GameObject[total];
		totalTiles = 0;

		for (int i = 0; i < total; i++) {
			Color col = texture.GetPixel(i%sizeX, i/sizeX);
			if (col == Color.black) {
				totalTiles++;
				tiles[i] = Instantiate(
					tilePrefab,
					new Vector3(
						(((i%sizeX)  - sizeX/2) - (i/sizeX)) + sizeX/2,
						0,
						(-((i%sizeX) - sizeX/2) - (i/sizeX)) + sizeY/2
					),
					tilePrefab.transform.rotation
				);
			}
			tilesDest = new int[(totalTiles - 1) / 10];
		}

		Vector3 newPosition = GrabUnselected();
		player.transform.position  = new Vector3(
			newPosition.x,
			2,
			newPosition.z
		);
	}
	private void ClearMap() {
		foreach (GameObject go in tiles) {
			Destroy(go);
		}
		Destroy(powerup);
		powerup = null;
	}

	//* Select the next tiles to destroy
	private void SelectNextTiles() {
		if (GetTileCount() <= 1) {
			mapEnd = true;
			return;
		}

		int count = 0;
		for (int i = 0; i < tilesDest.Length; i++) {
			int choice = Random.Range(0, tiles.Length);

			if (GetTileCount() == 1) return;

			if (tiles[choice] != null) {
				tilesDest[i] = choice;
				tiles[choice].GetComponent<Tile>().TileSelected();
				count++;
			} else i--;
		}
	}

	//* Destroy tiles
	private void DestroyTiles() {
		foreach (int i in tilesDest) {
			if (tiles[i] != null) {
				if ((powerup != null) &&
					tiles[i].transform.position.x == powerup.transform.position.x &&
					tiles[i].transform.position.z == powerup.transform.position.z) {
					Destroy(powerup);
					powerup = null;
				}
				Destroy(tiles[i]);
				tiles[i] = null;
			}
		}
	}

	//* Generate powerup
	private void GeneratePowerup() {
		if (powerup != null) return;

		int choice  = 0;
		bool result = false;

		while (true) {
			choice = Random.Range(0, tiles.Length);
			result = CheckSelected(choice);

			if (result) continue;
			else break;
		}

		powerup = Instantiate(
			powerupPrefab,
			new Vector3(
				tiles[choice].transform.position.x,
				0.75f,
				tiles[choice].transform.position.z
			),
			powerupPrefab.transform.rotation
		);
		powerup.GetComponent<Powerup>().Init();
	}

	private bool CheckSelected(int i) {
		if (tiles[i] == null) return true;
		foreach (int o in tilesDest) {
			if (o == i) return true;
		}
		return false;
	}

	private void TestGenerator() {
		LoadMap(1);
	}

	//= Public Functions
	//* Alarm
	public void AlarmSetOff() {
		DestroyTiles();
		SelectNextTiles();
		GeneratePowerup();
	}

	//* Returns time in seconds
//	public int   GetTime() => (int)totalSeconds;
	public float GetTime() => totalSeconds;

	//* Returns if input tile location has tile
	public bool HasTile(int x, int y) {
		Vector3 pos = new Vector3(x, 0, y);

		for (int i = 0; i < tiles.Length; i++) {
			if (tiles[i] == null) continue;
			if (tiles[i].transform.position == pos) {
				return true;
			}
		}
		return false;
	}

	//* Returns the remaining tiles in map
	public int GetTileCount() {
		int count = 0;
		int selected = 0;

		foreach (GameObject go in tiles) {
			if (go != null) count++;
		}

		foreach (int i in tilesDest) {
			if (tiles[i] != null) selected++;
		}

		return count - selected;
	}
	public int GetTileCountOnly() {
		int count = 0;

		foreach (GameObject go in tiles) {
			if (go != null) count++;
		}

		return count;
	}
	public void DestroyPowerup() {
		Destroy(powerup);
		powerup = null;
	}
	public Vector3 GrabUnselected() {
		int choice  = 0;
		bool result = false;

		while (true) {
			choice = Random.Range(0, tiles.Length);
			result = CheckSelected(choice);

			if (result) continue;
			else break;
		}
		return tiles[choice].transform.position;
	}
}
