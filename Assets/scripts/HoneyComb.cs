using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HoneyComb : MonoBehaviour
{
    //遍历所有的子目录
    [Tooltip("蜂巢名单")] public List<GameObject> Beehive_list = new List<GameObject>();
    [Tooltip("满蜂蜜的蜂巢数量")] public List<GameObject> Beehive_full_list = new List<GameObject>();

    [Tooltip("满率")] [SerializeField] private float full_Per;

    [Tooltip("蜂巢预制体")] public GameObject Beehive_obj;


    //建立蜂巢版图
    [SerializeField] public  int[,] CombMap;
    public int CombSize;


    private void Awake()
    {
        //判断是否为奇数，若是偶数则加一
        if (CombSize % 2 == 0)
        {
            CombSize++;
        }

 

    }

    private void Update()
    {

       


        Check();

        full_Per = Beehive_full_list.Count / Beehive_list.Count ;
        if (full_Per>0.9f)
        {
            GameObject newhive = Instantiate(Beehive_obj, transform.position, transform.rotation);
            newhive.transform.SetParent(this.transform);
        }
    }

    void Check()
    {
        Beehive_list.Clear();
        Beehive_full_list.Clear();

        //遍历所有子物体，生成蜂巢
        foreach(Transform child in this.transform)
        {
           
            if (child.gameObject.tag == "Beehive")
            {
                Beehive_list.Add(child.gameObject);
                if (child.gameObject.GetComponent<Storage>().IsFull == true)
                {
                    Beehive_full_list.Add(child.gameObject);
                }

            }
        }
    }

    void Build()
    {

    }


}
