﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_cutscene1 : MonoBehaviour {
	public GameObject[] comic;
	private int sprite_count;
	private int index = 0;
	// Use this for initialization
	void Start () {
		sprite_count = comic.Length;
		foreach (GameObject panel in comic){
            panel.SetActive(false);
        }
		comic[0].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp("space"))
		{
			comic[index].SetActive(false);
			index +=1;
			comic[index].SetActive(true);

		}
	}
}