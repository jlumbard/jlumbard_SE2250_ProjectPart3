using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
	public static int TOTAL_POINTS=0;
	protected int speed = 20; // The speed in m/s
	public float fireRate = 0.3f; // Seconds/shot (Unused)
	private float scorePoints;
	public float health = 10;
	public int score = 100; // Points earned for destroying this
	public bool ________________;
	private BoundsCheck bndCheck;
	public Text currentScore;
	public Text highScore;
	protected static int increase0 = 0;
	protected static int increase1 = 0;
	public bool notifiedOfDestruction;
	public float powerUpDropChance = .1f;
	public static int DESTROY_COUNT = 0;
	public static bool AVAILABLE = true;

	// Update is called once per frame



	public Vector3 pos{
		get{
			return(this.transform.position);
		}
		set{
			this.transform.position = value;
		}
	}

	void Awake() {
		bndCheck = GetComponent<BoundsCheck> ();

	}

	void Start(){

	}
	public void setScore(){
		scorePoints = health;
	}


	void Update() {
		Move ();

		if (bndCheck != null && !bndCheck.isOnScreen) {
				
				Destroy (gameObject);
		}
			
	}


	public virtual void Move(){
		Vector3 tempPos = pos;
		tempPos.y -= speed * Time.deltaTime;
		pos = tempPos;
	}
	public virtual void MoveRight() {
		Vector3 tempPos = pos;
		tempPos.x -= -speed * Time.deltaTime * 0.8f; //Slow down angle
		pos = tempPos;
	}
	public virtual void MoveLeft(){
		Vector3 tempPos = pos;
		tempPos.x -= speed * Time.deltaTime * 0.8f; // ^^
		pos = tempPos;
	}
	public virtual void MoveDown(){
		Vector3 TempPos = pos;
		TempPos.y -= speed * Time.deltaTime;
		pos = TempPos;
	}
		

	void OnCollisionEnter(Collision coll){
		GameObject otherGO = coll.gameObject;
		switch (otherGO.tag) {
		case "ProjectileHero":
			Projectile p = otherGO.GetComponent<Projectile> ();

			if (!bndCheck.isOnScreen) {
				Destroy (otherGO);
				break;
			}

			health -= Main.GetWeaponDefinition(p.type).damageOnHit;
			if (health <= 0) {
				if (!notifiedOfDestruction) {
					Main.S.ShipDestroyed (this);
				}
				notifiedOfDestruction = true;

				Destroy (this.gameObject);
				DESTROY_COUNT++;
				TOTAL_POINTS = TOTAL_POINTS + (int)scorePoints;
				Hero.S.setCurrentText ();
				if (DESTROY_COUNT >= 10) {
					Hero.nukeAvailability = "Ready!";
					Hero.S.setNukeText();
				}
			}
			Destroy (otherGO);
			break;

		case "Nuke":
			DestroyAll ("Enemy");
			Destroy (otherGO);
			DESTROY_COUNT = 0;
			AVAILABLE = false;
			Hero.S.setCurrentText ();
			break;

		default:
			print ("Enemy hit by non-ProjectileHero: " + otherGO.name);
			break;
		}
	}
	void DestroyAll(string tag){
		GameObject[] enemies = GameObject.FindGameObjectsWithTag (tag);
		for (int i = 0; i < enemies.Length; i++) {
			print (enemies [i]);
			try{
				TOTAL_POINTS = TOTAL_POINTS + (int)(enemies[i].GetComponent<Enemy>().scorePoints);
			}
			catch(Exception e){
				
			}
			Destroy (enemies [i]);
		}
	}
}
