using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Bee
{
    //这个脚本负责，决定当前小蜜蜂的行为状态，以什么为目标。
    protected override void Awake()
    {
        base.Awake();
        Init();

    }


    public void Update()
    {


        Receive();

        Hunger();
        Work();
        Move();
        
 
        Die();

    }

    public void Work()
    {

        //口袋空了，就去采集，满了就回去卸货
        if (state !=State.HUNGER)
        {
             List<GameObject> L ;

            if (GetComponent<Storage>().state != Storage.State.FULL)
            {
                //采集
                state = State.PICKPOLLEN;
                 L = GetComponent<Bee_Search>().Food_list;
                if (L.Count > 0 && L[0] != null)
                {
                    Target_Point = L[0].transform.position;
                }
                else { Target_Point = PickPollen_Point; }
            }
            if (GetComponent<Storage>().state ==Storage.State.FULL)
            {
                //回家
                state = State.UNLOAD;
                 L = GetComponent<Bee_Search>().Beehive_STORAGEROOM_list;
                if (L.Count > 0)
                {
                    Target_Point = L[0].transform.position;
                }
                else { Target_Point = Unload_Point; }
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
            case State.PICKPOLLEN:                //采花粉
                PickPollen();
                break;
            case State.UNLOAD:                //卸货
                Unload();
                break;
            default:
                break;
        }
    }

    public void PickPollen()
    {
        if (col_Obj.tag == "Food" && storage.state != Storage.State.FULL && food_col.state == Food.State.POLLEN)
        {

            //吃到花粉后，储存到蜜胃中,开始发酵
            col_Obj.transform.SetParent(this.transform);
            food_col.state = Food.State.POLLEN_FERTILIZED;
            //将其替换到记忆体中
            PickPollen_Point = col_Obj.transform.position;
            Mark();
        }
    }
    public void Unload()
    {
        if (col_Obj.tag == "Beehive" && beeHive_col.state == BeeHive.BeeHiveState.STORAGEROOM)
        {

            //碰到到蜂巢了，把蜂蜜倒到巢穴中
            //需要先计算，蜜巢的剩余空间，循环执行，直到将蜜巢填充满
            while (storage.state != Storage.State.EMPTY && storage_col.state != Storage.State.FULL)
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
