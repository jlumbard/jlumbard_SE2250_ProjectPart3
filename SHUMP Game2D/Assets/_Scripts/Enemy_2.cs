using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy {
	public float lastShotTime;
	public float delayBetweenShots = 0.1f;
	public GameObject projectilePrefab;
	public Transform PROJECTILE_ANCHOR;


	public int number;
	// Use this for initialization
	void Start () {
		PROJECTILE_ANCHOR = Weapon.PROJECTILE_ANCHOR;
		number = Random.Range(0,2); // Changed 10 -> 2 so its binary (max is exclusive for int)
		health = 10;
		base.setScore ();
		Invoke ("fireBack", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		base.destroyIfOff ();
		







		speed = 20 + 3*Main.S.level;
		if (number == 0) {
			MoveRight ();
			if (this.gameObject.transform.position.x > 25)
				number = 1;
		} else {
			MoveLeft ();
			if (this.transform.position.x < -25)
				number = 0;
		}

	}
	void fireBack(){

		GameObject go = Instantiate <GameObject>(projectilePrefab);
		go.transform.position = gameObject.transform.position;
		go.transform.position = go.transform.position - new Vector3 (0, 10, 0);
		go.tag = "ProjectileEnemy";
		go.layer = LayerMask.NameToLayer ("ProjectileEnemy");
		Projectile p = go.GetComponent<Projectile> ();
		p.rigid.velocity = Vector3.down * 40;


		//if (!gameObject.activeInHierarchy) 
		//	return;
		//float tempp = Time.time - lastShotTime;
		//if ((tempp) < delayBetweenShots){
		//	print ("too little time"+ Time.time+ " and " + lastShotTime+ " eqaualls " + tempp);
		//	return;
		//}
//		GameObject go = Instantiate <GameObject>(projectilePrefab);
//		print ("new projecctile");
//		print ("at" + go.transform.position);
//		go.transform.position = gameObject.transform.position;
//		print ("at" + go.transform.position);
//		go.transform.SetParent (PROJECTILE_ANCHOR, true);
//		Projectile p = go.GetComponent<Projectile> ();
//		p.rigid.velocity = Vector3.down * 40;
//		go.tag = "ProjectileHero";
//		go.layer = LayerMask.NameToLayer ("ProjectileHero");
//		p.type = WeaponType.blaster;
//		lastShotTime = Time.time;
		Invoke ("fireBack", 0.5f);
	}
}
