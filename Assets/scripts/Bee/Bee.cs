using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bee : FindGM
{
    protected enum State
    {
        HUNGER,

        //woker
        PICKPOLLEN,
        UNLOAD,

        //builder
        TAKEHONEY,
        BREW,
        MOLD,
    }
    //这个脚本负责，决定当前小蜜蜂的行为状态，以什么为目标。

    [Tooltip("当前状态")] [SerializeField]protected State state = State.PICKPOLLEN;

    [Tooltip("当前目标的坐标位置")] public Vector3 Target_Point;

    //记忆体
    [Tooltip("记忆体-卸货坐标")] [SerializeField] protected Vector3 Unload_Point;
    [Tooltip("记忆体-采集坐标")] [SerializeField] protected Vector3 PickPollen_Point;
    [Tooltip("记忆体-进食坐标")] [SerializeField] protected Vector3 Eat_Point;
    [Tooltip("记忆体-拿蜜坐标")] [SerializeField] protected Vector3 TakeHoney_Point;
    [Tooltip("记忆体-建造坐标")] [SerializeField] protected Vector3 Build_Point;


    //释放信息素
    [Tooltip("信息素预制体")] protected GameObject Pheromones_Obj;
    string phe_path = "Assets/Res/Prefab/Pheromones.prefab";

    //移动
    [Tooltip("移动速度")] public float MoveSpeed = 1f;
    [Tooltip("转向速度,数值越大转向越慢")] public float RotateSpeed = 5f;

    //死亡
    [Tooltip("剩余生命时间")] public float DieTime;

    //饥饿
    [Tooltip("剩余饥饿时间")] public float HungerTime;
    protected float StartHungerTime;

    //碰撞体 相关类 储存
    protected GameObject col_Obj;
    protected Storage storage ;
    protected Storage storage_col ;
    protected Food food_col ;
    protected BeeHive beeHive_col;

    protected float temp_RotateAngle = 0.0f;
    protected float temp_RotatedLerp = 0.0f;
    protected Vector3 temp_RotatedVec;

    

    public void Init()
    {

        Target_Point = transform.position;
        Eat_Point = transform.position;
        Unload_Point = transform.position;
        PickPollen_Point = transform.position;
        TakeHoney_Point = transform.position;
        Build_Point = transform.position;


        temp_RotateAngle = transform.eulerAngles.z;
        temp_RotatedVec = transform.eulerAngles;
        //继承饥饿值上限
        StartHungerTime = HungerTime;

        storage = GetComponent<Storage>();

        Pheromones_Obj = AssetDatabase.LoadAssetAtPath("Assets/Res/Prefab/Pheromones.prefab", typeof(GameObject))as GameObject;
       

    }//出生 初始化

    public void Receive()
    {
     
        List<GameObject> R = GetComponent<Bee_Search>().mark_list;
        if (R.Count > 0 && R[0])
        {
            Unload_Point = R[0].GetComponent<Pheromones>().Unload_Point;
            PickPollen_Point = R[0].GetComponent<Pheromones>().PickPollen_Point;
            Eat_Point = R[0].GetComponent<Pheromones>().Eat_Point;
            TakeHoney_Point= R[0].GetComponent<Pheromones>().TakeHoney_Point;
            Build_Point = R[0].GetComponent<Pheromones>().Build_Point;
        }
    }    //最近的信息素会影响记忆体的储存坐标

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
            temp_RotatedLerp += Time.deltaTime  * GM.TimeSpeed *RotateSpeed;
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
                state = State.HUNGER;

                List<GameObject> L =GetComponent<Bee_Search>().Beehive_NOEMPTY_list;
                if (L.Count > 0 && L[0] )
                {
                    Target_Point = L[0].transform.position;
                }
                else { Target_Point = Eat_Point; }

            }
        }

    }//逐渐饥饿
    public void Eat()
    {
        if (col_Obj.tag == "Beehive" && storage_col.HONEY_list[0])
        {
            //销毁蜂蜜，刷新饥饿值
            storage_col.HONEY_list[0].GetComponent<Food>().Die();
            storage_col.Check();
            HungerTime = StartHungerTime;

            //将其替换到记忆体中
            Eat_Point = col_Obj.transform.position;
            state = State.PICKPOLLEN;

            Mark();
        }
    }




    public void Die()
    {
        if (GC && GC.NoDie_WorkerBee != true)
        {
            if (HungerTime <= 0) { DieTime -= 2 * Time.deltaTime* GM.TimeSpeed; }
            else { DieTime -= Time.deltaTime*GM.TimeSpeed; }

            if (DieTime <= 0)
            {
                Destroy(this.gameObject);
            }
        }

    }//逐渐衰老
    protected void Mark() 
    {

        GameObject mark = Instantiate(Pheromones_Obj, transform.position, transform.rotation);
        Pheromones pheromenes = mark.GetComponent<Pheromones>();
        pheromenes.PickPollen_Point = PickPollen_Point;
        pheromenes.Unload_Point = Unload_Point;
        pheromenes.Eat_Point = Eat_Point;

    } //留下信息素

    protected void OnTriggerStay2D(Collider2D col)
    {
        col_Obj = col.gameObject;
        Behaviour();
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        //base.OnTriggerEnter2D( col);
        col_Obj = col.gameObject;
        Behaviour();

    } //碰撞检测

    protected  virtual void Behaviour()
    {
        storage_col = col_Obj.GetComponent<Storage>();
        food_col = col_Obj.GetComponent<Food>();
        beeHive_col = col_Obj.GetComponent<BeeHive>();
    }

    protected virtual void OnDrawGizmos()
    {
        //目的地线
        if (GM && GM.GetComponent<GameController>().Line_Target == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, Target_Point);
        }

        //被影响信息素线
        if (GM && GM.GetComponent<GameController>().Line_Pheromenes_Source == true)
        {
            if (GetComponent<Bee_Search>().mark_list[0])
            {
                GameObject z = GetComponent<Bee_Search>().mark_list[0];
                Gizmos.color = z.GetComponent<SpriteRenderer>().color;
                Gizmos.DrawLine(transform.position, z.transform.position);
            }
        }
    }
}