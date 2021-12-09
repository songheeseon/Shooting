using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public int power;
    public int maxpower;
    public int boom;
    public int maxboom;
   
    public float maxShotDelay; // 발사 딜레이 
    public float curShotDelay; 

    public int life;  // 목숨 
    public int score; // 점수

    public bool isTouchTop;   // 상하좌우 보더 
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;

    public bool isBoomTime;
    public bool isRespawnTime; // 생성 후 무적시간

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject BoomEffect;

    public GameManager gameManager;
    public ObjectManager objectManager;

    public bool isHit;

    SpriteRenderer spriteRenderer;
    // 애니메이션 선언 
    Animator anim;

    public GameObject[] follwers;
    public bool Overwelming;

    // Start is called before the first frame update

    void Awake()
    {
        // 애니메이션 불러오기 
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    void OnEnable()
    {
        welming();
        Invoke("welming", 3);
    }

    void welming()
    {
        Overwelming = !Overwelming;

        if (Overwelming)
        {
            Overwelming = true;
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            Overwelming = false;
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }


    void Update()
    {
        Move();
        Fire();
        Reload();
        Boom();
    }

    void Move()
    {
        // 상하 움직임 구현
        float h = Input.GetAxisRaw("Horizontal");
        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1))
            h = 0;

        // 좌우 움직임 구현 
        float v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))
            v = 0;

        // 움직임 구현 
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        // 애니메이션 실행 
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Input", (int)h);
        }
    }

    void Boom()
    {
        if (!Input.GetButton("Fire2"))
            return;

        if (isBoomTime)
            return;

        if (boom == 0)
            return;

        boom--;
        isBoomTime = true; 
        

        // Effect visible
        BoomEffect.SetActive(true);
        gameManager.UpdateBoomIcon(boom);
        Invoke("OffBoomEffect", 4f);

        // Remove Enemy 

        // Find 함수는 부하가 생길 수 있음 Logic 정리 
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] enemiesL = objectManager.GetPool("enemyL");
        GameObject[] enemiesM = objectManager.GetPool("enemyM");
        GameObject[] enemiesS = objectManager.GetPool("enemyS");
        for (int index = 0; index < enemiesL.Length; index++)
        {
            if (enemiesL[index].activeSelf)
            {
                Enemy enemyLogic = enemiesL[index].GetComponent<Enemy>();
                enemyLogic.OnHit(200);
            }
        }

        for (int index = 0; index < enemiesM.Length; index++)
        {
            if (enemiesM[index].activeSelf)
            {
                Enemy enemyLogic = enemiesM[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesS.Length; index++)
        {
            if (enemiesS[index].activeSelf)
            {
                Enemy enemyLogic = enemiesS[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        // Remove Enemy Bullet
        //GameObject[] bullets = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bulletsA = objectManager.GetPool("bulletEnemyA");
        GameObject[] bulletsB = objectManager.GetPool("bulletEnemyB");

        for (int index = 0; index < bulletsA.Length; index++)
        {
            if (bulletsA[index].activeSelf)
            {
                bulletsA[index].SetActive(false);
                //Destroy(bullets[index]);
            }
        }

        for (int index = 0; index < bulletsB.Length; index++)
        {
            if (bulletsB[index].activeSelf)
            {
                bulletsB[index].SetActive(false);
                //Destroy(bullets[index]);
            }
        }
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1") )
            return;

        if (curShotDelay < maxShotDelay)
            return;

        switch (power)
        {
            case 1:
                // Power One
                GameObject bullet = objectManager.MakeObj("bulletPlayerA");
                bullet.transform.position = transform.position;
                
                //Instantiate(bulletObjA, transform.position, transform.rotation);

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 0.5f, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletR = objectManager.MakeObj("bulletPlayerA");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;

                GameObject bulletL = objectManager.MakeObj("bulletPlayerA");
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;


                //GameObject bulletR = Instantiate(bulletObjA, transform.position + Vector3.right*0.1f, transform.rotation);
                //GameObject bulletL = Instantiate(bulletObjA, transform.position + Vector3.left*0.1f, transform.rotation);

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 0.5f, ForceMode2D.Impulse);
               
               
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidL.AddForce(Vector2.up * 0.5f, ForceMode2D.Impulse);
                break;

            default:
                GameObject bulletRR = objectManager.MakeObj("bulletPlayerA");
                bulletRR.transform.position = transform.position + Vector3.right * 0.35f;

                GameObject bulletLL = objectManager.MakeObj("bulletPlayerA");
                bulletLL.transform.position = transform.position + Vector3.left * 0.35f;

                GameObject bulletCC = objectManager.MakeObj("bulletPlayerB");
                bulletCC.transform.position = transform.position;

                //GameObject bulletRR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.35f, transform.rotation);
                //GameObject bulletLL = Instantiate(bulletObjA, transform.position + Vector3.left * 0.35f, transform.rotation);
                //GameObject bulletCC = Instantiate(bulletObjB, transform.position, transform.rotation);

                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();

                rigidRR.AddForce(Vector2.up * 0.5f, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 0.5f, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * 0.5f, ForceMode2D.Impulse);
                break;

        }

        

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
    // 상하좌우 보더 밖으로 못나가게 트리거로 막는 함수 
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name){
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
        }
        else if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            if (Overwelming)
                return; 

            if (isHit)
                return;

            isHit = true;
            life--;
            gameManager.UpdateLifeIcon(life);
            gameManager.callExplosion(transform.position, "P");

            if(life == 0)
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.RespawnPlayer();
            }
            gameManager.RespawnPlayer();
            // 사용자가 적에게 맞았을 떄 및 미사일 맞았을 때 비활 
            DeleteFollower();
           gameObject.SetActive(false);

        }else if(collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if (power == maxpower)
                        score += 500;
                    else
                    {
                        power++;
                        AddFollower();
                    }
                       
                    break;
                case "Boom":
                    if (boom == maxboom)
                        score += 500;
                    else
                        boom++;
                        gameManager.UpdateBoomIcon(boom);
                    break;

            }
            collision.gameObject.SetActive(false);
            //Destroy(collision.gameObject);
        }
   }

    void AddFollower()
    {
        if (power == 4)
            follwers[0].SetActive(true);
        else if(power == 5)
            follwers[1].SetActive(true);
        else if (power == 6)
            follwers[2].SetActive(true);
    }

    void DeleteFollower()
    {   
            follwers[0].SetActive(false);   
            follwers[1].SetActive(false);
            follwers[2].SetActive(false);
    }
    void OffBoomEffect()
    {
        BoomEffect.SetActive(false);
        isBoomTime = false;
    }
    // 상하좌우 보더에 부딪혔을 때 활성화된 부분을 나갈 때 비활성화 해주는 부분  
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
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
