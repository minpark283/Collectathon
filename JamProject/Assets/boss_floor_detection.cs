﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_floor_detection : MonoBehaviour {
    public GameObject boss;
    private bool grounded;
    private int attack_choice;
    private BoxCollider2D myCollider;
	// Use this for initialization
	void Start () {
        grounded = false;
        myCollider = GetComponent<BoxCollider2D>();
	}

	
	// Update is called once per frame
	void Update () {
        //Debug.Log("lel");
        if(attack_choice != 3){
            boss.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            grounded = false;
        }/*
        else if (attack_choice == 3 && myCollider. && !grounded)
        {
            Debug.Log("boss hits ground");
            grounded = true;
            boss.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            boss.SendMessage("Attack");
        }
        */
	}
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("floor detect: " + LayerMask.LayerToName(gameObject.layer));
        //if( && )
        if (attack_choice == 3 && collision.gameObject.layer == 9 && !grounded)
        {
            Debug.Log("boss hits ground");
            grounded = true;
            boss.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            boss.SendMessage("Attack");
        }
    }
}
