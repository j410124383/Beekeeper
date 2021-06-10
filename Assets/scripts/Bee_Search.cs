using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee_Search : FindGM
{
    //建立探测器内的所有物体分类

    [Tooltip("雷达搜索半径")] public float SearchRadius=15f;


    //食物
    [Tooltip("食物信息")] public List<GameObject> Food_list = new List<GameObject>();

    //蜂巢
    [Tooltip("巢穴信息")] public List<GameObject> Beehive_list = new List<GameObject>();

    [Tooltip("毛坯状态的蜂穴")] public List<GameObject> Beehive_ROUGHCAST_list = new List<GameObject>();
    [Tooltip("仓库状态的蜂穴")] public List<GameObject> Beehive_STORAGEROOM_list = new List<GameObject>();

    [Tooltip("空的的巢穴信息")] public List<GameObject> Beehive_EMPTY_list = new List<GameObject>();
    [Tooltip("未空的巢穴信息")] public List<GameObject> Beehive_NOEMPTY_list = new List<GameObject>();
    [Tooltip("未满的巢穴信息")] public List<GameObject> Beehive_NOFULL_list = new List<GameObject>();
    [Tooltip("已满的巢穴信息")] public List<GameObject> Beehive_FULL_list = new List<GameObject>();

    //单位
    //[Tooltip("工蜂信息")] public List<GameObject> Workerbee_list = new List<GameObject>();
    //[Tooltip("雄蜂信息")] public List<GameObject> Drone_list = new List<GameObject>();
    //[Tooltip("蜂后信息")] public GameObject Queen;

    [Tooltip("信息素物体")] public List<GameObject> mark_list = new List<GameObject>();

    private List<List<GameObject>> Alist;



    protected override void Awake()
    {
        base.Awake();

        //告诉程序，一共有多少个list需要排序，
        Alist = new List<List<GameObject>>
        {
            
            Food_list,
            //POLLEN_list,
            //POLLEN_FERTILIZED_list,
            //HONEY_list,
            //WAX_list,

            Beehive_list,
            Beehive_ROUGHCAST_list,
            Beehive_STORAGEROOM_list,

            Beehive_EMPTY_list,
            Beehive_NOEMPTY_list,
            Beehive_NOFULL_list,
            Beehive_FULL_list,

            //Workerbee_list,
            //Drone_list,

            mark_list

        };
    }

    //新的检测系统，使用射线
    private void Update()
    {

        //雷达侦查一波
        Check();
        Sequence();
 
    }

   
    protected void Check()
    {
        //清除以往记录
        for (int i = 0; i < Alist.Count; i++)
        {
            Alist[i].Clear();
        }


        ////查找蜂后
        //if (GameObject.FindWithTag("Queen"))
        //{
        //    Queen = GameObject.FindWithTag("Queen");
        //}
        //else
        //{
        //    Queen = null;
        //}

        //遍历范围内的所有物体，将其根据tag来进行分类
        Collider2D[] all = Physics2D.OverlapCircleAll(this.transform.position, SearchRadius);

        if (all.Length > 0)
        {

            for (int i = 0; i < all.Length; i++)
            {
                switch (all[i].gameObject.tag)
                {
                    case "Food":
                        Food_list.Add(all[i].gameObject);
                        break;
                    case "Beehive":
                        Beehive_list.Add(all[i].gameObject);
                        switch (all[i].GetComponent<BeeHive>().state)
                        {
                            case BeeHive.BeeHiveState.ROUGHCAST:
                                Beehive_ROUGHCAST_list.Add(all[i].gameObject);
                                break;
                            case BeeHive.BeeHiveState.STORAGEROOM:
                                Beehive_STORAGEROOM_list.Add(all[i].gameObject);
                                break;
                            default:
                                break;
                        }  //根据蜂巢的阶段性属性进行归类
                        switch (all[i].GetComponent<Storage>().state)
                        {
                            case Storage.State.EMPTY:
                                Beehive_EMPTY_list.Add(all[i].gameObject);
                                Beehive_NOFULL_list.Add(all[i].gameObject);
                                break;
                            case Storage.State.EXIST:
                                Beehive_NOFULL_list.Add(all[i].gameObject);
                                Beehive_NOEMPTY_list.Add(all[i].gameObject);
                                break;
                            case Storage.State.FULL:
                                Beehive_FULL_list.Add(all[i].gameObject);
                                Beehive_NOEMPTY_list.Add(all[i].gameObject);
                                break;
                            default:
                                break;
                        }  //根据蜂巢的存储量进行分类
                        break;
                    case "Mark":
                        mark_list.Add(all[i].gameObject);
                        break;
                    default:
                        break;

                }


            }

        }

    } //雷达侦查一波，进行分类

    protected void Sequence() {



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



    protected void OnDrawGizmos()
    {

        if (GM&& GM.GetComponent<GameController>().Sphere_Search == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, SearchRadius);
        }
      

    }//绘制侦测范围
   
}
