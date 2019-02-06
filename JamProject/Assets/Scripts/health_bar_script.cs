﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health_bar_script : MonoBehaviour {
    Vector3 ls;
    public GameObject parent;
    private squash_script script;
    private Scr_PlayerControl script2;
    private float orig_scale;
    private int flag;
	// Use this for initialization
	void Start () {
        flag = 0;
        if (flag == 0)
        {
            script = parent.GetComponent<squash_script>();
            if (script != null)
            {
                flag = 1;
            }
        }
        if (flag == 0)
        {
            script2 = parent.GetComponent<Scr_PlayerControl>();
            if (script2 != null)
            {
                flag = 2;
            }
        }

        ls = transform.localScale;
        orig_scale = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
        if(flag == 1){
            if (script.health_percentage > 0.0f)
            {
                //Debug.Log("squash hp -");
                ls.x = orig_scale * script.health_percentage;
            }
            transform.localScale = ls;
        }
        else if(flag == 2){
            
            if (script2.health_percentage > 0.0f)
            {
                Debug.Log("player hp -");
                ls.x = orig_scale * script2.health_percentage;
            }
            transform.localScale = ls;
        }
	}
}