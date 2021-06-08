using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : FindGM
{


    [Tooltip("花园大小")] public float grow_size;

    [Tooltip("生产速度")] public float GrowSpeed =2f;
    [Tooltip("剩余生产时间")] public float GrowTime;
    protected float StartGrowTime;


    [Tooltip("初始花粉的数量")] public int StartGrowCount;

    [Tooltip("生产花粉的预制体")] public GameObject Seed;


    

    //随机生产花粉
    protected  override void  Awake()
    {
        base.Awake();

        StartGrowTime = GrowTime;

        //初始生成花粉地图
        for (int i=0;i<StartGrowCount;i++)
        {
            Grow();
        }


    }


    void Update()
    {
        GrowTime -= Time.deltaTime * GrowSpeed * GM.TimeSpeed;
        if (GrowTime <=0 )
        {
            GrowTime = StartGrowTime;
            Grow();
        }

    }

    void Grow()
    {
        float x1 = Random.Range(-grow_size/2,grow_size/2);
        float x2 = Random.Range(-grow_size/2,grow_size/2);
        Vector3 v1 = transform.position + new Vector3(x1,x2,0);

        GameObject food = Instantiate(Seed, v1, transform.rotation);
        food.transform.SetParent(this.transform);

        //print("生产种子");

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(grow_size, grow_size, 0));


    }

}
