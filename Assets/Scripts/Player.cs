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

    // public GameObject bulletObjA;
    // public GameObject bulletObjB;
    public GameObject boomEffect;

    public GameManager gameManager;
    public ObjectManager objectManager;
    public bool isHit;
    public bool isBoomTime;

    public GameObject[] followers;
    public bool isRespawnTime;

    public bool[] joyControl;
    public bool isControl;
    public bool isButtonA;
    public bool isButtonB;

    Animator anim;
    SpriteRenderer spriteRenderer;
    

    void Awake(){
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable(){
        Unbeatable();
        Invoke("Unbeatable", 3);
    }

    void Unbeatable(){
        isRespawnTime = !isRespawnTime;
        if(isRespawnTime){  //#. 무적 타임 이펙트 (투명) 플레이어+팔로워
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);

            for(int i=0; i<followers.Length; i++){
                followers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            }
        }else{  //#. 무적 타임 종료 (원상태로)
            spriteRenderer.color = new Color(1, 1, 1, 1f);
            
            for(int i=0; i<followers.Length; i++){
                followers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            }
        }
    }

    void Update()
    {
        Move();
        Fire();
        Boom();
        Reload();
    }

    public void JoyPanel(int type){
        // 조이스틱 구현
        for(int i=0; i<9; i++){
            joyControl[i] = i == type;
        }
    }

    public void JoyDown(){
        isControl = true;
    }

    public void JoyUp(){
        isControl = false;
    }

    private void Move(){

        // #.Keyboard Control Value
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // #.Joy Control Value
        if(joyControl[0]){ h = -1; v = 1; }
        if(joyControl[1]){ h = 0; v = 1; }
        if(joyControl[2]){ h = 1; v = 1; }
        if(joyControl[3]){ h = -1; v = 0; }
        if(joyControl[4]){ h = 0; v = 0; }
        if(joyControl[5]){ h = 1; v = 0; }
        if(joyControl[6]){ h = -1; v = -1; }
        if(joyControl[7]){ h = 0; v = -1; }
        if(joyControl[8]){ h = 1; v = -1; }

        // Border Limit + Only Joy Control Down 
        if((isTouchLeft && h == -1) || (isTouchRight && h == 1) /*|| !isControl*/){
            h = 0;
        }
        if((isTouchBottom && v == -1) || (isTouchTop && v == 1) /*|| !isControl*/){
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


    public void ButtonADown(){
        isButtonA = true;
    }

    public void ButtonAUp(){
        isButtonA = false;
    }

    public void ButtonBDown(){
        isButtonA = true;
    }

    void Fire(){

        // if(!Input.GetButton("Fire1"))
            // return;

        // #.Joy Control Button Check
        if(!isButtonA)
            return;

        if(curShotDelay < maxShotDelay)
            return;

        switch(power){
            case 1:
                // Instantiate() : 매개변수 오브젝트를 생성하는 함수 
                // #.object pooling 작업
                // GameObject bullet = Instantiate(bull bulletObjA, transform.position, transform.rotation);
                GameObject bullet = objectManager.MakeObj("BulletPlayerA");
                bullet.transform.position = transform.position;
                
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up*10, ForceMode2D.Impulse);

                break;
            case 2:
                GameObject bulletR = objectManager.MakeObj("BulletPlayerA");
                bulletR.transform.position = transform.position+Vector3.right * 0.1f;

                GameObject bulletL = objectManager.MakeObj("BulletPlayerA");
                bulletL.transform.position = transform.position+Vector3.left * 0.1f;

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

                rigidR.AddForce(Vector2.up*10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up*10, ForceMode2D.Impulse);

                break;
            default:
                GameObject bulletRR = objectManager.MakeObj("BulletPlayerA");
                bulletRR.transform.position = transform.position+Vector3.right * 0.4f;
                GameObject bulletCC = objectManager.MakeObj("BulletPlayerB");
                bulletCC.transform.position = transform.position;
                GameObject bulletLL = objectManager.MakeObj("BulletPlayerA");
                bulletLL.transform.position = transform.position+Vector3.left * 0.4f;

                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

                rigidRR.AddForce(Vector2.up*10, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up*10, ForceMode2D.Impulse);
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
            gameManager.UpdateBoomIcon(boom);

            // #1.Effect visible
            boomEffect.SetActive(true);
            Invoke("OffBoomEffect", 4f);

            // #2.Remove Enemy
                // FindGameObjectWithTag : 태그로 장면의 모든 오브젝트를 추출
            // GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] enemiesL = objectManager.GetPool("EnemyL");
            GameObject[] enemiesM = objectManager.GetPool("EnemyM");
            GameObject[] enemiesS = objectManager.GetPool("EnemyS");
            for(int i = 0; i < enemiesL.Length; i++){
                if(enemiesL[i].activeSelf){
                    Enemy enemyLogic = enemiesL[i].GetComponent<Enemy>();
                    enemyLogic.OnHit(1000);
                }
            }
            for(int i = 0; i < enemiesM.Length; i++){
                if(enemiesM[i].activeSelf){
                    Enemy enemyLogic = enemiesM[i].GetComponent<Enemy>();
                    enemyLogic.OnHit(1000);
                }
            }
            for(int i = 0; i < enemiesS.Length; i++){
                if(enemiesS[i].activeSelf){
                    Enemy enemyLogic = enemiesS[i].GetComponent<Enemy>();
                    enemyLogic.OnHit(1000);
                }
            }

            // #3.Remove Enemy Bullet
            GameObject[] bulletsA = objectManager.GetPool("BulletEnemyA");
            GameObject[] bulletsB = objectManager.GetPool("BulletEnemyB");
            for(int i = 0; i < bulletsA.Length; i++){
                if(bulletsA[i].activeSelf){
                    bulletsA[i].SetActive(false);
                }
            }
            for(int i = 0; i < bulletsB.Length; i++){
                if(bulletsB[i].activeSelf){
                    bulletsB[i].SetActive(false);
                }
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
            
            if(isRespawnTime)
                return;

            if(isHit)
                return;

            isHit = true;

            hp--;
            gameManager.UpdateHpIcon(hp);
            gameManager.CallExplosion(transform.position, "P");

            if(hp == 0){
                gameManager.GameOver();
            }else{
                gameManager.RespawnPlayer();
            }
            gameObject.SetActive(false);
            // Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);

        }else if(collision.gameObject.tag == "Item"){
            Item item = collision.gameObject.GetComponent<Item>();
            switch(item.type){
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if(maxPower <= power)
                        score += 500;
                    else {
                        power++;
                        AddFollower();
                    }
                    break;
                case "Boom":
                    if(maxBoom <= boom){
                        score += 500;
                    }else{
                        boom++;
                        gameManager.UpdateBoomIcon(boom);
                    }
                    break;
            }
            // Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }

    void AddFollower(){
        if(power == 4){
            followers[0].SetActive(true);
        }else if(power == 5){
            followers[1].SetActive(true);
        }else if(power == 6){
            followers[2].SetActive(true);
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
