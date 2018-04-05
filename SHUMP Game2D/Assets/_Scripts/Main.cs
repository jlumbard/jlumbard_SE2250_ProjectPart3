using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum WeaponType {
	none,
	blaster,
	spread,
	phaser,
	missile,
	laser,
	shield,
	nuke
}

public class Main : MonoBehaviour {
	//PULL UP DRIPPY
	public GameObject[] prefabEnemies;
	public GameObject extraEnemy;
	public float enemySpawnPerSecond = 0.5f;
	public float enemyDefaultPadding = 1.5f;
	public int level;
	public bool isPaused;

	private BoundsCheck bndCheck;
	static public Main S;
	static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;
	public WeaponDefinition [] weaponDefinitions;
	public GameObject prefabPowerUp;
	public WeaponType[] powerUpFrequency = new WeaponType[] {
		WeaponType.blaster, WeaponType.shield, WeaponType.spread
	};

	public void ShipDestroyed(Enemy e){
		//mamybe generate a powerup?
		if(Random.value <= e.powerUpDropChance){
			print (e.powerUpDropChance.ToString());
			int ndx = Random.Range (0, powerUpFrequency.Length);
			WeaponType puType = powerUpFrequency[ndx];
			GameObject go = Instantiate (prefabPowerUp) as GameObject;
			PowerUp pu = go.GetComponent<PowerUp> ();
			pu.SetType (puType);
			print (puType);
			pu.transform.position = e.transform.position;
		}
	}



	// Use this for initialization
	void Awake(){
		bndCheck = GetComponent<BoundsCheck> ();

		S = this;
		Main.S.isPaused = false;

	

		Invoke ("SpawnEnemy", 1f / enemySpawnPerSecond);





		WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition> ();
		foreach (WeaponDefinition def in weaponDefinitions) {
			WEAP_DICT [def.type] = def;
		}
	
	}
	public void invokeOnce(){
		Invoke ("SpawnEnemy",0);

	}
	public void instantiateExtra(){
		GameObject temp = Instantiate <GameObject>(extraEnemy);
		temp.transform.localScale = (new Vector3 (6f, 6f, 6f));
		temp.transform.Rotate(90,0,0,Space.World);
		print (temp.transform.rotation);
	}

	public void SpawnEnemy(){
		if(!(Main.S.isPaused)){


			
			int ndx = Random.Range (0, prefabEnemies.Length);
			
			GameObject go = Instantiate <GameObject>(prefabEnemies [ndx]);
			float enemyPadding = enemyDefaultPadding;
			if (go.GetComponent<BoundsCheck> () != null) {
				enemyPadding = Mathf.Abs (go.GetComponent<BoundsCheck> ().radius);
			}

			Vector3 pos = Vector3.zero;

			float xMin = -bndCheck.camWidth + enemyPadding;
			float xMax = bndCheck.camWidth - enemyPadding;
			pos.x = Random.Range (xMin, xMax);
			pos.y = bndCheck.camHeight + enemyPadding;
			go.transform.position = pos;
	//		go.transform.position = new Vector3(Random.Range (-50f, 50f), 30 , 0);

			Invoke ("SpawnEnemy", 1f / enemySpawnPerSecond);
		//print ("whats happening");
		}
	}





	void Start () {
		
		Main.S.level = 1;	
		Utils.increaser = 0;
		Utils.SetCameraBounds();
		Enemy.DESTROY_COUNT = 0;
				
	}

	public void DelayedRestart( float delay ) {
		// Invoke the Restart() method in delay seconds
		print("GAME RESTARTING!");
		Invoke("Restart", delay);
	}
	public void Restart() {
		// Reload _Scene_0 to restart the game
		SceneManager.LoadScene("_Scene_0");
		Enemy.TOTAL_POINTS = 0;

	}
	static public WeaponDefinition GetWeaponDefinition (WeaponType wt){
		if (WEAP_DICT.ContainsKey (wt)) {
			return(WEAP_DICT [wt]);
		}
		return (new WeaponDefinition ());
	}

	void OnDrawGizmos () {
		
		float camWidth = Utils.camBounds.max.x - Utils.camBounds.min.x;
		float camHeight = Utils.camBounds.max.y - Utils.camBounds.min.y;
		//if (!Application.isPlaying) return;
		//print ("drawing boundaries");
		Vector3 boundSize = new Vector3(camWidth, camHeight, 10f);
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(Vector3.zero, boundSize);
	}

	
	

	
	// Update is called once per frame
	void Update () {
		if(Main.S.isPaused){
			print ("PAUSED!");
	}
}
}
