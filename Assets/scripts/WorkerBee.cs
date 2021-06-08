using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBee : Bee
{
    //����ű����𣬾�����ǰС�۷����Ϊ״̬����ʲôΪĿ�ꡣ


    //������
    [Tooltip("������-ж������")] public Vector3 Unload_Point;
    [Tooltip("������-��������")] public Vector3 Work_Point;


    protected override void Awake()
    {
        base.Awake();
        Init();

        Unload_Point = transform.position;
        Work_Point = transform.position;
    }


    public void Update()
    {

        //�������Ϣ�ػ�Ӱ�������Ĵ�������
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

        //�ڴ����ˣ���ȥ�ɼ������˾ͻ�ȥж��
        if (HungerTime > 0)
        {
            if (GetComponent<Storage>().IsFull == false)
            {
                //�ɼ�
                List<GameObject> L = GetComponent<Bee_Search>().Pollen_list;
                if (L.Count > 0 && L[0] != null)
                {
                    Target_Point = L[0].transform.position;
                }
                else { Target_Point = Work_Point; }
            }
            if (GetComponent<Storage>().IsFull == true)
            {
                //�ؼ�
                List<GameObject> L = GetComponent<Bee_Search>().Beehive_Nofull_list;
                if (L.Count > 0)
                {
                    Target_Point = L[0].transform.position;
                }
                else { Target_Point = Unload_Point; }
            }
        }
        
    }//�������еĹ�����Ϊ

 
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D( col);

        col_Obj = col.gameObject;
        Storage storage = GetComponent<Storage>();
        Storage storage_col = col.GetComponent<Storage>();
        Food food_col = col.GetComponent<Food>();

        if (HungerTime <= 0)
        {
            //�����ײ���Ƿ䳲���ҷ䳲�����ѳ���ķ���
            if (HungerTime <= 0 && col_Obj.tag == "Beehive" && storage_col.Honey_list.Count>0)
            {
                //���ٷ��ۣ�ˢ�¼���ֵ
                storage_col.Honey_list[0].GetComponent<Food>().Die();
                storage_col.Check();
                HungerTime = StartHungerTime;

                //�����滻����������
                Eat_Point = col_Obj.transform.position;
                Mark();
            }
            
        }
        else
        {
            //����
            if (col_Obj.tag == "Food" && storage.IsFull != true
                && food_col.IsBeActivate != true)
            {

                //�Ե����ۺ󣬴��浽��θ��,��ʼ����
                col_Obj.transform.SetParent(this.transform);
                food_col.IsBeActivate = true;
                //�����滻����������
                Work_Point = col_Obj.transform.position;
                Mark();
            }

            //ж��
            if (col_Obj.tag == "Beehive" && storage.IsFull == true)
            {

                //�������䳲�ˣ��ѷ��۵�����Ѩ��
                //��Ҫ�ȼ��㣬�۳���ʣ��ռ䣬ѭ��ִ�У�ֱ�����۳������
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


    } //��ײ���


    public void Mark() //������Ϣ��
    {

        GameObject mark = Instantiate(Pheromonesb_Obj,transform.position, transform.rotation);
        Pheromones pheromenes = mark.GetComponent<Pheromones>();
        pheromenes.Work_Point = Work_Point;
        pheromenes.Unload_Point = Unload_Point;
        pheromenes.Eat_Point = Eat_Point;

    }

    //����Ŀ��·��
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (GM && GM.GetComponent<GameController>().Line_Pheromenes_Source == true)
        {
            //��Ϣ������Ӱ���˱���
            if (Pheromenes_Source)
            {
                Gizmos.color = Pheromenes_Source.GetComponent<SpriteRenderer>().color;
                Gizmos.DrawLine(transform.position, Pheromenes_Source.transform.position);
            }
        }

    }






}
