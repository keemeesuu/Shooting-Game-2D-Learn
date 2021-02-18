﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;
    
    public float maxSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] hpImage;
    public Image[] boomImage;
    public GameObject gameOverSet;

    void Update(){
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > maxSpawnDelay){
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3.0f);
            curSpawnDelay = 0;
        }

        // #.UI Score Update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    void SpawnEnemy(){
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 5);
        // int ranPoint = Random.Range(0, 9);
        GameObject enemy = Instantiate(enemyObjs[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation );
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;

        if(ranPoint == 5 || ranPoint == 6){ //#.Right Spawn
            // enemy.transform.Rotate(Vector3.back * 90);
            // rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else if(ranPoint == 7 || ranPoint == 8){ //#.Left Spawn
            // enemy.transform.Rotate(Vector3.forward * 90);
            // rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }
        else{ //#.Front Spawn
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }

    }

    public void UpdateHpIcon(int hp){

        // hpImage[hp].color = new Color(1, 1, 1, 0);
        // #.UI Hp Init Disable
        for (int i = 0; i < 3; i++){
            // Debug.Log("2");
            hpImage[i].color = new Color(1, 1, 1, 0);
        }

        // #.UI Hp Init Active
        for (int i = 0; i < hp; i++){
            // Debug.Log("3");
            hpImage[i].color = new Color(1, 1, 1, 1);
        }
    }
    public void UpdateBoomIcon(int boom){
        Debug.Log("count : " + boom); 
        // boomImage[boom].color = new Color(1, 1, 1, 1);

        // #.UI Hp Init Disable
        for (int i = 0; i < 3; i++){
            // Debug.Log("2");
            boomImage[i].color = new Color(1, 1, 1, 0);
        }

        // #.UI Hp Init Active
        for (int i = 0; i < boom; i++){
            Debug.Log("i : " + i);
            boomImage[i].color = new Color(1, 1, 1, 1);
        }
    }
    


    public void RespawnPlayer(){
        Invoke("RespawnPlayerExe", 2f);
    }

    void RespawnPlayerExe(){
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void GameOver(){
        gameOverSet.SetActive(true);
    }

    public void GameRetry(){
        SceneManager.LoadScene(0);
    }

}