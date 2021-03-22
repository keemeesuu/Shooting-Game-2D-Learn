using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public int stage;
    public int maxStage;

    public Animator stageAnim;
    public Animator clearAnim;
    public Animator fadeAnim;
    public Transform PlayerPos;

    // public GameObject[] enemyObjs;
    public string[] enemyObjs;
    public Transform[] spawnPoints;
    
    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] hpImage;
    public Image[] boomImage;
    public GameObject gameOverSet;
    public ObjectManager objectManager;

    public List<Spawn> spawnList;
    // Spawn에 대한 구조체가 담겨있는 List

    public int spawnIndex;
    public bool spawnEnd;

    void Awake(){
        spawnList = new List<Spawn>();
        enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL", "EnemyB" };
        StageStart();
    }

    public void StageStart(){
        Debug.Log("StageStart");

        // #.Stage UI Load
        stageAnim.SetTrigger("On");
        stageAnim.GetComponent<Text>().text = "Stage " + stage + "\nStart";

        // #.Enemy Spawn File Read 
        ReadSpawnFile();

        // #.Fade In
        fadeAnim.SetTrigger("In");

    }
    
    public void StageEnd(){
        Debug.Log("StageEnd");

        // #.Clear UI Load
        clearAnim.SetTrigger("On");
        clearAnim.GetComponent<Text>().text = "Stage " + stage + "\nClear";

        // #.Fade Out
        fadeAnim.SetTrigger("Out");

        // #.Player Repos
        player.transform.position = PlayerPos.position;

        // #.Stage Increament
        stage++;
        Debug.Log("stage : " + stage);
        if(stage > maxStage){
            // 구현된 스테이지를 넘기면 게임 오버로 재실행 하도록 흐름 설정
            Invoke("GameOver", 6);
        }else{
            Debug.Log("StageStart");
            Invoke("StageStart", 5);
        }

    }

    void ReadSpawnFile(){
        // #1.변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // #2.리스폰 파일 읽기
        // 스테이지 변수를 통한 적 배이 파일을 로드
        Debug.Log("Stage " + stage);
        TextAsset textFile = Resources.Load("Stage " + stage) as TextAsset;
            // stage.ToString()을 써야하지만 묵시적 형변환으로 안써도 된다
        StringReader stringReader = new StringReader(textFile.text);

        while(stringReader != null){
            string line = stringReader.ReadLine();
            // Debug.Log(line);

            if (line == null)
                break;

            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        // #.텍스트 파일 닫기 - StringReader로 열어둔 파일은 작업이 끝난 후 꼭 닫기
        stringReader.Close();

        // #.첫번째 스폰 딜레이 적용
        nextSpawnDelay = spawnList[0].delay;

    }


    void Update(){
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > nextSpawnDelay && !spawnEnd){
            SpawnEnemy();
            // nextSpawnDelay = Random.Range(0.5f, 3.0f);
            curSpawnDelay = 0;
        }

        // #.UI Score Update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    void SpawnEnemy(){
        int enemyIndex = 0;
        
        // 기존 적 생성 로직을 구조체를 활용한 로직으로 교체
        switch(spawnList[spawnIndex].type){
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }

        int enemyPoint = spawnList[spawnIndex].point;

        // #.object pooling 작업
        // Instantiate(enemyObjs[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation );
        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

        // 에너미에 게임오브젝트를 넘긴다.
        enemyLogic.gameManager = this;

        if(enemyPoint == 5 || enemyPoint == 6){ //#.Right Spawn
            // enemy.transform.Rotate(Vector3.back * 90);
            // rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else if(enemyPoint == 7 || enemyPoint == 8){ //#.Left Spawn
            // enemy.transform.Rotate(Vector3.forward * 90);
            // rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }
        else{ //#.Front Spawn
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }

        // #.리스폰 인덱스 증가
        spawnIndex++;
        if(spawnIndex == spawnList.Count){
            spawnEnd = true;
            return;
        }

        // #.다음 리스폰 딜레이 갱신
        nextSpawnDelay = spawnList[spawnIndex].delay;

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
        // Debug.Log("count : " + boom); 
        // boomImage[boom].color = new Color(1, 1, 1, 1);

        // #.UI Hp Init Disable
        for (int i = 0; i < 3; i++){
            // Debug.Log("2");
            boomImage[i].color = new Color(1, 1, 1, 0);
        }

        // #.UI Hp Init Active
        for (int i = 0; i < boom; i++){
            // Debug.Log("i : " + i);
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

    public void CallExplosion(Vector3 pos, string type){
        // GameManager에서 생성한 폭발 함수를 플레이어, 적에서 호출 할수 있게함

        // Debug.Log("CallExplosion()");

        GameObject explosion = objectManager.MakeObj("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }

    public void GameOver(){
        gameOverSet.SetActive(true);
    }

    public void GameRetry(){
        SceneManager.LoadScene(0);
    }

}
