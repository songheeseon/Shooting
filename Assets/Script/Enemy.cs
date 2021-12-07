using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{

    public int enemyScore; // enemy들이 각각 갖는 스코어 변수 
    public string enemyname;
    public float speed;
    public int health;
    public Sprite[] sprites;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject itemCoin;
    public GameObject itemBoom;
    public GameObject itemPower;

    public float maxShotDelay;
    public float curShotDelay;

    public GameObject player;

    public ObjectManager objectManager;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        switch(enemyname)
        {
            case "L":
                health = 40;
                break;
            case "M":
                health = 15;
                break;

            case "S":
                health = 5;
                break;




        }
    }

    void Update()
    {
        Fire();
        Reload();

    }


    public void OnHit(int dmg)
    {
        if (health <= 0)
            return;

        health -= dmg;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            // #. Random Ratio Item Drop
            int ran = Random.Range(0, 10); 

            if(ran < 5)
            {
                Debug.Log("Not Item");
            }else if (ran < 6)
            {
                GameObject itemCoin = objectManager.MakeObj("ItemCoin");
                itemCoin.transform.position = transform.position;
                //Rigidbody2D rigid = itemCoin.GetComponent<Rigidbody2D>();
                //rigid.velocity = Vector2.down * 1.5f;

                //Instantiate(itemCoin, transform.position, itemCoin.transform.rotation);
            }else if (ran < 9) // Power
            {
                GameObject itemPower = objectManager.MakeObj("ItemPower");
                itemPower.transform.position = transform.position;
                //Rigidbody2D rigid = itemPower.GetComponent<Rigidbody2D>();
                //rigid.velocity = Vector2.down * 1.5f;

                //Instantiate(itemPower, transform.position, itemPower.transform.rotation);
            }
            else if (ran < 10) // Boom 
            {
                GameObject itemBoom = objectManager.MakeObj("ItemBoom");
                itemBoom.transform.position = transform.position;
                //Rigidbody2D rigid = itemBoom.GetComponent<Rigidbody2D>();
                //rigid.velocity = Vector2.down * 1.5f;

                //Instantiate(itemBoom, transform.position, itemBoom.transform.rotation);
            }

            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity; // 각도 초기화 
            //Destroy(gameObject);
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BorderBullet")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity; // 각도 초기화 
            //Destroy(gameObject); 
        }
        else if(collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);

            collision.gameObject.SetActive(false);
            //Destroy(collision.gameObject);

        }
    }
    void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;

        if(enemyname == "S")
        {
            GameObject bullet = objectManager.MakeObj("bulletEnemyA");
            bullet.transform.position = transform.position;

            //GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirvec = player.transform.position - transform.position;
            rigid.AddForce(dirvec.normalized * 0.6f, ForceMode2D.Impulse);

        }
        else if (enemyname == "L")
        {
            GameObject bulletR = objectManager.MakeObj("bulletEnemyB");
            bulletR.transform.position = transform.position+Vector3.right * 0.3F;

            GameObject bulletL = objectManager.MakeObj("bulletEnemyB");
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;

            //GameObject bulletR = Instantiate(bulletObjB, transform.position+Vector3.right*0.3F, transform.rotation);
            //GameObject bulletL = Instantiate(bulletObjB, transform.position+Vector3.left * 0.3f, transform.rotation);

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            Vector3 dirvecR = player.transform.position - (transform.position + Vector3.right * 0.3F);
            Vector3 dirvecL = player.transform.position - (transform.position + Vector3.left * 0.3F);

            rigidR.AddForce(dirvecR.normalized * 0.8f, ForceMode2D.Impulse);
            rigidL.AddForce(dirvecL.normalized * 0.8f, ForceMode2D.Impulse);
        }

            curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
    // 상하좌우 보더 밖으로 못나가게 트리거로 막는 함수 

}
