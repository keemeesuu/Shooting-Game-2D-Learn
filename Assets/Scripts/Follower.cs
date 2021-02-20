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

        // #.Input Pos
        // 움직일때마다 포지션값이 변경됨으로 요소여부확인(Contains)이 된다.
        if(!parentPos.Contains(parent.position)){
            parentPos.Enqueue(parent.position);
                // Enqueue() : 큐에 데이터 저장하는 함수

            // Debug.Log(parentPos.Count);
        }
        
        // Debug.Log(parentPos.Count + ", " + followDelay);
        
        // #.Output Pos
        // 큐에 일정 데이터 갯수가 채워지면 그 때부터 반환하도록 작성
        // 딜레이만큼 이전 프레임 위치를 보조무기에 적용
        if(parentPos.Count > followDelay){
            followPos = parentPos.Dequeue();
                // Dequeue() : 큐의 첫 데이터를 빼면서 반환하는 함수
        }else if(parentPos.Count < followDelay){
            // 큐가 채워지기 전까진 부모 위치 적용
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
