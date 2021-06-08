using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee_Search : MonoBehaviour
{
    //建立探测器内的所有物体分类

    [Tooltip("雷达搜索半径")] public float SearchRadius=15f;

    [Tooltip("蜂后")] public GameObject Queen;
    //食物
    [Tooltip("食物信息")] public List<GameObject> Food_list = new List<GameObject>();
    [Tooltip("未激活的食物信息")] public List<GameObject> Pollen_list = new List<GameObject>();
    [Tooltip("已酿造完成的食物信息")] public List<GameObject> Honey_list = new List<GameObject>();
    //蜂巢
    [Tooltip("巢穴信息")] public List<GameObject> Beehive_list = new List<GameObject>();
    [Tooltip("未满的巢穴信息")] public List<GameObject> Beehive_Nofull_list = new List<GameObject>();
    [Tooltip("非空的巢穴信息")] public List<GameObject> Beehive_NoEmpty_list = new List<GameObject>();
    //单位
    [Tooltip("工蜂信息")] public List<GameObject> Workerbee_list = new List<GameObject>();
    [Tooltip("雄蜂信息")] public List<GameObject> Drone_list = new List<GameObject>();


    [Tooltip("信息素物体")] public List<GameObject> mark_list = new List<GameObject>();

    private List<List<GameObject>> Alist;

    private GameObject GM;


    private void Awake()
    {
        //告诉程序，一共有多少个list需要排序，
        Alist = new List<List<GameObject>>
        {
            Pollen_list,
            Food_list,
            Honey_list,
            Beehive_list,
            mark_list,
            Beehive_Nofull_list,
            Beehive_NoEmpty_list,
            Workerbee_list,
            Drone_list

        };
    }

    //新的检测系统，使用射线
    private void Update()
    {
        //查找gm的位置
        if (GameObject.FindWithTag("GameManager"))
        {
            GM = GameObject.FindWithTag("GameManager");
        }



        //雷达侦查一波
        Check();
        Sequence();
 
    }

   
    public void Check()
    {
        //查找蜂后
        if (GameObject.FindWithTag("Queen"))
        {
            Queen = GameObject.FindWithTag("Queen");
        }
        else
        {
            Queen = null;
        }

        //遍历范围内的所有物体，将其根据tag来进行分类
        Collider2D[] all = Physics2D.OverlapCircleAll(this.transform.position, SearchRadius);

        if (all.Length > 0)
        {

            //清楚以往记录
            for(int i = 0;i<Alist.Count;i++)
            {
                Alist[i].Clear();


            }



            for (int i = 0; i < all.Length; i++)
            {
                if (all[i].gameObject.tag == "Food")
                {
                    Food_list.Add(all[i].gameObject);
                    //未激活
                    if (all[i].GetComponent<Food>().IsBeActivate != true)
                    {
                        Pollen_list.Add(all[i].gameObject);
                    }
                    //已酿造完成
                    if (all[i].GetComponent<Food>().IsBrew == true)
                    {
                        Honey_list.Add(all[i].gameObject);
                    }


                }
                if (all[i].gameObject.tag == "Beehive")
                {
                    //蜂巢未满
                    if(all[i].GetComponent<Storage>().IsFull != true)
                    {
                        Beehive_Nofull_list.Add(all[i].gameObject);
                        //print("add nofull" + all[i].gameObject);
                    }
                    //蜂巢不为空，有东西
                    if (all[i].GetComponent<Storage>().IsEmpty != true)
                    {
                        Beehive_NoEmpty_list.Add(all[i].gameObject);
                        //print("add noempty" + all[i].gameObject);
                    }

                    Beehive_list.Add(all[i].gameObject);
                }
                if (all[i].gameObject.tag == "Mark")
                {
                    mark_list.Add(all[i].gameObject);
                }
                if (all[i].gameObject.tag == "Workerbee")
                {
                    Workerbee_list.Add(all[i].gameObject);
                }
                if (all[i].gameObject.tag == "Drone")
                {
                    Drone_list.Add(all[i].gameObject);
                }
            }

        }

    } //雷达侦查一波，进行分类

    public void Sequence() {



        for (int y = 0; y < Alist.Count; y++)
        {
           

            //使用冒泡排序，将最近的放到第一个去，这种算法也只能把最近的放到前面来
            if (Alist[y].Count > 0)
            {
                for (int i = Alist[y].Count - 1; i > 0; i--)
                {
                    float v1 = (transform.position - Alist[y][i - 1].transform.position).sqrMagnitude;
                    float v = (transform.position - Alist[y][i].transform.position).sqrMagnitude;
                    
                    //比较 后面的v和前面的v1那个小，如果是后面的小，把它换上来，若果是前面的小，不变
                    if (v < v1)
                    {
                        GameObject MinObj = Alist[y][i];
                        Alist[y][i] = Alist[y][i - 1];
                        Alist[y][i - 1] = MinObj;
                    }

                }
            }
        }


    }//将分类好的进行排序



    private void OnDrawGizmos()
    {

        if (GM&& GM.GetComponent<GameController>().Sphere_Search == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, SearchRadius);
        }
      

    }//绘制侦测范围
   
}
