using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // 프리펩 변수 선언
    public GameObject enemyLPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemySPrefab;

    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;

    public GameObject bulletPlayerAPrefab;
    public GameObject bulletPlayerBPrefab;
    public GameObject bulletEnemyAPrefab;
    public GameObject bulletEnemyBPrefab;
    public GameObject bulletFollowerPrefab;

    // 프리펩을 생성하여 저장할 배열 변수 생성
    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;

    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;
    GameObject[] bulletFollower;

    GameObject[] targetPool;

    void Awake(){ 
        // 초기화
        // 한번에 등장할 개수를 고려하여 배열 길이 할당

        // 첫 로딩 시간 = 장면배치 + 오브젝트 풀 생성

        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[20];

        itemCoin = new GameObject[20];
        itemPower = new GameObject[20];
        itemBoom = new GameObject[20];

        bulletPlayerA = new GameObject[100];
        bulletPlayerB = new GameObject[100];
        bulletEnemyA = new GameObject[100];
        bulletEnemyB = new GameObject[100];
        bulletFollower = new GameObject[100];

        Generate();
    }

    void Generate(){

        // #1.Enemy 
        for(int i=0; i<enemyL.Length; i++){
            enemyL[i] = Instantiate(enemyLPrefab);
            enemyL[i].SetActive(false);
        }
        for(int i=0; i<enemyM.Length; i++){
            enemyM[i] = Instantiate(enemyMPrefab);
            enemyM[i].SetActive(false);
        }
        for(int i=0; i<enemyS.Length; i++){
            enemyS[i] = Instantiate(enemySPrefab);
            enemyS[i].SetActive(false);
        }

        // #2.Item 
        for(int i=0; i<itemCoin.Length; i++){
            itemCoin[i] = Instantiate(itemCoinPrefab);
            itemCoin[i].SetActive(false);
        }
        for(int i=0; i<itemPower.Length; i++){
            itemPower[i] = Instantiate(itemPowerPrefab);
            itemPower[i].SetActive(false);
        }
        for(int i=0; i<itemBoom.Length; i++){
            itemBoom[i] = Instantiate(itemBoomPrefab);
            itemBoom[i].SetActive(false);
        }

        // #3.Bullet
        for(int i=0; i<bulletPlayerA.Length; i++){
            bulletPlayerA[i] = Instantiate(bulletPlayerAPrefab);
            bulletPlayerA[i].SetActive(false);
        }
        for(int i=0; i<bulletPlayerB.Length; i++){
            bulletPlayerB[i] = Instantiate(bulletPlayerBPrefab);
            bulletPlayerB[i].SetActive(false);
        }
        for(int i=0; i<bulletEnemyA.Length; i++){
            bulletEnemyA[i] = Instantiate(bulletEnemyAPrefab);
            bulletEnemyA[i].SetActive(false);
        }
        for(int i=0; i<bulletEnemyB.Length; i++){
            bulletEnemyB[i] = Instantiate(bulletEnemyBPrefab);
            bulletEnemyB[i].SetActive(false);
        }
        for(int i=0; i<bulletFollower.Length; i++){
            bulletFollower[i] = Instantiate(bulletFollowerPrefab);
            bulletFollower[i].SetActive(false);
        }
        
    }

    // 오브젝트 풀에 접근할 수 있는 함수 생성
    public GameObject MakeObj(string type){

        switch(type){
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "ItemCoin":
                targetPool = itemCoin;
                break;
            case "ItemPower":
                targetPool = itemPower;
                break;
            case "ItemBoom":
                targetPool = itemBoom;
                break;
            case "BulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "BulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "BulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "BulletEnemyB":
                targetPool = bulletEnemyB;
                break;
            case "BulletFollower":
                targetPool = bulletFollower;
                break;
        }

        for(int i=0; i < targetPool.Length; i++){
            // 비활성화 되어있다면
            if(!targetPool[i].activeSelf){
                // 활성화 후, 반환
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string type) {

        switch(type){
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "ItemCoin":
                targetPool = itemCoin;
                break;
            case "ItemPower":
                targetPool = itemPower;
                break;
            case "ItemBoom":
                targetPool = itemBoom;
                break;
            case "BulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "BulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "BulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "BulletFollower":
                targetPool = bulletFollower;
                break;
        }
        
        return targetPool;
    }
}
