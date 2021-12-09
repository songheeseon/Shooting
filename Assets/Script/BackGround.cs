﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    // 배경 스크롤링 

    public float speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;


    float viewHeight;

    private void Awake()
    {

        viewHeight = Camera.main.orthographicSize * 2; // 카메라의 값 x 2를 하면 높이가 나옴 

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        if (sprites[endIndex].position.y < viewHeight*(-1)) 
        {
            // Sprite 다시 사용
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            Vector3 frontSpritePos = sprites[endIndex].localPosition;

            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.up * viewHeight;

            
            // Cursor 인덱스 체인지 
            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = (startIndexSave-1 == -1) ? sprites.Length - 1 : startIndexSave - 1;
            Debug.Log(startIndexSave);

        }
    }
}
