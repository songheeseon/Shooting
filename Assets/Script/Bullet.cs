﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg;
    public bool isRotate; // 혼자 도는 총알 

     void Update()
    {
        if (isRotate)
            transform.Rotate(Vector3.forward * 10);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BorderBullet")
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
