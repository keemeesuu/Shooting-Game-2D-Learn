using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{

    public float maxShotDelay;
    public float curShotDelay;
    public ObjectManager objectManager;

    public Vector3 followPos;
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;
    
    void Awake(){
        parentPos = new Queue<Vector3>();
    }

    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    void Watch(){
        // Queue = FIFO(First Input First Out) 먼저 입력된 데이터가 먼저 나가는 자료구조

        // #.Input Position
        // 부모와 딱 안붙고 거리를 유지하고 싶을때.
        // 부모 위치가 가만히 있으면 저장하지 않도록 조건 추가.
        // 만약 플레이어가 가만히 있는다면은 더이상 집어넣지 말자.
        // Contains함수로 지금 플레이어의 위치를 확인해서 같지 않을경우에만 실행시킨다.
        if(!parentPos.Contains(parent.position)){
            parentPos.Enqueue(parent.position); // 부모가 움직일때마다 부모 포지션값 저장
        }

        // #.Output Position
        // 큐에 설정한 딜레이 값 만큼의 데이터 갯수가 채워지면 그 때부터 반환
        // 딜레이만큼 이전 프레임 위치를 자식오브젝트에 적용
        if(parentPos.Count > followDelay){
            followPos = parentPos.Dequeue();
        }
        // 큐가 딜레이 값 이상으로 채워지기 전까지는 부모 위치값
        else if(parentPos.Count < followDelay){
            followPos = parent.position;
        }
        
    }

    void Follow(){
        // 최종이동
        transform.position = followPos;
    }

    void Fire(){

        // if(Input.GetButton("Fire1"))
            // return;

        if(curShotDelay < maxShotDelay)
            return;

        // GameObject bullet = Instantiate(bull bulletObjA, transform.position, transform.rotation);
        GameObject bullet = objectManager.MakeObj("BulletFollower");
        bullet.transform.position = transform.position;
        
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up*10, ForceMode2D.Impulse);

        curShotDelay = 0;
    }

    void Reload(){
        curShotDelay += Time.deltaTime;
    }

}
