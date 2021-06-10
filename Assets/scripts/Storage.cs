using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public enum State
    {
        EMPTY,
        EXIST,
        FULL
    }


    //存储库
    public int Max_Count;
    public State state =State.EXIST;

    //食物
    [Tooltip("食物信息")] public List<GameObject> Food_list = new List<GameObject>();
    //[Tooltip("未受精花粉")] public List<GameObject> POLLEN_list = new List<GameObject>();
    [Tooltip("已受精花粉")] public List<GameObject> POLLEN_FERTILIZED_list = new List<GameObject>();
    [Tooltip("蜂蜜")] public List<GameObject> HONEY_list = new List<GameObject>();
    [Tooltip("蜜蜡")] public List<GameObject> WAX_list = new List<GameObject>();

    private List<List<GameObject>> Alist;


    private void Awake()
    {
        //告诉程序，一共有多少个list需要排序，
        Alist = new List<List<GameObject>>
        {
            Food_list,
            //POLLEN_list,
            POLLEN_FERTILIZED_list,
            HONEY_list,
            WAX_list

        };
    }

    void Update()
    {
        Check();
    }

    public void Check()  //检查自己的口袋
    {

        //清楚以往记录
        for (int i = 0; i < Alist.Count; i++)
        {
            Alist[i].Clear();
        }

        //遍历所有子物体
        foreach (Transform child in this.transform)
        {


            if (child.gameObject.tag == "Food")
            {
                Food_list.Add(child.gameObject);

                switch (child.GetComponent<Food>().state)
                {
                    //case Food.State.POLLEN:
                    //    POLLEN_list.Add(child.gameObject);
                    //    break;
                    case Food.State.POLLEN_FERTILIZED:
                        POLLEN_FERTILIZED_list.Add(child.gameObject);
                        break;
                    case Food.State.HONEY:
                        HONEY_list.Add(child.gameObject);
                        break;
                    case Food.State.WAX:
                        WAX_list.Add(child.gameObject);
                        break;
                    default:
                        break;
                }
            }
        }

        //当存储蜜蜂低于最大值，则full is false
        if (Food_list.Count < Max_Count && Food_list.Count > 0)
        {
            state = State.EXIST;
        }
        if(Food_list.Count ==0){
            state = State.EMPTY;
        }
        if (Food_list.Count == Max_Count)
        {
            state = State.FULL;
        }

    }


}
