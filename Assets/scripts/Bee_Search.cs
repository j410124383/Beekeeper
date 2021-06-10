using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee_Search : FindGM
{
    //����̽�����ڵ������������

    [Tooltip("�״������뾶")] public float SearchRadius=15f;


    //ʳ��
    [Tooltip("ʳ����Ϣ")] public List<GameObject> Food_list = new List<GameObject>();

    //�䳲
    [Tooltip("��Ѩ��Ϣ")] public List<GameObject> Beehive_list = new List<GameObject>();

    [Tooltip("ë��״̬�ķ�Ѩ")] public List<GameObject> Beehive_ROUGHCAST_list = new List<GameObject>();
    [Tooltip("�ֿ�״̬�ķ�Ѩ")] public List<GameObject> Beehive_STORAGEROOM_list = new List<GameObject>();

    [Tooltip("�յĵĳ�Ѩ��Ϣ")] public List<GameObject> Beehive_EMPTY_list = new List<GameObject>();
    [Tooltip("δ�յĳ�Ѩ��Ϣ")] public List<GameObject> Beehive_NOEMPTY_list = new List<GameObject>();
    [Tooltip("δ���ĳ�Ѩ��Ϣ")] public List<GameObject> Beehive_NOFULL_list = new List<GameObject>();
    [Tooltip("�����ĳ�Ѩ��Ϣ")] public List<GameObject> Beehive_FULL_list = new List<GameObject>();

    //��λ
    //[Tooltip("������Ϣ")] public List<GameObject> Workerbee_list = new List<GameObject>();
    //[Tooltip("�۷���Ϣ")] public List<GameObject> Drone_list = new List<GameObject>();
    //[Tooltip("�����Ϣ")] public GameObject Queen;

    [Tooltip("��Ϣ������")] public List<GameObject> mark_list = new List<GameObject>();

    private List<List<GameObject>> Alist;



    protected override void Awake()
    {
        base.Awake();

        //���߳���һ���ж��ٸ�list��Ҫ����
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

    //�µļ��ϵͳ��ʹ������
    private void Update()
    {

        //�״����һ��
        Check();
        Sequence();
 
    }

   
    protected void Check()
    {
        //���������¼
        for (int i = 0; i < Alist.Count; i++)
        {
            Alist[i].Clear();
        }


        ////���ҷ��
        //if (GameObject.FindWithTag("Queen"))
        //{
        //    Queen = GameObject.FindWithTag("Queen");
        //}
        //else
        //{
        //    Queen = null;
        //}

        //������Χ�ڵ��������壬�������tag�����з���
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
                        }  //���ݷ䳲�Ľ׶������Խ��й���
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
                        }  //���ݷ䳲�Ĵ洢�����з���
                        break;
                    case "Mark":
                        mark_list.Add(all[i].gameObject);
                        break;
                    default:
                        break;

                }


            }

        }

    } //�״����һ�������з���

    protected void Sequence() {



        for (int y = 0; y < Alist.Count; y++)
        {
           

            //ʹ��ð�����򣬽�����ķŵ���һ��ȥ�������㷨Ҳֻ�ܰ�����ķŵ�ǰ����
            if (Alist[y].Count > 0)
            {
                for (int i = Alist[y].Count - 1; i > 0; i--)
                {
                    float v1 = (transform.position - Alist[y][i - 1].transform.position).sqrMagnitude;
                    float v = (transform.position - Alist[y][i].transform.position).sqrMagnitude;
                    
                    //�Ƚ� �����v��ǰ���v1�Ǹ�С������Ǻ����С��������������������ǰ���С������
                    if (v < v1)
                    {
                        GameObject MinObj = Alist[y][i];
                        Alist[y][i] = Alist[y][i - 1];
                        Alist[y][i - 1] = MinObj;
                    }

                }
            }
        }


    }//������õĽ�������



    protected void OnDrawGizmos()
    {

        if (GM&& GM.GetComponent<GameController>().Sphere_Search == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, SearchRadius);
        }
      

    }//������ⷶΧ
   
}
