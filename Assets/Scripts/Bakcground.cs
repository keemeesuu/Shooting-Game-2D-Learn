using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bakcground : MonoBehaviour
{
    public float speed;
    public int startIndex; // 2
    public int endIndex; // 0 -> 맨 아래 스프라이트(위로 올라갈 스프라이트)
    public Transform[] sprites;

    float viewHeight;

    void Awake(){
        // 카메라 높이 구하는방법 orthographicSize : orthographic 카메라 Size
        viewHeight = Camera.main.orthographicSize * 2;

        Debug.Log(sprites[endIndex].position.y);
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 위치
        Vector3 curPos = transform.position;
        // Debug.Log(curPos); = (0.0, 10.0, 0.0)

        // 오브젝트 아래로 이동 계산
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        // Debug.Log(sprites[endIndex].position.y);
        // -10.1 부터 실행됨        
        if(sprites[endIndex].position.y < viewHeight*(-1)){
            // #.Sprite ReUse
            Vector3 backSpritePos = sprites[startIndex].localPosition; // 맨위 스프라이트
            Vector3 frontSpritePos = sprites[endIndex].localPosition; // 맨 아래 스프라이트

            // Back Group A,B,C 는 y=0 그리고 무한으로 -a값이 된다 하지만 안에 있는 스프라이트 들은 로컬좌표를 이용해 위로 올린다.
            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.up*10;

            // #.Cursor Index Change
            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = (startIndexSave-1 == -1) ? sprites.Length-1 : startIndexSave - 1; // 배열을 넘어가지 않도록 예외 처리
        }
    }
}
