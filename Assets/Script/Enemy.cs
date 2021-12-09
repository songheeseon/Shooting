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
    public GameManager gameManager;
    public ObjectManager objectManager;

    SpriteRenderer spriteRenderer;
   
    Animator anim;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (enemyname == "B")
            anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        switch(enemyname)
        {
            case "B":
                health = 1000;
                Invoke("Stop", 1.2f);
                break;
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

    private void Stop()
    {
        if (!gameObject.activeSelf)
            return;

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }

    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch(patternIndex)
        {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void FireFoward()
    {
        if(health <= 0) 
            return;
        // # Fire 4 
        GameObject bulletR = objectManager.MakeObj("bulletBossA");
        bulletR.transform.position = transform.position + Vector3.right * 0.3F;
  

        GameObject bulletL = objectManager.MakeObj("bulletBossA");
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
  

        GameObject bulletRR = objectManager.MakeObj("bulletBossA");
        bulletRR.transform.position = transform.position + Vector3.right * 0.9F;
  

        GameObject bulletLL = objectManager.MakeObj("bulletBossA");
        bulletLL.transform.position = transform.position + Vector3.left * 0.9f;
      

        //GameObject bulletR = Instantiate(bulletObjB, transform.position+Vector3.right*0.3F, transform.rotation);
        //GameObject bulletL = Instantiate(bulletObjB, transform.position+Vector3.left * 0.3f, transform.rotation);

        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

        Vector3 dirvecR = player.transform.position - (transform.position + Vector3.right * 0.3F);
        Vector3 dirvecL = player.transform.position - (transform.position + Vector3.left * 0.3F);
        Vector3 dirvecRR = player.transform.position - (transform.position + Vector3.right * 0.9F);
        Vector3 dirvecLL = player.transform.position - (transform.position + Vector3.left * 0.9F);

        rigidR.AddForce(Vector2.down * 0.5F, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 0.5f, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 0.5f, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 0.5f, ForceMode2D.Impulse);

        Debug.Log("앞으로 4발 발사");

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireFoward", 2);
        else
            Invoke("Think", 2);
    }

    void FireShot()
    {
        if (health <= 0)
            return;

        for (int index = 0; index < 5; index++)
        {
            GameObject bullet = objectManager.MakeObj("bulletBossB");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity; // 회전 초기화, 회전 값 0 

            //GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirvec = player.transform.position - transform.position;
  
            // 미사일 위치가 겹치지않게 랜덤값을 더해주는 코드 
            Vector2 ranvec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirvec += ranvec;
            rigid.AddForce(dirvec.normalized * 0.5f, ForceMode2D.Impulse);
        }



        Debug.Log("플레이어 방향으로 샷건");

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireShot", 3);
        else
            Invoke("Think", 2);
    }

    void FireArc()
    {
        if (health <= 0)
            return;

        // # Fire Arc 부채꼴모양 
        GameObject bullet = objectManager.MakeObj("bulletBossA");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity; // 회전 초기화, 회전 값 0 

        //GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        Vector2 dirvec = new Vector2(Mathf.Sin(Mathf.PI* 4 * curPatternCount/maxPatternCount[patternIndex]), -1);
        rigid.AddForce(dirvec.normalized * 0.4f, ForceMode2D.Impulse);

        Debug.Log("부채꼴 모양 발사 ");

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireArc", 0.05f);
        else
            Invoke("Think", 2);
    }

    void FireAround()
    {
        if (health <= 0)
            return;

        // # Fire Around 부채꼴모양 
        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;
        for (int index=0; index<roundNumA; index++)
        {
            GameObject bullet = objectManager.MakeObj("bulletBossB");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity; // 회전 초기화, 회전 값 0 

            //GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirvec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index/ roundNum),
                                        Mathf.Sin(Mathf.PI * 2 * index / roundNum));
            rigid.AddForce(dirvec.normalized * 0.2f, ForceMode2D.Impulse);

            Vector3 rotvec = Vector3.forward * 360 * index / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotvec);

        }


        Debug.Log("원 형태로 전체 공격.");

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireAround", 0.5f);
        else
            Invoke("Think", 2);
    }

    void Update()
    {
        if (enemyname == "B")
            return;

        Fire();
        Reload();

    }


    public void OnHit(int dmg)
    {
        if (health <= 0)
            return;

        health -= dmg;

        if (enemyname == "B")
        {
            anim.SetTrigger("OnHit");
        }
        else
        {
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }

        if (health <= 0){
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            // #. Random Ratio Item Drop
            int ran =  enemyname == "B" ? 0 : Random.Range(0, 10); 

            if(ran < 5){
                Debug.Log("Not Item");
            }else if (ran < 6){
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
            gameManager.callExplosion(transform.position, enemyname);
            //Destroy(gameObject);

            // # Boss Kill 
            if(enemyname == "B")
            {
                gameManager.StageEnd();
            }
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BorderBullet" && enemyname != "B")
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
