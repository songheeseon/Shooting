using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyBPrefab;
    public GameObject enemyLPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemySPrefab;
    public GameObject ItemCoinPrefab;
    public GameObject ItemPowerPrefab;
    public GameObject ItemBoomPrefab;
    public GameObject bulletPlayerAPrefab;
    public GameObject bulletPlayerBPrefab;
    public GameObject bulletEnemyAPrefab;
    public GameObject bulletEnemyBPrefab;
    public GameObject bulletFollowerPrefab;
    public GameObject bulletBossAPrefab;
    public GameObject bulletBossBPrefab;
    public GameObject explosionPrefab;


    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;

    GameObject[] ItemCoin;
    GameObject[] ItemPower;
    GameObject[] ItemBoom;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;
    GameObject[] bulletFollower;
    GameObject[] bulletBossA;
    GameObject[] bulletBossB;
    GameObject[] enemyB;
    GameObject[] explosion;


    GameObject[] targetPool;
    void Awake()
    {
        enemyL = new GameObject[20];
        enemyM = new GameObject[20];
        enemyS = new GameObject[30];

        ItemCoin = new GameObject[30];
        ItemPower = new GameObject[20];
        ItemBoom = new GameObject[20];

        bulletPlayerA = new GameObject[100];
        bulletPlayerB = new GameObject[100];
        bulletEnemyA = new GameObject[100];
        bulletEnemyB = new GameObject[100];
        bulletFollower = new GameObject[100];
        bulletBossA = new GameObject[200];
        bulletBossB = new GameObject[300];

        enemyB = new GameObject[1];
        explosion = new GameObject[30];


        Generate();
    }

    void Generate()
    {
        // # Enemy
        for(int index = 0; index < enemyL.Length; index++)
        {
            enemyL[index] = Instantiate(enemyLPrefab);
            enemyL[index].SetActive(false);
        }

        for (int index = 0; index < enemyM.Length; index++)
        {
            enemyM[index] = Instantiate(enemyMPrefab);
            enemyM[index].SetActive(false);
        }

        for (int index = 0; index < enemyS.Length; index++)
        {
            enemyS[index] = Instantiate(enemySPrefab);
            enemyS[index].SetActive(false);
        }

        // # Item 
        for (int index = 0; index < ItemCoin.Length; index++)
        {
            ItemCoin[index] = Instantiate(ItemCoinPrefab);
            ItemCoin[index].SetActive(false);
        }

        for (int index = 0; index < ItemPower.Length; index++)
        {
            ItemPower[index] = Instantiate(ItemPowerPrefab);
            ItemPower[index].SetActive(false);
        }

        for (int index = 0; index < ItemBoom.Length; index++)
        {
            ItemBoom[index] = Instantiate(ItemBoomPrefab);
            ItemBoom[index].SetActive(false);
        }

        // # Bullet 
        for (int index = 0; index < bulletPlayerA.Length; index++)
        {
            bulletPlayerA[index] = Instantiate(bulletPlayerAPrefab);
            bulletPlayerA[index].SetActive(false);
        }

        for (int index = 0; index < bulletPlayerB.Length; index++)
        {
            bulletPlayerB[index] = Instantiate(bulletPlayerBPrefab);
            bulletPlayerB[index].SetActive(false);
        }

        for (int index = 0; index < bulletEnemyA.Length; index++)
        {
            bulletEnemyA[index] = Instantiate(bulletEnemyAPrefab);
            bulletEnemyA[index].SetActive(false);
        }

        for (int index = 0; index < bulletEnemyB.Length; index++)
        {
            bulletEnemyB[index] = Instantiate(bulletEnemyBPrefab);
            bulletEnemyB[index].SetActive(false);
        }

        for (int index = 0; index < bulletFollower.Length; index++)
        {
            bulletFollower[index] = Instantiate(bulletFollowerPrefab);
            bulletFollower[index].SetActive(false);
        }

        for (int index = 0; index < bulletBossA.Length; index++)
        {
            bulletBossA[index] = Instantiate(bulletBossAPrefab);
            bulletBossA[index].SetActive(false);
        }

        for (int index = 0; index < bulletBossB.Length; index++)
        {
            bulletBossB[index] = Instantiate(bulletBossBPrefab);
            bulletBossB[index].SetActive(false);
        }

        for (int index = 0; index < enemyB.Length; index++)
        {
            enemyB[index] = Instantiate(enemyBPrefab);
            enemyB[index].SetActive(false);
        }

        for (int index = 0; index < explosion.Length; index++)
        {
            explosion[index] = Instantiate(explosionPrefab);
            explosion[index].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        
        switch (type)
        {
            case "enemyL":
                targetPool = enemyL;
                break;

            case "enemyM":
                targetPool = enemyM;
                break;

            case "enemyS":
                targetPool = enemyS;
                break;

            case "ItemCoin":
                targetPool = ItemCoin;
                break;

            case "ItemPower":
                targetPool = ItemPower;
                break;

            case "ItemBoom":
                targetPool = ItemBoom;
                break;

            case "bulletPlayerA":
                targetPool = bulletPlayerA;
                break;

            case "bulletPlayerB":
                targetPool = bulletPlayerB;
                break;

            case "bulletEnemyA":
                targetPool = bulletEnemyA;
                break;

            case "bulletEnemyB":
                targetPool = bulletEnemyB;
                break;

            case "bulletFollower":
                targetPool = bulletFollower;
                break;

            case "bulletBossA":
                targetPool = bulletBossA;
                break;

            case "bulletBossB":
                targetPool = bulletBossB;
                break;

            case "enemyB":
                targetPool = enemyB;
                break;

            case "explosion":
                targetPool = explosion;
                break;

        }

        for (int index = 0; index < targetPool.Length; index++){
            if (!targetPool[index].activeSelf){
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }

        return null;

    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "enemyL":
                targetPool = enemyL;
                break;

            case "enemyM":
                targetPool = enemyM;
                break;

            case "enemyS":
                targetPool = enemyS;
                break;

            case "ItemCoin":
                targetPool = ItemCoin;
                break;

            case "ItemPower":
                targetPool = ItemPower;
                break;

            case "ItemBoom":
                targetPool = ItemBoom;
                break;

            case "bulletPlayerA":
                targetPool = bulletPlayerA;
                break;

            case "bulletPlayerB":
                targetPool = bulletPlayerB;
                break;

            case "bulletEnemyA":
                targetPool = bulletEnemyA;
                break;

            case "bulletEnemyB":
                targetPool = bulletEnemyB;
                break;

            case "bulletFollower":
                targetPool = bulletFollower;
                break;

            case "bulletBossA":
                targetPool = bulletBossA;
                break;

            case "bulletBossB":
                targetPool = bulletBossB;
                break;

            case "enemyB":
                targetPool = enemyB;
                break;

            case "explosion":
                targetPool = explosion;
                break;
        }

        return targetPool; 
    }


}


