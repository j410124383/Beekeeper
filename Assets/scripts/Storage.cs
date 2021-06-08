using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    //存储库
    public int Max_Count;
    [Tooltip("是否为满")] public bool IsFull = false;
    [Tooltip("是否为空")] public bool IsEmpty = true;

    //食物
    [Tooltip("食物信息")] public List<GameObject> Food_list = new List<GameObject>();
    [Tooltip("未激活的食物信息")] public List<GameObject> Pollen_list = new List<GameObject>();
    [Tooltip("已酿造完成的食物信息")] public List<GameObject> Honey_list = new List<GameObject>();

    private List<List<GameObject>> Alist;


    private void Awake()
    {
        //告诉程序，一共有多少个list需要排序，
        Alist = new List<List<GameObject>>
        {
            Pollen_list,
            Food_list,
            Honey_list,

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

                //未激活
                if (child.GetComponent<Food>().IsBeActivate != true)
                {
                    Pollen_list.Add(child.gameObject);
                }
                //已酿造完成
                if (child.GetComponent<Food>().IsBrew == true)
                {
                    Honey_list.Add(child.gameObject);
                }
            }
        }

        //当存储蜜蜂低于最大值，则full is false
        if (Food_list.Count < Max_Count )
        {
            IsFull = false;
        }
        else { IsFull = true; }


        if (Food_list.Count == 0)
        {
            IsEmpty = true;
        }
        else { IsEmpty = false; }

    }


}
