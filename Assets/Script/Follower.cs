using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float maxShotDelay; // 발사 딜레이 
    public float curShotDelay;
    public ObjectManager objectManager;

    public Vector3 followPos;
    public int followDelay; 
    public Transform parent;
    public Queue<Vector3> parentPos;

    private void Awake()
    {
        parentPos = new Queue<Vector3>();
    }
    void Update(){
        Watch();
        Follow();
        Fire();
        Reload();
    }

    void Watch()
    {
        // # Input Pos
        if(!parentPos.Contains(parent.position)) // 부모위치가 가만히있으면 저장x 조건 
            parentPos.Enqueue(parent.position);

        // # Output Pos
        if (parentPos.Count > followDelay) // 위치값이 딜레이보다 커야 반환, 펫이 따라오게 만드는 기능
            followPos = parentPos.Dequeue();
        else if (parentPos.Count < followDelay) // 큐가 채워지기전까진 부모위치 적용 
            followPos = parent.position;
    }
    void Follow(){
        // # 플레이어를 따라가는 로직 
        transform.position = followPos;


    }


    void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;

        if (curShotDelay < maxShotDelay)
            return;

                // Power One
                GameObject bullet = objectManager.MakeObj("bulletFollower");
                bullet.transform.position = transform.position;

                //Instantiate(bulletObjA, transform.position, transform.rotation);

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 0.5f, ForceMode2D.Impulse);
         

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
    // 상하좌우 보더 밖으로 못나가게 트리거로 막는 함수 
   
}
