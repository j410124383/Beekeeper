using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : FindGM
{

    //这个脚本负责，决定当前小蜜蜂的行为状态，以什么为目标。

    [Tooltip("当前目标的坐标位置")] public Vector3 Target_Point;

    //记忆体
    //[Tooltip("记忆体-卸货坐标")] public Vector3 Unload_Point;
    //[Tooltip("记忆体-工作坐标")] public Vector3 Work_Point;
    [Tooltip("记忆体-进食坐标")] public Vector3 Eat_Point;

    //释放信息素
    [Tooltip("信息素预制体")] public GameObject Pheromonesb_Obj;
    protected GameObject Pheromenes_Source;

    //移动
    [Tooltip("移动速度")] public float MoveSpeed = 1f;
    [Tooltip("转向速度")] public float RotateSpeed = 5f;

    //死亡
    [Tooltip("剩余生命时间")] public float DieTime;

    //饥饿
    [Tooltip("剩余饥饿时间")] public float HungerTime;
    protected float StartHungerTime;

    //行为
    //[Tooltip("去采集")] public bool ToWork;
    //[Tooltip("回家卸货")] public bool ToHive;
    //[Tooltip("去吃东西")] public bool ToEat;

    //碰撞体 相关类 储存
    protected GameObject col_Obj;
    protected Storage storage ;
    protected Storage storage_col ;
    protected Food food_col ;



    protected float temp_RotateAngle = 0.0f;
    protected float temp_RotatedLerp = 0.0f;
    protected Vector3 temp_RotatedVec;

    

    public void Init()
    {

        Target_Point = transform.position;
        Eat_Point = transform.position;
        temp_RotateAngle = transform.eulerAngles.z;
        temp_RotatedVec = transform.eulerAngles;
        //继承饥饿值上限
        StartHungerTime = HungerTime;



    }//出生 初始化

    public void Move()
    {
        Vector3 v = Target_Point - transform.position;
        v.z = 0;
        
        if (Quaternion.Euler(0, 0, temp_RotateAngle) == transform.rotation)
        {
            temp_RotateAngle = Vector3.SignedAngle(Vector3.right, v, Vector3.forward);
            temp_RotatedLerp = 0.0f;
        }
        else
        {
            //加lerp变成变量++++++++++
            //修正 目标之间的角度
            if (temp_RotatedLerp >= 1)
            {
                temp_RotatedLerp = 1;
                transform.rotation = Quaternion.Euler(0, 0, temp_RotateAngle);
                return;
            }
            Quaternion ros = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, temp_RotateAngle), temp_RotatedLerp);
            transform.rotation = ros;
            temp_RotatedLerp += Time.deltaTime * RotateSpeed * GM.TimeSpeed;
        }

        this.transform.Translate(Vector3.right * Time.deltaTime * MoveSpeed* GM.TimeSpeed);

    } //朝着目标移动就完事了


    public void Hunger()
    {
        if (GC && GC.NoHunger_WorkerBee != true)
        {
            HungerTime -= Time.deltaTime*GM.TimeSpeed;

            if (HungerTime <= 0)        //如果是饥饿状态，则去吃饭
            {
                HungerTime = 0;

                List<GameObject> L = GetComponent<Bee_Search>().Honey_list;
                if (L.Count > 0 && L[0] )
                {
                    Target_Point = L[0].transform.position;
                }
                else { Target_Point = Eat_Point; }

            }
        }

    }//逐渐饥饿


    public void ToEat()
    {
        //进食
        //已经饥饿，碰撞体是事物，食物为已发酵的
        if (HungerTime <= 0 && col_Obj.tag == "Food" && food_col.IsBrew == true)
        {
            //销毁蜂蜜，刷新饥饿值
            food_col.Die();
            HungerTime = StartHungerTime;

            //将其替换到记忆体中
            Eat_Point = col_Obj.transform.position;
            //Mark();
        }

    }//还需要修改，暂时使用过的暴力覆盖的子类方式

    public void Die()
    {
        if (GC && GC.NoDie_WorkerBee != true)
        {
            if (HungerTime <= 0) { DieTime -= 2 * Time.deltaTime* GM.TimeSpeed; }
            else { DieTime -= Time.deltaTime*GM.TimeSpeed; }

            if (DieTime <= 0)
            {
                ToDie();
            }
        }

    }//逐渐衰老

    public void ToDie()
    {
        Destroy(this.gameObject);
        //返回一个数值
    }//死掉


    public void Sleep()
    {


    }



    protected virtual void OnTriggerEnter2D(Collider2D col)
    {

    }

    protected virtual void OnDrawGizmos()
    {
        if (GM && GM.GetComponent<GameController>().Line_Target == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, Target_Point);
        }


    }
}