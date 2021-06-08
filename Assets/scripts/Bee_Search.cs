using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee_Search : MonoBehaviour
{
    //����̽�����ڵ������������

    [Tooltip("�״������뾶")] public float SearchRadius=15f;

    [Tooltip("���")] public GameObject Queen;
    //ʳ��
    [Tooltip("ʳ����Ϣ")] public List<GameObject> Food_list = new List<GameObject>();
    [Tooltip("δ�����ʳ����Ϣ")] public List<GameObject> Pollen_list = new List<GameObject>();
    [Tooltip("��������ɵ�ʳ����Ϣ")] public List<GameObject> Honey_list = new List<GameObject>();
    //�䳲
    [Tooltip("��Ѩ��Ϣ")] public List<GameObject> Beehive_list = new List<GameObject>();
    [Tooltip("δ���ĳ�Ѩ��Ϣ")] public List<GameObject> Beehive_Nofull_list = new List<GameObject>();
    [Tooltip("�ǿյĳ�Ѩ��Ϣ")] public List<GameObject> Beehive_NoEmpty_list = new List<GameObject>();
    //��λ
    [Tooltip("������Ϣ")] public List<GameObject> Workerbee_list = new List<GameObject>();
    [Tooltip("�۷���Ϣ")] public List<GameObject> Drone_list = new List<GameObject>();


    [Tooltip("��Ϣ������")] public List<GameObject> mark_list = new List<GameObject>();

    private List<List<GameObject>> Alist;

    private GameObject GM;


    private void Awake()
    {
        //���߳���һ���ж��ٸ�list��Ҫ����
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

    //�µļ��ϵͳ��ʹ������
    private void Update()
    {
        //����gm��λ��
        if (GameObject.FindWithTag("GameManager"))
        {
            GM = GameObject.FindWithTag("GameManager");
        }



        //�״����һ��
        Check();
        Sequence();
 
    }

   
    public void Check()
    {
        //���ҷ��
        if (GameObject.FindWithTag("Queen"))
        {
            Queen = GameObject.FindWithTag("Queen");
        }
        else
        {
            Queen = null;
        }

        //������Χ�ڵ��������壬�������tag�����з���
        Collider2D[] all = Physics2D.OverlapCircleAll(this.transform.position, SearchRadius);

        if (all.Length > 0)
        {

            //���������¼
            for(int i = 0;i<Alist.Count;i++)
            {
                Alist[i].Clear();


            }



            for (int i = 0; i < all.Length; i++)
            {
                if (all[i].gameObject.tag == "Food")
                {
                    Food_list.Add(all[i].gameObject);
                    //δ����
                    if (all[i].GetComponent<Food>().IsBeActivate != true)
                    {
                        Pollen_list.Add(all[i].gameObject);
                    }
                    //���������
                    if (all[i].GetComponent<Food>().IsBrew == true)
                    {
                        Honey_list.Add(all[i].gameObject);
                    }


                }
                if (all[i].gameObject.tag == "Beehive")
                {
                    //�䳲δ��
                    if(all[i].GetComponent<Storage>().IsFull != true)
                    {
                        Beehive_Nofull_list.Add(all[i].gameObject);
                        //print("add nofull" + all[i].gameObject);
                    }
                    //�䳲��Ϊ�գ��ж���
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

    } //�״����һ�������з���

    public void Sequence() {



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



    private void OnDrawGizmos()
    {

        if (GM&& GM.GetComponent<GameController>().Sphere_Search == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, SearchRadius);
        }
      

    }//������ⷶΧ
   
}
