﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMemberController : MonoBehaviour {

    private bool handsUp;
    public bool AreHandsUp() {
        return handsUp;
    }

    public KeyCode keyCode;

    public Sprite[] frames = new Sprite[2];

	// Use this for initialization
	void Start () {

    }

    public float putHandsDownCooldown = 1.0f;

    private float handsUpTime = 0.0f;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(keyCode)) {
            if (!handsUp) {
                // Go to hands up frame.
                gameObject.GetComponent<SpriteRenderer>().sprite = frames[1];

                handsUp = true;
            }

            handsUpTime = 0.0f;
        } else {
            

            handsUpTime += Time.deltaTime;

            if (handsUp) {
                if (handsUpTime > putHandsDownCooldown) {
                    // Go to hands down frame.
                    gameObject.GetComponent<SpriteRenderer>().sprite = frames[0];

                    handsUp = false;
                }
            }
        }
	}
}
