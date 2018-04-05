using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour {
	static public Hero S; // Singleton
	// These fields control the movement of the ship
	public float gameRestartDelay = 2f;
	public GameObject projectilePrefab;
	public Weapon[] weapons;
	public float projectileSpeed = 40;
	public float speed = 30;
	public float rollMult = -45;
	public float pitchMult = 30;
	// Ship status information
	private float _shieldLevel = 1;
	private GameObject lastTriggerGo = null;
	public delegate void WeaponFireDelegate();
	public WeaponFireDelegate fireDelegate;
	public static Bounds bounds;
	public bool ____________________________;
	public Text currentScore;
	public Text highScore;
	public Text nukeText;
	public Text levelText;
	public Text pauser;
	public static int highScoreInt;
	public bool hasSpread;
	public bool hasPause;
	public static string nukeAvailability = "Not Ready";
	public void setCurrentText(){
		currentScore.text = "Current Score: " + Enemy.TOTAL_POINTS.ToString ();
		setHighText ();
	}
	public void setPauseText(){
		if (Hero.S.hasPause) {
			pauser.text = "Pause Ready";
		} else {
			pauser.text = "Pause Not Ready";
		}
	}

	public  void setHighText(){
		if ((Enemy.TOTAL_POINTS > highScoreInt)&&(Enemy.TOTAL_POINTS != 0)) {
			highScoreInt = Enemy.TOTAL_POINTS;
		}
		highScore.text = "High Score: " + highScoreInt;

	}


	public void setLevelText() {
		levelText.text = "Level: " + Main.S.level;
	}
		
	public void setNukeText(){
		
		nukeText.text = "Nuke: " + nukeAvailability;
	}

	// Use this for initialization

	void Start () {
		Main.S.level = 1;	
		
		setCurrentText();

		setHighText ();

		setLevelText();

		nukeAvailability = "Not Ready";
		setNukeText ();


		if(S==null){
			S = this; // Set the Singleton
		}
		S.shieldLevel = 4;
		bounds = Utils.CombineBoundsOfChildren(this.gameObject);
		ClearWeapons ();
		weapons [0].SetType (WeaponType.blaster);

	}
	
	// Update is called once per frame
	void Update () {
			// Pull in information from the Input class
			float xAxis = Input.GetAxis("Horizontal"); // 1
			float yAxis = Input.GetAxis("Vertical"); // 1
			// Change transform.position based on the axes
			Vector3 pos = transform.position;
			pos.x += xAxis * speed * Time.deltaTime;
			pos.y += yAxis * speed * Time.deltaTime;
			transform.position = pos;

		bounds.center = transform.position;
		Vector3 off = Utils.ScreenBoundsCheck (bounds, BoundsTest.onScreen);
		if (off != Vector3.zero) {
			pos -= off;
			transform.position = pos;
		}
			// Rotate the ship to make it feel more dynamic // 2
			transform.rotation = Quaternion.Euler(yAxis*pitchMult,xAxis*rollMult,0);
		//Allow the ship to fire
		//if(Input.GetKeyDown(KeyCode.Space)){
		//	TempFire ();
		//}
		if (Input.GetAxis ("Jump") == 1 && fireDelegate != null) {
			fireDelegate ();
		}


		}
	/*void TempFire(){
		GameObject projGO = Instantiate<GameObject> (projectilePrefab);
		projGO.transform.position = transform.position;
		Rigidbody rigidB = projGO.GetComponent<Rigidbody> ();
		rigidB.velocity = Vector3.up * projectileSpeed;

		Projectile proj = projGO.GetComponent<Projectile> ();
		proj.type = WeaponType.blaster;
		float tSpeed = Main.GetWeaponDefinition (proj.type).velocity;
		rigidB.velocity = Vector3.up * tSpeed;

	}*/

	void OnTriggerEnter(Collider other) {
			GameObject go = Utils.FindTaggedParent(other.gameObject);
			
			if (go != null) {
				// Make sure it's not the same triggering go as last time
				if (go == lastTriggerGo) { // 2
					return;
				}
				lastTriggerGo = go; // 3
			if (go.tag == "ProjectileEnemy") {
				shieldLevel = shieldLevel - 1;
				print ("hero colided w projectile");
				Destroy (go);
			}

			if (go.tag == "Enemy") {
				// If the shield was triggered by an enemy
				// Decrease the level of the shield by 1
				_shieldLevel--;
				// Destroy the enemy
				Destroy (go); // 4
			}else if (go.tag == "PowerUp") {
				
				AbsorbPowerUp (go);
			}else {
					print("Triggered: "+go.name); // Move this linehere!
				}
			} 

			else {
				print("Triggered: "+other.gameObject.name);
				}
	}
	public void AbsorbPowerUp(GameObject go){
		//THESE ARE THE CASES FOR POWERUPS IF WE WANT TO CHANGE THEM LATER 

		PowerUp pu = go.GetComponent<PowerUp> ();
		switch (pu.type) {
		case WeaponType.shield:
			shieldLevel++;
			break;
		case WeaponType.blaster:
			print ("GOT A BLASTER");
			Hero.S.hasPause = true;
			Hero.S.setPauseText ();
			break;
		default:
			hasSpread = true;
			break;
		}
		pu.AbsorbedBy (this.gameObject);
	}
	public float shieldLevel {
		
		get {
			if (_shieldLevel < 0) {
				Destroy(this.gameObject);
				// Tell Main.S to restart the game after a delay
				Main.S.DelayedRestart(gameRestartDelay);
			}
			return(_shieldLevel); // 1
		}

		set {
			print ("shield level: " + _shieldLevel);
			_shieldLevel = Mathf.Min (value, 4); // 2
			// If the shield is going to be set to less than zero
			if (value < 0) {
				Destroy(this.gameObject);
				// Tell Main.S to restart the game after a delay
				Main.S.DelayedRestart(gameRestartDelay);
			}
		}
	}
	Weapon GetEmptyWeaponSlot(){
		for (int i = 0; i < weapons.Length; i++) {
			if (weapons[i].type == WeaponType.none) {
				return(weapons [i]);

			}
		}
		return(null);
	}
	void ClearWeapons(){
		foreach (Weapon w in weapons) {
			w.SetType (WeaponType.none);
		}
	}
}
