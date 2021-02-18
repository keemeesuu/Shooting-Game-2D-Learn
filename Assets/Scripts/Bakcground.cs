using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bakcground : MonoBehaviour
{
    public float speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;

    float viewHeight;

    void Awake(){
        // 카메라 높이 구하는방법 orthographicSize : orthographic 카메라 Size
        viewHeight = Camera.main.orthographicSize * 2;

        // Debug.Log(sprites[endIndex].position.y);
    }
    
    void Update() 
    {
        Move();
        Scrolling();
    }

    void Move(){
        // 현재 위치
        Vector3 curPos = transform.position;

        // 오브젝트 아래로 이동 계산
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;
    }

    void Scrolling(){

        if(sprites[endIndex].position.y < viewHeight*(-1)){
            // #.Sprite ReUse
            Vector3 topSpritePos = sprites[startIndex].localPosition;
            // Vector3 bottomSpritePos = sprites[endIndex].localPosition; // 맨 아래 스프라이트

            // 현재 보여지고 있는 스프라이트를 로컬좌표를 이용해 topSpritePos의 위로 계속 올린상태를 유지.
            sprites[endIndex].transform.localPosition = topSpritePos + Vector3.up*10;

            // #.Cursor Index Change
            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = (startIndexSave-1 == -1) ? sprites.Length-1 : startIndexSave - 1;
            /*
            endIndex = startIndexSave-1
            예외처리)
                startIndexSave-1 = 0-1
                0 에서 2가 되어야 하는데 startIndexSave-1을 해버리면 -1이 되어 버리니 -1일 경우에는 sprites.Length-1을 해서 2를 가져온다.
            2 > 0 > 1 > 2
            1 > 2 > 0 > 1
            0 > 1 > 2 > 0
            */
        }

    }
    

    // void Update()
    // {
    //     // 현재 위치
    //     Vector3 curPos = transform.position;
    //     // Debug.Log(curPos); = (0.0, 10.0, 0.0)

    //     // 오브젝트 아래로 이동 계산
    //     Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
    //     transform.position = curPos + nextPos;

    //     if(sprites[endIndex].position.y < viewHeight*(-1)){
    //         // #.Sprite ReUse
    //         Vector3 topSpritePos = sprites[startIndex].localPosition;
    //         // Vector3 bottomSpritePos = sprites[endIndex].localPosition; // 맨 아래 스프라이트

    //         // Back Group A,B,C 의 y값은 무한으로 마이너스값이 된다.
    //         // 현재 보여지고 있는 스프라이트를 로컬좌표를 이용해 backSpritePos의 위로 계속 올린상태를 유지.
    //         sprites[endIndex].transform.localPosition = topSpritePos + Vector3.up*10;


    //         // #.Cursor Index Change
    //         int startIndexSave = startIndex;
    //         startIndex = endIndex;
    //         endIndex = (startIndexSave-1 == -1) ? sprites.Length-1 : startIndexSave - 1; // 배열을 넘어가지 않도록 예외 처리
    //     }
    // }

    // 내방식
    // void Update()
    // {
    //     // 무한 배경 이동구현
    //     Vector3 curPos = transform.position;
    //     Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
    //     transform.position = curPos + nextPos;

    //     Debug.Log("y : " + sprites[endIndex].position.y + "vh" + viewHeight*(-1));

    //     // 스프라이트 체인지
    //     if(sprites[endIndex].position.y < viewHeight*(-1)){
    //         /* 
    //         ### 알고리즘 목표 ###
    //         (이전 조건) #. -10 값이 되면

    //         Index
    //         [1] [2] [3] [4]
    //         2 -> 0 -> 1 -> 2 // startIndex
    //         1 -> 2 -> 0 -> 1
    //         0 -> 1 -> 2 -> 0 // endIndex

    //         0.맨 위의 스프라이트를 구해야 한다.
    //         [0].맨 아래(현재) 스프라이트를 맨 위 스프라이트 위로 올려야 한다.(로컬좌표 이용)
    //             - [1] 맨 위의 스프라이트를 구해야 한다.=> [1-1]맨위 스프라이트가 변경될 때마다 변수저장.
    //                 -> [마지막 처리] startIndex값 변경.
    //             - [2] 현재 스프라이트를 맨 위로 올린다.
    //         [3].올리기전 스프라이트 위의 스프라이트가 내려와야하며 그게 첫번째 스프라이트가 되어야 한다.
    //             - 올라간 스프라이트에서 -1을 한다. 예외처리) 0 - 1;
    //                 -> endIndex
    //         */

    //         // [1] 맨 위의 스프라이트 포지션을 가져온다.
    //         Vector3 topPos = sprites[startIndex].localPosition; // 로컬 포지션
    //         Vector3 botPos = sprites[endIndex].position;

    //         // [2] 현재 스프라이트를 맨 위로 올린다.
    //         sprites[endIndex].localPosition = topPos + Vector3.up * 10;

            
    //         // [1-1] 맨 위의 스프라이트가 변경될 때마다 변수 저장
    //         startIndex = endIndex;

    //         // [3] 
    //         // endIndex = endIndex - 1;
    //         /*
    //         0 ? () = 1
    //         1 ? () = 2
    //         2 ? () = 0
    //         ---
    //         0+(1) = 1
    //         1+(1) = 2
    //         2+(1) = 0 // 여기서 예외처리
    //         */
    //         endIndex = endIndex + 1;
    //         if(endIndex == sprites.Length){
    //             endIndex = 0;
    //         }
    //         // 20 -> 01 -> 12 -> 20 > 01


    //     }
    // }

}

