using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBee : Bee
{
    //这个脚本负责，决定当前小蜜蜂的行为状态，以什么为目标。


    //记忆体
    [Tooltip("记忆体-卸货坐标")] public Vector3 Unload_Point;
    [Tooltip("记忆体-工作坐标")] public Vector3 Work_Point;


    protected override void Awake()
    {
        base.Awake();
        Init();

        Unload_Point = transform.position;
        Work_Point = transform.position;
    }


    public void Update()
    {

        //最近的信息素会影响记忆体的储存坐标
        List<GameObject> R = GetComponent<Bee_Search>().mark_list;
        if (R.Count > 0 && R[0])
        {
            Pheromenes_Source = R[0];
            Unload_Point = R[0].GetComponent<Pheromones>().Unload_Point;
            Work_Point = R[0].GetComponent<Pheromones>().Work_Point;

        }

        Hunger();
        Worker();
        Move();
        
 
        Die();

    }

    public void Worker()
    {

        //口袋空了，就去采集，满了就回去卸货
        if (HungerTime > 0)
        {
            if (GetComponent<Storage>().IsFull == false)
            {
                //采集
                List<GameObject> L = GetComponent<Bee_Search>().Pollen_list;
                if (L.Count > 0 && L[0] != null)
                {
                    Target_Point = L[0].transform.position;
                }
                else { Target_Point = Work_Point; }
            }
            if (GetComponent<Storage>().IsFull == true)
            {
                //回家
                List<GameObject> L = GetComponent<Bee_Search>().Beehive_Nofull_list;
                if (L.Count > 0)
                {
                    Target_Point = L[0].transform.position;
                }
                else { Target_Point = Unload_Point; }
            }
        }
        
    }//工蜂特有的工作行为

 
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D( col);

        col_Obj = col.gameObject;
        Storage storage = GetComponent<Storage>();
        Storage storage_col = col.GetComponent<Storage>();
        Food food_col = col.GetComponent<Food>();

        if (HungerTime <= 0)
        {
            //如果碰撞体是蜂巢，且蜂巢内有已成熟的蜂蜜
            if (HungerTime <= 0 && col_Obj.tag == "Beehive" && storage_col.Honey_list.Count>0)
            {
                //销毁蜂蜜，刷新饥饿值
                storage_col.Honey_list[0].GetComponent<Food>().Die();
                storage_col.Check();
                HungerTime = StartHungerTime;

                //将其替换到记忆体中
                Eat_Point = col_Obj.transform.position;
                Mark();
            }
            
        }
        else
        {
            //采蜜
            if (col_Obj.tag == "Food" && storage.IsFull != true
                && food_col.IsBeActivate != true)
            {

                //吃到花粉后，储存到蜜胃中,开始发酵
                col_Obj.transform.SetParent(this.transform);
                food_col.IsBeActivate = true;
                //将其替换到记忆体中
                Work_Point = col_Obj.transform.position;
                Mark();
            }

            //卸货
            if (col_Obj.tag == "Beehive" && storage.IsFull == true)
            {

                //碰到到蜂巢了，把蜂蜜倒到巢穴中
                //需要先计算，蜜巢的剩余空间，循环执行，直到将蜜巢填充满
                if (storage.Food_list.Count > 0)
                {
                    while (storage.IsEmpty != true && storage_col.IsFull != true)
                    {

                        storage.Food_list[0].transform.SetParent(col_Obj.transform);
                        storage.Check();
                        storage_col.Check();

                        Unload_Point = col_Obj.transform.position;
                        Mark();
                    }


                }
            }
        }


    } //碰撞检测


    public void Mark() //留下信息素
    {

        GameObject mark = Instantiate(Pheromonesb_Obj,transform.position, transform.rotation);
        Pheromones pheromenes = mark.GetComponent<Pheromones>();
        pheromenes.Work_Point = Work_Point;
        pheromenes.Unload_Point = Unload_Point;
        pheromenes.Eat_Point = Eat_Point;

    }

    //绘制目标路径
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (GM && GM.GetComponent<GameController>().Line_Pheromenes_Source == true)
        {
            //信息素若是影响了本体
            if (Pheromenes_Source)
            {
                Gizmos.color = Pheromenes_Source.GetComponent<SpriteRenderer>().color;
                Gizmos.DrawLine(transform.position, Pheromenes_Source.transform.position);
            }
        }

    }






}
