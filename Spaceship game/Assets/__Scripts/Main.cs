using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	[System.Serializable]
	public class Horde {
		public GameObject[] enemies;
		public int[] amountEnemies;
		public float[] timeToWait;
	}



	static public Main S;
	static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;
	public GameObject[] prefabEnemies;
	public float enemySpawnPerSecond = 0.5f; // # Enemies/second
	public float enemySpawnPadding = 1.5f; // Padding for position
	public WeaponDefinition[] weaponDefinitions;
	public GameObject prefabPowerUp;
	public WeaponType[] powerUpFrequency = new WeaponType[] {
		WeaponType.blaster, WeaponType.blaster,
		WeaponType.spread,
		WeaponType.shield };
	public bool ________________;
	public float enemySpawnRate; // Delay between Enemy spawns
	public WeaponType[] activeWeaponTypes;
	public bool _________________;
	public Horde horde;
	private bool moreEnemies = true;
	private int iterator;
	private float time;
	public int level;
	public bool endGame;

	void Awake() {
		S = this;
		// Set Utils.camBounds
		Utils.SetCameraBounds(this.GetComponent<Camera>());
		// 0.5 enemies/second = enemySpawnRate of 2
		enemySpawnRate = 1f/enemySpawnPerSecond;
		// Invoke call SpawnEnemy() once after a 2 second delay
		InvokeRepeating( "SpawnEnemy", enemySpawnRate, 1 );

		// A generic Dictionary with WeaponType as the key
		W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
		foreach( WeaponDefinition def in weaponDefinitions ) {
			W_DEFS[def.type] = def;
		}
	}

	static public WeaponDefinition GetWeaponDefinition( WeaponType wt ) {
		// Check to make sure that the key exists in the Dictionary
		// Attempting to retrieve a key that didn't exist, would throw an error,
		// so the following if statement is important.
		if (W_DEFS.ContainsKey(wt)) {
			return( W_DEFS[wt]);
		}
		// This will return a definition for WeaponType.none,
		// which means it has failed to find the WeaponDefinition
		return( new WeaponDefinition() );
	}

	void Start() {

		activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
		for ( int i=0; i<weaponDefinitions.Length; i++ ) {
			activeWeaponTypes[i] = weaponDefinitions[i].type;
		}
		StartCoroutine (popUp("Level" + level.ToString(), 1));
	}

	public IEnumerator popUp(string text, int time){
		this.GetComponent<Pop_Up>().Create (text,time);
		yield return 0;
	}

	public void SpawnEnemy() {
		if (iterator == horde.enemies.Length) { // If finish the horde, cancel the invoke repeating
			CancelInvoke("Spawn");
			moreEnemies = false;
			return;
		}
		
		if (time <= 0) { // Create a enemy 
			// Position the Enemy above the screen with a random x position
			Vector3 pos = Vector3.zero;
			float xMin = Utils.camBounds.min.x+enemySpawnPadding;
			float xMax = Utils.camBounds.max.x-enemySpawnPadding;
			pos.x = Random.Range( xMin, xMax );
			pos.y = Utils.camBounds.max.y + enemySpawnPadding;

			Instantiate (horde.enemies[iterator], pos, horde.enemies[iterator].GetComponent<Transform>().rotation);
			time = horde.timeToWait[iterator];
			horde.amountEnemies[iterator]--;
		}
		
		if (horde.amountEnemies[iterator] == 0){ // if we haven't got more enemies, go to the next type of enemy 
			iterator++;
		}
	}

	public void DelayedRestart( float delay ) {
		// Invoke the Restart() method in delay seconds
			Invoke("Restart", delay);
	}

	public void Restart() {
		// Reload _Scene_0 to restart the game
		Application.LoadLevel("_Scene_0");
	}

	public void ShipDestroyed( Enemy e ) {
		// Potentially generate a PowerUp
		if (Random.value <= e.powerUpDropChance) {
			// Random.value generates a value between 0 & 1 (though never == 1)
			// If the e.powerUpDropChance is 0.50f, a PowerUp will be generated
			// 50% of the time. For testing, it's now set to 1f.
			// Choose which PowerUp to pick
			// Pick one from the possibilities in powerUpFrequency
			int ndx = Random.Range(0,powerUpFrequency.Length);
			WeaponType puType = powerUpFrequency[ndx];
			// Spawn a PowerUp
			GameObject go = Instantiate( prefabPowerUp ) as GameObject;
			PowerUp pu = go.GetComponent<PowerUp>();
			// Set it to the proper WeaponType
			pu.SetType( puType );
			// Set it to the position of the destroyed ship
			pu.transform.position = e.transform.position;
		}
	}

	void Update (){
		if (time > 0){
			time -= Time.deltaTime;
		}
		GameObject[] go = GameObject.FindGameObjectsWithTag ("Enemy");

		if (!moreEnemies && go.Length == 0) {
			Invoke("goNextLevel", 1.5f);
		}
	}

	void goNextLevel(){
		if (endGame) {
			GetComponent<Change_Scene>().ChangeToScene(1); //Credits
		} 
		else {
			// Level 1 --> 2
			// Level 2 --> 3
			// By this the number of the next scene is this.level+2
			GetComponent<Change_Scene>().ChangeToScene(level+2);
		}
	}

}
