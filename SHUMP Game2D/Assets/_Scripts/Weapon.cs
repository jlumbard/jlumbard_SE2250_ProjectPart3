using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponDefinition {

	public WeaponType type = WeaponType.none;
	public string letter;
	public Color color = Color.white;
	public GameObject projectilePrefab;
	public Color projectileColor = Color.white;
	public float damageOnHit = 0;
	public float continuousDamage = 0;
	public float delayBetweenShots = 0;
	public float velocity = 20;
	public Time Itime;

}
public class Weapon : MonoBehaviour {
	
	static public Transform PROJECTILE_ANCHOR;

	[Header ("Set Dynamically")] [SerializeField]
	private WeaponType _type = WeaponType.none;
	public WeaponDefinition def;
	public GameObject collar;
	public float lastShotTime = 0;
	private Renderer collarRend;
	public float tempSpawnRate = 0f ;

	// Use this for initialization
	void Start () {
		collar = transform.Find ("Collar").gameObject;
		collarRend = collar.GetComponent<Renderer> ();
		SetType(_type);

		if (PROJECTILE_ANCHOR == null) {
			GameObject go = new GameObject ("_ProjectileAnchor");
			PROJECTILE_ANCHOR = go.transform;
		}
		GameObject rootGO = transform.root.gameObject;
		if (rootGO.GetComponent<Hero> () != null) {
			rootGO.GetComponent<Hero> ().fireDelegate += Fire;
		}

	}
	public WeaponType type {
		get { return(_type); }
		set{ SetType (value); }
	}

	public void SetType (WeaponType wt){
		_type = wt;
		if (_type == WeaponType.none){
			this.gameObject.SetActive(false);
			return;
		}
		else{
			this.gameObject.SetActive(true);
		}
		def = Main.GetWeaponDefinition(_type);
		collarRend.material.color = def.color;
		lastShotTime = 1; 
	}

	public void Fire(){
		if (!gameObject.activeInHierarchy) 
			return;
		if ((Time.time - lastShotTime) < def.delayBetweenShots){
			return;
		}
		Projectile p;
		Vector3 vel = Vector3.up * def.velocity;
		if(transform.up.y <0){
			vel.y = -vel.y;
		}
		switch(type){
		case WeaponType.blaster:
			p = MakeProjectile ();
			p.rigid.velocity = vel;
			def.delayBetweenShots = 0.5f; //0.3f;
			def.damageOnHit = 2;
			break;
		
		case WeaponType.spread:
			p = MakeProjectile ();
			p.rigid.velocity = vel;
			p = MakeProjectile ();
			p.transform.rotation = Quaternion.AngleAxis (30, Vector3.back);
			p.rigid.velocity = p.transform.rotation * vel;
			p = MakeProjectile ();
			p.transform.rotation = Quaternion.AngleAxis (-30, Vector3.back);
			p.rigid.velocity = p.transform.rotation * vel;
			def.delayBetweenShots = 0.4f;
			def.damageOnHit = 1;
			break;

		case WeaponType.nuke:
			p = MakeNuke ();
			p.rigid.velocity = vel;
			def.delayBetweenShots = 0.3f;
			def.damageOnHit = 2;
			break;
		}
	}

	public Projectile MakeNuke(){
		GameObject go = Instantiate <GameObject>(def.projectilePrefab);
		if (transform.parent.gameObject.tag == "Hero"){
			go.tag = "Nuke";
			go.layer = LayerMask.NameToLayer ("Nuke");
		}
		else{
			go.tag = "ProjectileEnemy";
			go.layer = LayerMask.NameToLayer ("ProjectileEnemy");
		}
		go.transform.position = collar.transform.position;
		go.transform.SetParent (PROJECTILE_ANCHOR, true);
		Projectile p = go.GetComponent<Projectile> ();
		p.type= type;
		lastShotTime = Time.time;
		return(p);
	}

	public Projectile MakeProjectile(){
		GameObject go = Instantiate <GameObject>(def.projectilePrefab);
		if (transform.parent.gameObject.tag == "Hero"){
			go.tag = "ProjectileHero";
			go.layer = LayerMask.NameToLayer ("ProjectileHero");
		}
		else{
			go.tag = "ProjectileEnemy";
			go.layer = LayerMask.NameToLayer ("ProjectileEnemy");
		}
		go.transform.position = collar.transform.position;
		go.transform.SetParent (PROJECTILE_ANCHOR, true);
		Projectile p = go.GetComponent<Projectile> ();
		p.type= type;
		lastShotTime = Time.time;
		return(p);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.B)) {
			type = WeaponType.blaster;
		}
		if (Input.GetKey (KeyCode.V) && Hero.S.hasSpread) {
			type = WeaponType.spread;
		}
		if (Input.GetKey (KeyCode.N) && Enemy.DESTROY_COUNT>=10) {
			type = WeaponType.nuke;
		}

		if (!Enemy.AVAILABLE) {
			type = WeaponType.blaster;
			Enemy.AVAILABLE = true;
			Hero.nukeAvailability = "Not Ready";
			Hero.S.setNukeText ();
		}
	
		if (Input.GetKey (KeyCode.P)) {
			if (Hero.S.hasPause) {
				print ("not paused, pausing.");
				Main.S.isPaused = true;

			}
		}

		if (Input.GetKey (KeyCode.I)) {
			if(((Time.time-lastShotTime)>1.5f)||lastShotTime==0){
			
			print ("I");

			Main.S.isPaused = false;
			Main.S.invokeOnce ();
				lastShotTime = Time.time;
			}
			Hero.S.setPauseText ();
		}
			
}

}
