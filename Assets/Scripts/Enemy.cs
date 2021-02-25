using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int enemyScore;
    public float speed;
    public int hp;
    public Sprite[] sprites;
    public float maxShotDelay;
    public float curShotDelay;
    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject itemCoin;
    public GameObject itemPower;
    public GameObject itemBoom;
    public GameObject player;
    public ObjectManager objectManager;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;

    Animator anim;

    // #.패턴 흐름에 필요한 변수 생성
    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        /*
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.down * speed;
        */
        if(enemyName == "B"){
            anim = GetComponent<Animator>();
        }
    }

    // 컴포넌트가 활성화 될 때 호출되는 생명주기 함수
    void OnEnable(){
        switch (enemyName) {
            case "L":
                hp = 10;
                break;
            case "M":
                hp = 5;
                break;
            case "S":
                hp = 1;
                break;
            case "B":
                hp = 1000;
                Invoke("Stop", 2);
                break;
        }
    }

    void Stop(){
        if(!gameObject.activeSelf)
            return;

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }

    void Think(){
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        // Debug.Log("patternIndex : " + patternIndex);

        switch(patternIndex){
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void FireFoward(){
        // Debug.Log("일직선 4발 발사.");

        // #.Fire 4 Bullet Forward
        GameObject bulletL = objectManager.MakeObj("BulletBossA");
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        GameObject bulletLL = objectManager.MakeObj("BulletBossA");
        bulletLL.transform.position = transform.position + Vector3.left * 0.45f;
        GameObject bulletR = objectManager.MakeObj("BulletBossA");
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        GameObject bulletRR = objectManager.MakeObj("BulletBossA");
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;

        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        
        rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        
        // #.Pattern Counting
        curPatternCount++;

        if(curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireFoward", 2);
        else
            Invoke("Think", 3);
        
    }

    void FireShot(){
        // Debug.Log("플레이어 방향으로 샷건");
    
        // #.Fire Shotgun Forward
        for(int i = 0; i < 5; i++){
            GameObject bullet = objectManager.MakeObj("BulletEnemyB");
            bullet.transform.position = transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 6, ForceMode2D.Impulse);
        }

        // #.Pattern Counting
        curPatternCount++;

        if(curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireShot", 2);
        else
            Invoke("Think", 3);
    }

    void FireArc(){
        // Debug.Log("부채모양으로 발사.");
        
        // #.Fire Arc Continue Fire
        GameObject bullet = objectManager.MakeObj("BulletEnemyA");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 10 * curPatternCount/maxPatternCount[patternIndex]), -1);
            // Mathf.Sin() : 삼각함수 sin
            // Mathf.Cos() : 삼각함수 cos // sin이랑 첫 위치만 다르지 전체적인 형태는 같다
            // Mathf.PI() : 원주율 상수 (3.14)
            // 원의 둘레값을 많이 줄수록 빠르게 파형을 그림
            // 발사 횟수를 짝수로 두면 같은 위치에 뿌려짐으로 발사 횟수를 홀수로 두면 불규칙적인 반복으로 플레이어의 이동을 유발시킬수 있다.
        rigid.AddForce(dirVec.normalized * 6, ForceMode2D.Impulse);
    
        // #.Pattern Counting
        curPatternCount++;

        if(curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireArc", 0.15f);
        else
            Invoke("Think", 3);
    }

    void FireAround(){
        // Debug.Log("원 형태로 전체 공격");
        
        // #.Fire Around

        // 불규칙적으로 공격하기 위해
        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount%2==0 ? roundNumA : roundNumB;

        for(int i=0; i < roundNum; i++){
            GameObject bullet = objectManager.MakeObj("BulletBossB");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));
                // 생성되는 총알의 순분을 활용하여 방향 결정
            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);

            // 총알이 퍼지는방향으로 회전
            Vector3 rotVec = Vector3.forward * 360 * i / roundNum + Vector3.forward*90;
            bullet.transform.Rotate(rotVec);
        }

        curPatternCount++;

        if(curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireAround", 0.7f);
        else
            Invoke("Think", 3);
    }

    void Update()
    {
        if(enemyName == "B")
            return;
        
        Fire();
        Reload();
    }

    void Fire(){

        if(curShotDelay < maxShotDelay)
            return;
        
        if(enemyName == "S"){
            // GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            GameObject bullet = objectManager.MakeObj("BulletEnemyA");
            bullet.transform.position = transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 6, ForceMode2D.Impulse);
        }else if(enemyName == "L"){
            GameObject bulletL = objectManager.MakeObj("BulletEnemyB");
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;
            GameObject bulletR = objectManager.MakeObj("BulletEnemyB");
            bulletR.transform.position = transform.position + Vector3.right * 0.3f;

            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);
            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.left * 0.3f);
            rigidL.AddForce(dirVecL.normalized * 6, ForceMode2D.Impulse);
            rigidR.AddForce(dirVecR.normalized * 6, ForceMode2D.Impulse);
        }

        curShotDelay = 0;
    }

    void Reload(){
        curShotDelay += Time.deltaTime;
    }

    public void OnHit(int dmg){
        if(hp <= 0)
            return;
        
        hp -= dmg;

        if(enemyName == "B"){
            anim.SetTrigger("OnHit");
        }else{
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }

        
        if(hp <= 0) {
            Player palyerLogic = player.GetComponent<Player>();
            palyerLogic.score += enemyScore;

            // #.Random Ratio Item Drop
            int ran = enemyName == "B" ? 0 : Random.Range(0, 10);

            if(ran < 3){
                Debug.Log("Not Item");
            }else if(ran < 6){  // Coin 30%
                GameObject itemCoin = objectManager.MakeObj("ItemCoin");
                itemCoin.transform.position = transform.position;
            }else if(ran < 8){  // Power 20%
                GameObject itemPower = objectManager.MakeObj("ItemPower");
                itemPower.transform.position = transform.position;
            }else if(ran < 10){ // Boom 20%
                GameObject itemBoom = objectManager.MakeObj("ItemBoom");
                itemBoom.transform.position = transform.position;
            }

            // Destroy(gameObject);
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
    }

    void ReturnSprite(){
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "BorderBullet" && enemyName != "B"){
            // Destroy(gameObject);
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }else if(collision.gameObject.tag == "PlayerBullet"){
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);

            // Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    { 
        
    }
}
