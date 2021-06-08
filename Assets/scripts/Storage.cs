using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    //�洢��
    public int Max_Count;
    [Tooltip("�Ƿ�Ϊ��")] public bool IsFull = false;
    [Tooltip("�Ƿ�Ϊ��")] public bool IsEmpty = true;

    //ʳ��
    [Tooltip("ʳ����Ϣ")] public List<GameObject> Food_list = new List<GameObject>();
    [Tooltip("δ�����ʳ����Ϣ")] public List<GameObject> Pollen_list = new List<GameObject>();
    [Tooltip("��������ɵ�ʳ����Ϣ")] public List<GameObject> Honey_list = new List<GameObject>();

    private List<List<GameObject>> Alist;


    private void Awake()
    {
        //���߳���һ���ж��ٸ�list��Ҫ����
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

    public void Check()  //����Լ��Ŀڴ�
    {

        //���������¼
        for (int i = 0; i < Alist.Count; i++)
        {
            Alist[i].Clear();
        }

        //��������������
        foreach (Transform child in this.transform)
        {


            if (child.gameObject.tag == "Food")
            {
                Food_list.Add(child.gameObject);

                //δ����
                if (child.GetComponent<Food>().IsBeActivate != true)
                {
                    Pollen_list.Add(child.gameObject);
                }
                //���������
                if (child.GetComponent<Food>().IsBrew == true)
                {
                    Honey_list.Add(child.gameObject);
                }
            }
        }

        //���洢�۷�������ֵ����full is false
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
