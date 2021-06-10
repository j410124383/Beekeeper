using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Bee
{
    //����ű����𣬾�����ǰС�۷����Ϊ״̬����ʲôΪĿ�ꡣ
    protected override void Awake()
    {
        base.Awake();
        Init();

    }


    public void Update()
    {


        Receive();

        Hunger();
        Build();
        Move();


        Die();

    }

    public void Build()
    {

        if (state != State.HUNGER)
        {
            List<GameObject> L;

            //�ڴ���Ϊ��������ȥ�ɼ�honey
            if (GetComponent<Storage>().state != Storage.State.FULL)
            {
                
                state = State.TAKEHONEY;
                L = GetComponent<Bee_Search>().Beehive_STORAGEROOM_list;
                if (L.Count > 0 )
                {
                    //��l������Ѱ�������۷�������honey��ǰ��
                    for (int i = 0; i < L.Count; i++)
                    {
                        if (L[i].GetComponent<Storage>().HONEY_list.Count > 0)
                        {
                            Target_Point = L[i].transform.position;
                            break;
                        }
                    }

                }
                else { Target_Point = TakeHoney_Point; }
            }
            //����ڴ�������ʼ����
            if (storage.WAX_list.Count>0)
            {
                //���������ȥ�µ�
                state = State.MOLD;
                //������Ӧ��Χ��ë�������������ģ�ѡ��֮ΪĿ��
                if (GetComponent<Bee_Search>().Beehive_ROUGHCAST_list.Count > 0) {
                    L = GetComponent<Bee_Search>().Beehive_ROUGHCAST_list;
                    GameObject K = L[0];
                    for (int i = 0; i < L.Count; i++)
                    {
                        if (L[i].GetComponent<Storage>().WAX_list.Count >= K.GetComponent<Storage>().WAX_list.Count)
                        {
                            K = L[i];
                        }
                    }
                    Target_Point = K.transform.position;
                }
                else
                {
                    Target_Point = Build_Point;
                }
                

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
            case State.TAKEHONEY:                //�ɻ���
                TakeHoney();
                break;
            case State.MOLD:                //ж��
                Mold();
                break;
            default:
                break;
        }
    }

    public void TakeHoney()
    {
        if (col_Obj.tag == "Beehive" && beeHive_col.state == BeeHive.BeeHiveState.STORAGEROOM)
        {
            //ֻҪ���治Ϊ�գ������Լ���Ϊ����һֱ���
            while ( storage_col.HONEY_list.Count>0 && storage.state != Storage.State.FULL )
            {
                storage_col.HONEY_list[0].transform.SetParent(transform);
                storage.Check();
                storage_col.Check();

                TakeHoney_Point = col_Obj.transform.position;
                Mark();
            }
     
        }
    }
    public void Mold()
    {
        //������beehive����ë�����д���ռ䣬��������
        if (col_Obj.tag == "Beehive" && beeHive_col.state ==BeeHive.BeeHiveState.ROUGHCAST )
        {
            //ֻҪ���治Ϊ���������Լ���Ϊ����һֱ���
            while (storage_col.state != Storage.State.FULL && storage.WAX_list.Count>0)
            {
                storage.WAX_list[0].transform.SetParent(col_Obj.transform);
                storage.Check();
                storage_col.Check();
                beeHive_col.ToSRCheck();

                Build_Point = col_Obj.transform.position;
                Mark();
            }
        }
    }


}
