using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Queen : MonoBehaviour
{
    //这个脚本负责，决定当前小蜜蜂的行为状态，以什么为目标。

    [Tooltip("当前目标的坐标位置")] public Vector3 Target_Point;

    //记忆体
    //[Tooltip("记忆体-卸货坐标")] public Vector3 Unload_Point;
    [Tooltip("记忆体-产生坐标")] public Vector3 Born_Point;
    [Tooltip("记忆体-进食坐标")] public Vector3 Eat_Point;

    [Tooltip("移动速度")] public float MoveSpeed = 1f;

    [Tooltip("剩余生命时间")] public float DieTime;
    [Tooltip("剩余饥饿时间")] public float HungerTime;
    private float StartHungerTime;

    [Tooltip("剩余饥饿时间")] public float BornTime;
    private float StartBornTime;

    [Tooltip("工蜂预制体")] public GameObject WorkerBee_InsOBJ;
    [Tooltip("雄蜂预制体")] public GameObject Drone_InsOBJ;
    //[Tooltip("产卵速度")] public float BornSpeed;

    private GameObject GM;

    private void Awake()
    {
        //重置各项坐标
        Target_Point = transform.position;
        Born_Point = transform.position;
        Eat_Point = transform.position;

        //重置各项时间
        StartHungerTime = HungerTime;
        StartBornTime = BornTime;

        //查找gm的位置
        if (GameObject.FindWithTag("GameManager"))
        {
            GM = GameObject.FindWithTag("GameManager");
        }

    }


    private void Update()
    {
        Action();
        Move();

        Hunger();

        Born();
    }




    public void Action()
    {
        //如果是饥饿状态，则去吃饭，否则工作
        //if (HungerTime <= 0)
        //{
        //    //进食
        //    List<GameObject> L = GetComponent<Bee_Search>().HONEY_list;
        //    if (L.Count > 0 && L[0] != null)
        //    {
        //        Target_Point = L[0].transform.position;
        //    }
        //    else { Target_Point = Eat_Point; }

        //}
        //else
        //{
        //    //不饿的话就生孩子
        //    if (BornTime<=0)
        //    {
        //        //回家
        //        List<GameObject> L = GetComponent<Bee_Search>().Beehive_list;
        //        if (L.Count > 0)
        //        {
        //            Target_Point = L[0].transform.position;
        //        }
        //        else { Target_Point = Born_Point; }
        //    }
        //}






    }//指定行为目标，当储存未满，目标是food，存储已满，目标是家里

    private void OnTriggerEnter2D(Collider2D col)
    {
        Food food_col = col.GetComponent<Food>();
        //进食
        if (col.tag == "Food" && HungerTime <= 0 && food_col.state==Food.State.HONEY)
        {
            //销毁蜂蜜，刷新饥饿值
            food_col.Die();
            HungerTime = StartHungerTime;

            //将其替换到记忆体中
            Eat_Point = col.transform.position;
          
        }

        //生产
        if (col.tag == "Beehive" && BornTime <=0)
        {

            //碰到到蜂巢了
                BornTime = StartBornTime;
                Vector3 v1 = transform.position + Vector3.right;
                GameObject instance = Instantiate(WorkerBee_InsOBJ, v1, transform.rotation);
            


            ////需要先计算，蜜巢的剩余空间，循环执行，直到将蜜巢填充满
            //if (storage.Food_list.Count > 0)
            //{
            //    while (storage.IsEmpty != true && storage_col.IsFull != true)
            //    {

            //        storage.Food_list[0].transform.SetParent(col.transform);
            //        storage.Check();
            //        storage_col.Check();

            //        Unload_Point = col.transform.position;
            //        Mark();
            //    }


            //}
        }




    } //碰撞检测

    public void Move()
    {

        Vector3 v = Target_Point - transform.position;
        v.z = 0;
        float angle = Vector3.SignedAngle(Vector3.right, v, Vector3.forward);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, angle), 1f);
        this.transform.Translate(Vector3.right * Time.deltaTime * MoveSpeed);
    } //朝着目标移动就完事了

    void Hunger()
    {
        if (GM.GetComponent<GameController>().NoDie_WorkerBee != true)
        {
            HungerTime -= Time.deltaTime;
            if (HungerTime <= 0)
            {
                HungerTime = 0;

            }
        }
    }


    public void Born()
    {
        BornTime -= Time.deltaTime;
        if (BornTime <= 0)
        {
            BornTime = 0;

        }
    }



}
