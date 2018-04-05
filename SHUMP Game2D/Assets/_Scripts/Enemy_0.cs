using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy_0 : Enemy {
	
	public int number; // Lewis made unstatic so enemies can have diff directions at same time


	// Use this for initialization
	void Start () {
		number = Random.Range(0,2); // Changed 10 -> 2 so its binary (max is exclusive for int)
		health = 3;
		base.setScore ();
		base.powerUpDropChance = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		base.destroyIfOff ();
		speed = 20 + 3 * Main.S.level;
		MoveDown ();
		if (Main.S.level > 3) {
			speed =  3 * Main.S.level; // Level 4 and on has enemy_0 at slope 2
			MoveDown ();
		}
		if (number == 0) 
			MoveRight ();
		 else
			MoveLeft ();
	}
}
