using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy_1 : Enemy {

	// Use this for initialization
	void Start () {
		speed = Random.Range (5*Main.S.level+10, 5*Main.S.level+20);
		base.powerUpDropChance = 0.2f;

		if (TOTAL_POINTS >= 20 && Main.S.level == 1) {
			Main.S.enemySpawnPerSecond++;
			Main.S.level++;
			Hero.S.setLevelText();
			Utils.SetCameraBounds();
		}

		if (TOTAL_POINTS >= 40 && Main.S.level == 2) {
			Main.S.level++;
			Hero.S.setLevelText();
			Utils.SetCameraBounds();
		}

		if (TOTAL_POINTS >= 60 && Main.S.level == 3) {
			Main.S.enemySpawnPerSecond++;
			Main.S.level++;
			Hero.S.setLevelText();
			Utils.SetCameraBounds();
			Main.S.instantiateExtra ();

		}

		if (TOTAL_POINTS >= 80 && Main.S.level == 4) {
			Main.S.level++;
			Hero.S.setLevelText();
			Utils.SetCameraBounds();
		}

		if (TOTAL_POINTS >= 100 && Main.S.level == 5) {
			Main.S.enemySpawnPerSecond++;
			Main.S.level++;
			Hero.S.setLevelText();
			Utils.SetCameraBounds();
		}

		health = 2;
		base.setScore ();
	}
	
	// Update is called once per frame
	void Update () {
		MoveDown ();

	}



}
