using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Bee
{
    //����ű����𣬾�����ǰС�۷����Ϊ״̬����ʲôΪĿ�ꡣ
    [Tooltip("������-�ɼ�����")] [SerializeField] protected Vector3 PickPollen_Point;
    [Tooltip("������-ж������")] [SerializeField] protected Vector3 Unload_Point;


    protected override void Awake()
    {
        base.Awake();
        Init();

    }


    public void Update()
    {

        Hunger();
        Work();
        Move();


        Receive();


        Die();

    }

    public void Work()
    {

        //�ڴ����ˣ���ȥ�ɼ������˾ͻ�ȥж��
        if (state != State.HUNGER)
        {
            List<GameObject> L;

            if (GetComponent<Storage>().state != Storage.State.FULL)
            {
                //�ɼ�
                state = State.PICKPOLLEN;
                L = GetComponent<Bee_Search>().Food_list;
                if (L.Count > 0 && L[0] != null)
                {
                    Target_Point = L[0].transform.position;
                }
                else { Target_Point = PickPollen_Point; }
            }
            if (GetComponent<Storage>().state == Storage.State.FULL)
            {
                //�ؼ�
                state = State.UNLOAD;
                L = GetComponent<Bee_Search>().Beehive_STORAGEROOM_list;
                if (L.Count > 0)
                {
                    Target_Point = L[0].transform.position;
                }
                else { Target_Point = Unload_Point; }
            }
        }

    }//�������еĹ�����Ϊ

    protected override void Behaviour()
    {
        base.Behaviour();

        switch (state)
        {
            case State.HUNGER:
                Eat();
                break;
            case State.PICKPOLLEN:                //�ɻ���
                PickPollen();
                break;
            case State.UNLOAD:                //ж��
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

            //�Ե����ۺ󣬴��浽��θ��,��ʼ����
            col_Obj.transform.SetParent(this.transform);
            food_col.state = Food.State.POLLEN_FERTILIZED;
            //�����滻����������
            PickPollen_Point = col_Obj.transform.position;
            Mark();
        }
    }
    public void Unload()
    {
        if (col_Obj.tag == "Beehive" && beeHive_col.state == BeeHive.BeeHiveState.STORAGEROOM)
        {

            //�������䳲�ˣ��ѷ��۵�����Ѩ��
            //��Ҫ�ȼ��㣬�۳���ʣ��ռ䣬ѭ��ִ�У�ֱ�����۳������
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

    protected override void Mark()
    {
        base.Mark();
        phe_m.GetComponent<Pheromones>().PickPollen_Point = PickPollen_Point;
        phe_m.GetComponent<Pheromones>().Unload_Point = Unload_Point;

    }
    protected override void Receive()
    {
        base.Receive();

        if (phe_r)
        {
            PickPollen_Point = phe_r.GetComponent<Pheromones>().PickPollen_Point;
            Unload_Point = phe_r.GetComponent<Pheromones>().Unload_Point;
        }
        



    }
}
