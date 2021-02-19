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

    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        /*
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.down * speed;
        */
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
        }
    }

    void Update()
    {
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
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);
        
        if(hp <= 0) {
            Player palyerLogic = player.GetComponent<Player>();
            palyerLogic.score += enemyScore;

            // #.Random Ratio Item Drop
            int ran = Random.Range(0, 10);
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
        if(collision.gameObject.tag == "BorderBullet"){
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
