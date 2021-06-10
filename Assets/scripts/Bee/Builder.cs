using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Bee
{
    //这个脚本负责，决定当前小蜜蜂的行为状态，以什么为目标。
    [Tooltip("记忆体-拿蜜坐标")] [SerializeField] protected Vector3 PickHoney_Point;
    [Tooltip("记忆体-建造坐标")] [SerializeField] protected Vector3 Build_Point;

    protected override void Awake()
    {
        base.Awake();
        Init();

    }


    public void Update()
    {

        Hunger();
        Build();
        Move();


        Receive();

        Die();

    }

    public void Build()
    {

        if (state != State.HUNGER)
        {
            List<GameObject> L;

            //口袋不为满，则先去采集honey
            if (GetComponent<Storage>().state != Storage.State.FULL)
            {
                
                state = State.TAKEHONEY;
                L = GetComponent<Bee_Search>().Beehive_STORAGEROOM_list;
                if (L.Count > 0 )
                {
                    //在l数组中寻找有无蜜蜂的如果有honey才前往
                    for (int i = 0; i < L.Count; i++)
                    {
                        if (L[i].GetComponent<Storage>().HONEY_list.Count > 0)
                        {
                            Target_Point = L[i].transform.position;
                            break;
                        }
                    }

                }
                else { Target_Point = PickHoney_Point; }
            }
            //如果口袋已满则开始变蜡
            if (storage.WAX_list.Count>0)
            {
                //如果有蜡就去吐掉
                state = State.MOLD;
                //遍历感应范围内毛坯，中数量最多的，选择之为目标
                if (GetComponent<Bee_Search>().Beehive_ROUGHCAST_list.Count > 0) {
                    L = GetComponent<Bee_Search>().Beehive_ROUGHCAST_list;
                    GameObject K = L[0];
                    for (int i = 0; i < L.Count; i++)
                    {
                        if (L[i].GetComponent<Storage>().WAX_list.Count >= K.GetComponent<Storage>().WAX_list.Count)
                        {
                            K = L[i];
                        }
                    }
                    Target_Point = K.transform.position;
                }
                else
                {
                    Target_Point = Build_Point;
                }
                

            }
        }

    }//工蜂特有的工作行为

    protected override void Behaviour()
    {
        base.Behaviour();

        switch (state)
        {
            case State.HUNGER:
                Eat();
                break;
            case State.TAKEHONEY:                //采花粉
                TakeHoney();
                break;
            case State.MOLD:                //卸货
                Mold();
                break;
            default:
                break;
        }
   
    }

    public void TakeHoney()
    {
        if (col_Obj.tag == "Beehive" && beeHive_col.state == BeeHive.BeeHiveState.STORAGEROOM)
        {
            //只要对面不为空，并且自己不为满则一直填充
            while ( storage_col.HONEY_list.Count>0 && storage.state != Storage.State.FULL )
            {
                storage_col.HONEY_list[0].transform.SetParent(transform);
                storage.Check();
                storage_col.Check();

                PickHoney_Point = col_Obj.transform.position;

                Mark();

            }

        }
    }
    public void Mold()
    {
        //对面是beehive，是毛坯，有储存空间，自身有蜡
        if (col_Obj.tag == "Beehive" && beeHive_col.state ==BeeHive.BeeHiveState.ROUGHCAST )
        {
            //只要对面不为满，并且自己不为空则一直填充
            while (storage_col.state != Storage.State.FULL && storage.WAX_list.Count>0)
            {
                storage.WAX_list[0].transform.SetParent(col_Obj.transform);
                storage.Check();
                storage_col.Check();
                beeHive_col.ToSRCheck();

                Build_Point = col_Obj.transform.position;
                Mark();
            }

        }
    }
    protected override void Mark()
    {
        base.Mark();
        phe_m.GetComponent<Pheromones>().PickHoney_Point = PickHoney_Point;
        phe_m.GetComponent<Pheromones>().Build_Point = Build_Point;

    }
    protected override void Receive()
    {
        base.Receive();
        if (phe_r)
        {
            PickHoney_Point = phe_r.GetComponent<Pheromones>().PickHoney_Point;
            Build_Point = phe_r.GetComponent<Pheromones>().Build_Point;
        }

        
    }
}
