using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public string[] enemyObjs;
    public Transform[] spawnPoints;

    public float NextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;

    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;

    public ObjectManager objectManager;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    private void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjs = new string[] { "enemyS", "enemyM", "enemyL" };
        ReadSpawnFile();
    }



    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > NextSpawnDelay && !spawnEnd)
        {
            spawnEnemy();
            //NextSpawnDelay = Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }

        // #UI Score Update 
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    void ReadSpawnFile()
    {
        // # 변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // # 리스폰 읽기
        TextAsset textFile = Resources.Load("Stage0") as TextAsset; // text파일이 아니면 null처리 
        StringReader stringReader = new StringReader(textFile.text); // string 읽기 

        
        while(stringReader != null)
        {
            string line = stringReader.ReadLine();
            Debug.Log(line);

            if (line == null)
                break;

            // # 리스폰 데이터 생성 
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        // # 텍스트 파일 닫기 
        stringReader.Close();


        // # 첫번째 스폰 딜레이 적용 
        NextSpawnDelay = spawnList[0].delay;
    }

    void spawnEnemy()
    {
        int enemyIndex = 0;
        switch (spawnList[spawnIndex].type)
        {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
        }
        Debug.Log(enemyIndex);
        int enemyPoint = spawnList[spawnIndex].point;
        //int ranEnemy = Random.Range(0, 3);
        //int ranPoint = Random.Range(0, 9);

        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;

        //GameObject enemy = Instantiate(enemyObjs[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation);

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

        if(enemyPoint == 5 || enemyPoint == 6){
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed*(-1), 1);
            
        }
        else if(enemyPoint == 7 || enemyPoint == 8)
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }
        else
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed*(-1));
        }


        // # 리스폰 인덱스 증가 
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }
        else if (spawnList[0].delay == 0)
        {
            spawnEnemy();
        }

        // # 다음 리스폰 딜레이 갱신 
        NextSpawnDelay = spawnList[spawnIndex].delay;
    }

    public void UpdateLifeIcon(int life)
    {
        // 죽었을 때 이미지형태의 목숨 아이콘이 하나 사라지는 코드  

        // UI Life Init Disable
        for(int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        // UI Life Active 
        for (int index = 0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        // UI Boom Init Disable
        for (int index = 0; index < 3; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }

        // UI Boom Active 
        for (int index = 0; index < boom; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
    public void RespawnPlayer()
    {
        Player playerLogic = player.GetComponent<Player>();
        playerLogic.power = 1;
        Invoke("RespawnPlayerExe", 2f);
    }
    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);
     
       
        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;

    }
}
