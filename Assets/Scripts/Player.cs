using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchLeft;
    public bool isTouchRight;

    public int hp;
    public int score;
    public float speed;
    public int maxPower;
    public int power;
    public int maxBoom;
    public int boom;

    public float maxShotDelay;
    public float curShotDelay;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject boomEffect;

    public GameManager manager;
    public bool isHit;
    public bool isBoomTime;

    Animator anim;

    void Awake(){
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Fire();
        Boom();
        Reload();
    }

    private void Move(){

        float h = Input.GetAxisRaw("Horizontal");
        // Border Limit
        if((isTouchLeft && h == -1) || (isTouchRight && h == 1)){
            h = 0;
        }
        // Border Limit 
        float v = Input.GetAxisRaw("Vertical");
        if((isTouchBottom && v == -1) || (isTouchTop && v == 1)){
            v = 0;
        }

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        // Player Move
        transform.position = curPos + nextPos;

        // 생략가능해보임
        /*
        if(Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal")){
            anim.SetInteger("Input", (int)h);
        }*/
        anim.SetInteger("Input", (int)h);

    }

    void Fire(){

        // if(Input.GetButton("Fire1"))
            // return;

        if(curShotDelay < maxShotDelay)
            return;

        switch(power){
            case 1:
                // Instantiate() : 매개변수 오브젝트를 생성하는 함수 
                GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up*10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletR = Instantiate(bulletObjA, transform.position+Vector3.right * 0.1f, transform.rotation);
                GameObject bulletL = Instantiate(bulletObjA, transform.position+Vector3.left * 0.1f, transform.rotation);
                
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up*10, ForceMode2D.Impulse);
                
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidL.AddForce(Vector2.up*10, ForceMode2D.Impulse);
                break;
            case 3:
                GameObject bulletRR = Instantiate(bulletObjA, transform.position+Vector3.right * 0.4f, transform.rotation);
                GameObject bulletCC = Instantiate(bulletObjB, transform.position, transform.rotation);
                GameObject bulletLL = Instantiate(bulletObjA, transform.position+Vector3.left * 0.4f, transform.rotation);
                
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up*10, ForceMode2D.Impulse);

                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                rigidCC.AddForce(Vector2.up*10, ForceMode2D.Impulse);
                
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidLL.AddForce(Vector2.up*10, ForceMode2D.Impulse);
                break;
        }

        curShotDelay = 0;
    }

    void Boom(){

        if(Input.GetButton("Fire1")){

            if(isBoomTime)
                return;
        
            if(boom == 0)
                return;

            boom--;
            isBoomTime = true;
            manager.UpdateBoomIcon(boom);

            // #1.Effect visible
            boomEffect.SetActive(true);
            Invoke("OffBoomEffect", 4f);

            // #2.Remove Enemy
                // FindGameObjectWithTag : 태그로 장면의 모든 오브젝트를 추출
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for(int i = 0; i < enemies.Length; i++){
                Enemy enemyLogic = enemies[i].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }

            // #3.Remove Enemy Bullet
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
            for(int i = 0; i < bullets.Length; i++){
                Destroy(bullets[i]);
            }
   
        }
    }

    void Reload(){
        curShotDelay += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Border"){
            switch(collision.gameObject.name){
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
            }
        }else if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet"){
            
            if(isHit)
                return;

            isHit = true;

            hp--;
            manager.UpdateHpIcon(hp);
            if(hp == 0){
                manager.GameOver();
            }else{
                manager.RespawnPlayer();
            }
            gameObject.SetActive(false);
            Destroy(collision.gameObject);

        }else if(collision.gameObject.tag == "Item"){
            Item item = collision.gameObject.GetComponent<Item>();
            switch(item.type){
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if(maxPower <= power)
                        score += 500;
                    else
                        power++;
                    break;
                case "Boom":
                    if(maxBoom <= boom){
                        score += 500;
                    }else{
                        boom++;
                        manager.UpdateBoomIcon(boom);
                    }
                    break;
            }
            Destroy(collision.gameObject);
        }
    }

    // Boom Disable
    void OffBoomEffect(){
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Border"){
            switch(collision.gameObject.name){
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
            }
        }

    }

}
