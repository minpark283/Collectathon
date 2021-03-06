﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_healthbar : MonoBehaviour {

    public GameObject dialogue_box;
    public GameObject conversation_indicator;
    public GameObject health_bar;
    public GameObject gauge_bar;
    public Text interIndicator;
    public Text ability_text;
    public Text CT_count;
    public GameObject passive_icon1;
    private GameObject player;
    private Scr_PlayerControl player_script;
    private Slider actual_bar;
    private Slider actual_gauge;

    private master_script ms;
    // Use this for initialization
    void Start (){
        if (passive_icon1 != null)
        {
            passive_icon1.SetActive(false);
        }
        player = GameObject.FindGameObjectWithTag("Player");
        player_script = player.GetComponent<Scr_PlayerControl>();
		actual_bar = health_bar.GetComponent<Slider>();
        actual_gauge = gauge_bar.GetComponent<Slider>();
        ms = GameObject.FindGameObjectWithTag("MasterScript").GetComponent<master_script>();
        dialogue_box.SetActive(false);
        conversation_indicator.SetActive(false);
        interIndicator.color = Color.clear;
        interIndicator.text = "Press S to enter";
    }

	// Update is called once per frame
	void Update () {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player_script = player.GetComponent<Scr_PlayerControl>();
        }
        actual_bar.value = player_script.health;
        actual_gauge.value = player_script.Ability_gauge();
        CT_count.text = ms.getCT().ToString();

	}
    public void SetAbilityText(int num)
    {
        ability_text.text = "Ability #" + num;
    }
    public void ShowConvIndicator()
    {
        conversation_indicator.SetActive(true);
    }
    public void HideConvIndicator()
    {
        conversation_indicator.SetActive(false);
    }
    public void ShowDialogueBox()
    {
        dialogue_box.SetActive(true);
    }
    public void HideDialogueBox()
    {
        dialogue_box.SetActive(false);
    }
    public void ShowInterIndicator()
    {
      interIndicator.color = Color.black;
    }
    public void HideInterIndicator()
    {
      interIndicator.color = Color.clear;
    }
    public void HidePassive1(){
        passive_icon1.SetActive(false);
    }
    public void ShowPassive1(){
        passive_icon1.SetActive(true);
    }
}
