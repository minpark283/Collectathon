﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_script : MonoBehaviour {
    public GameObject default_pos_L;
    public GameObject default_pos_R;
    public GameObject up_pos;
    public GameObject m_sprite;
    public GameObject r_sprite;
    public GameObject floor_detection;
    public float max_health;
    public float attack_3_drop_spd;
    public float attack_3_move_spd;
    public GameObject cannon_projectile;
    public GameObject lighter_projectile;
    public GameObject spatula_hitbox_L;
    public GameObject spatula_hitbox_R;
    public float move_spd;
    public float spin_atk_spd;
    public int spin_dmg;
    public int attack_interval_lower_bound;
    public int attack_interval_upper_bound;
    public float cannonball_spd;
    public GameObject cannonball_spawn_L;
    public GameObject cannonball_spawn_R;
    private bool moving_L;
    private bool moving_R;
    private CircleCollider2D myCollider;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer m_sr;
    private SpriteRenderer r_sr;
    private Animator m_anim;
    private Animator r_anim;
    private float health;
    private bool attacking;
    private bool moving;
    private bool in_position;
    private bool preparing_atk;
    private bool on_left;
    private bool on_right;
    private GameObject player;
    private int attack_choice; // 0:none, 1:spin, 2:cannon, 3:spatula, 4:lighter 
    private int cannon_fire_count;
    private Vector3 spin_target;

	// Use this for initialization
    void Start () {
        moving_L = false;
        moving_R = false;
        cannon_fire_count = 0;
        attack_choice = 0;
        preparing_atk = false;
        on_left = false;
        on_right = false;
        attacking = false;
        moving = false;
        in_position = false;
        health = max_health;
        myCollider = GetComponent<CircleCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        m_sr = m_sprite.GetComponent<SpriteRenderer>();
        m_anim = m_sprite.GetComponent<Animator>();
        r_sr = r_sprite.GetComponent<SpriteRenderer>();
        r_anim = r_sprite.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        myRigidbody.freezeRotation = true;
	}
	
	// Update is called once per frame
	void Update () {
        if(myRigidbody.velocity.x > 1){
            m_sr.flipX = true;
            r_sr.flipX = true;
        }
        else if(myRigidbody.velocity.x < -1){
            m_sr.flipX = false;
            r_sr.flipX = false;
        }
        if(myCollider.IsTouchingLayers(LayerMask.GetMask("Player"))){
            if (attack_choice == 1)
            {
                player.SendMessage("TakeDMG", spin_dmg);
            }
        }
		/*
		 * if boss is not in position, not attacking, and not moving, move to either L or R position
		 */
        if(!in_position && !attacking && !moving && !preparing_atk){
            MoveToStandbyLocation();
        }
        /*
         * if boss is moving, but not in position or attacking, then it's on its way to the position.
         */ 
        else if(moving && !in_position && !attacking && !preparing_atk){
            //if it has reached the position, or even passing it
            Moving();
        }
        /*
         * if boss is not moving or attacking, but in position, then it's prepared to attack
         */
        else if(!moving && !attacking && in_position && !preparing_atk){
            AttackPrep();
        }
        /*
         * attacking
         */ 

        /*
         * check if it has gone past the player (spin attack)
         */ 
        else if(moving && attacking && !in_position && !preparing_atk){
            if(attack_choice == 1){
                Debug.Log("CheckSpin()");
                CheckSpin();
            }
            else if(attack_choice == 3){
                Debug.Log("CheckSpatula()");
                CheckSpatula();
            }
        }
	}
    private void CheckSpatula(){
        if (on_left)
        {
            if (transform.position.x < default_pos_R.transform.position.x - 0.3f)
            {
                myRigidbody.velocity = Vector2.zero;
                r_anim.Play("Post-Attack3");
            }
        }
        else if (on_right)
        {
            if (transform.position.x > default_pos_R.transform.position.x + 0.3f)
            {
                myRigidbody.velocity = Vector2.zero;
                r_anim.Play("Post-Attack3");
            }
        }
    }
    private void CheckSpin(){
        if(transform.position.x < spin_target.x + 0.3f && transform.position.x > spin_target.x - 0.3f){
            MoveToOppositeStandbyLocation();
        }
    }
    /*
     * this function is called by animation event
     */ 
    private void StartAttack(){
        in_position = false;
        moving = false;
        attacking = false;
        if(attack_choice == 1){
            //play and loop the spinning animation
            Debug.Log("playing attack anim");
            r_anim.Play("Attack1");
        }
        else if(attack_choice == 2){
            //nothing needs to be done here, probably
        }
        else if(attack_choice == 3){
            //Debug.Log("playing attack anim");
            //r_anim.Play("Attack3");
            //move down
            Debug.Log("boss moves down (spatula)");
            myRigidbody.velocity = new Vector2(0, -1.0f * attack_3_drop_spd);
        }
        else if(attack_choice == 4){
            
        }
        attacking = true;
        if(attack_choice != 3){
            Attack();
        }
    }
    private void MoveToStandbyLocation(){
        attack_choice = 0;
        Debug.Log("boss starts moving");
        System.Random rng = new System.Random();
        int temp = rng.Next(0, 2);
        if (temp == 0)
        {
            Debug.Log("boss moves L");
            moving_L = true;
            moving_R = false;
            //go left
            Vector3 dir = default_pos_L.transform.position - transform.position;
            //Debug.Log("dir (b4 normalization): " + dir);
            dir.Normalize();
            //Debug.Log("dir (normolized)" + dir);
            dir *= move_spd;
            //Debug.Log("dir (scaled): " + dir);
            myRigidbody.velocity = new Vector2(dir.x, dir.y);
            //Debug.Log("boss velocity: " + myRigidbody.velocity);

        }
        else
        {
            moving_L = false;
            moving_R = true;
            Debug.Log("boss moves R");
            //go right
            Vector3 dir = default_pos_R.transform.position - transform.position;
            //Debug.Log("dir (b4 normalization): " + dir);
            dir.Normalize();
            //Debug.Log("dir (normolized)" + dir);
            dir *= move_spd;
            //Debug.Log("dir (scaled): " + dir);
            myRigidbody.velocity = new Vector2(dir.x, dir.y);
            //Debug.Log("boss velocity: " + myRigidbody.velocity);
           
        }
        moving = true;
        in_position = false;
        attacking = false;
        preparing_atk = false;
    }
    private void MoveToOppositeStandbyLocation()
    {
        moving = true;
        in_position = false;
        attacking = false;
        preparing_atk = false;
        attack_choice = 0;
        Debug.Log("boss starts moving");
        //System.Random rng = new System.Random();
        if (on_right)
        {
            Debug.Log("boss moves L");
            moving_L = true;
            moving_R = false;
            //go left
            Vector3 dir = default_pos_L.transform.position - transform.position;
            //Debug.Log("dir (b4 normalization): " + dir);
            dir.Normalize();
            //Debug.Log("dir (normolized)" + dir);
            dir *= move_spd;
            //Debug.Log("dir (scaled): " + dir);
            myRigidbody.velocity = new Vector2(dir.x, dir.y);
            //Debug.Log("boss velocity: " + myRigidbody.velocity);

        }
        else
        {
            Debug.Log("boss moves R");
            moving_R = true;
            moving_L = false;
            //go right
            Vector3 dir = default_pos_R.transform.position - transform.position;
            //Debug.Log("dir (b4 normalization): " + dir);
            dir.Normalize();
            //Debug.Log("dir (normolized)" + dir);
            dir *= move_spd;
            //Debug.Log("dir (scaled): " + dir);
            myRigidbody.velocity = new Vector2(dir.x, dir.y);
            //Debug.Log("boss velocity: " + myRigidbody.velocity);

        }
    }
    private void Moving(){
        attack_choice = 0;
        if(!r_anim.GetCurrentAnimatorStateInfo(0).IsName("Boss Idle")){
            Debug.Log("playing idle anim (Moving:())");
            r_anim.Play("Boss Idle");
        }

        if (transform.position.x < default_pos_L.transform.position.x && moving_L)
        {
            Debug.Log("boss stops at L");
            //stop
            moving_L = false;
            moving_R = false;
            myRigidbody.velocity = Vector2.zero;
            moving = false;
            in_position = true;
            on_left = true;
            on_right = false;
            in_position = true;
            r_sr.flipX = true;
            m_sr.flipX = true;
        }
        else if (transform.position.x > default_pos_R.transform.position.x && moving_R)
        {
            //stop
            Debug.Log("boss stops at R");
            moving_L = false;
            moving_R = false;
            myRigidbody.velocity = Vector2.zero;
            moving = false;
            in_position = true;
            on_right = true;
            on_left = false;
            r_sr.flipX = false;
            m_sr.flipX = false;
        }
       

        attacking = false;
        preparing_atk = false;
    }
    private void Attack(){
        //do what needs to be done
        attacking = true;
        preparing_atk = false;
        in_position = false;
        Debug.Log("boss attacks, attack_choice = " + attack_choice);
        if (attack_choice == 1)
        {
            Debug.Log("boss spins");
            //start moving towards player
            spin_target = player.transform.position;
            Vector3 dir = spin_target - transform.position;
            dir.Normalize();
            dir *= spin_atk_spd;
            myRigidbody.velocity = new Vector2(dir.x, dir.y);
            moving = true;
        }
        else if (attack_choice == 2)
        {
            System.Random rng = new System.Random();
            cannon_fire_count = rng.Next(1, 4);
            StartCoroutine(FireCannon());
        }
        else if (attack_choice == 3)
        {
            r_anim.Play("Attack3");
            moving = true;
            if(on_left){
                myRigidbody.velocity = new Vector2(attack_3_move_spd, 0.0f);
            }
            else if(on_right){
                myRigidbody.velocity = new Vector2(-1.0f * attack_3_move_spd, 0.0f);
            }
        }
        else if (attack_choice == 4)
        {
            
        }
    }
    private void AttackPrep(){
        preparing_atk = true;
        Debug.Log("boss preparing for attack");
        StartCoroutine(Prep());
    }
    private IEnumerator FireCannon(){
        Vector3 cannon_target = player.transform.position;
        r_anim.Play("Cannon Firing 2nd Half",-1, 0f);
        if(!r_sr.flipX){
            Vector3 dir = cannon_target - cannonball_spawn_L.transform.position;
            dir.Normalize();
            dir *= cannonball_spd;
            GameObject cannon_clone = Instantiate(cannon_projectile, cannonball_spawn_L.transform.position, transform.rotation);
            cannon_clone.GetComponent<Rigidbody2D>().velocity = new Vector2(dir.x, dir.y);
        }
        else{
            Vector3 dir = cannon_target - cannonball_spawn_R.transform.position;
            dir.Normalize();
            dir *= cannonball_spd;
            GameObject cannon_clone = Instantiate(cannon_projectile, cannonball_spawn_R.transform.position, transform.rotation);
            cannon_clone.GetComponent<Rigidbody2D>().velocity = new Vector2(dir.x, dir.y);
        }
        yield return new WaitForSeconds(1.5f);
        if(cannon_fire_count > 0){
            cannon_fire_count -= 1;
            StartCoroutine(FireCannon());
        }
        else{
            r_anim.Play("Post-Attack2");
        }
    }
    private IEnumerator Prep(){
        System.Random rng = new System.Random();
        int pause_time = rng.Next(attack_interval_lower_bound, attack_interval_upper_bound);
        yield return new WaitForSeconds(pause_time);
        if ((player.transform.position.x < default_pos_R.transform.position.x
            && player.transform.position.x > default_pos_L.transform.position.x))
        {
            //int temp = rng.Next(1, 3);
            //Debug.Log("boss idle for " + temp + " seconds");
            //yield return new WaitForSeconds(temp);
            //int temp = rng.Next(0, 4);
            int temp = 2;
            if (temp == 0)
            {
                //spin
                Debug.Log("playing pre-attack anim (spin)");
                r_anim.Play("Pre-Attack1");
                attack_choice = 1;
            }
            else if (temp == 1)
            {
                //cannon
                Debug.Log("playing pre-attack anim (cannon)");
                r_anim.Play("Pre-Attack2");
                attack_choice = 2;
            }
            else if (temp == 2)
            {
                //spatula
                Debug.Log("playing pre-attack anim (spatula)");
                r_anim.Play("Pre-Attack3");
                attack_choice = 3;
            }
            else if (temp == 3)
            {
                //candle lighter
            }
        }
        else
        {
            Debug.Log("player not in range, suspend");
            StartCoroutine(Prep());
        }
    }
    private void SpatulaAttack(){
        
    }
    public int GetAttackSelection(){
        return attack_choice;
    }
    public void ResetAttackSelection(){
        attack_choice = 0;
    }
}
