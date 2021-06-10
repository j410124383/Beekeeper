using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public enum State
    {
        EMPTY,
        EXIST,
        FULL
    }


    //�洢��
    public int Max_Count;
    public State state =State.EXIST;

    //ʳ��
    [Tooltip("ʳ����Ϣ")] public List<GameObject> Food_list = new List<GameObject>();
    //[Tooltip("δ�ܾ�����")] public List<GameObject> POLLEN_list = new List<GameObject>();
    [Tooltip("���ܾ�����")] public List<GameObject> POLLEN_FERTILIZED_list = new List<GameObject>();
    [Tooltip("����")] public List<GameObject> HONEY_list = new List<GameObject>();
    [Tooltip("����")] public List<GameObject> WAX_list = new List<GameObject>();

    private List<List<GameObject>> Alist;


    private void Awake()
    {
        //���߳���һ���ж��ٸ�list��Ҫ����
        Alist = new List<List<GameObject>>
        {
            Food_list,
            //POLLEN_list,
            POLLEN_FERTILIZED_list,
            HONEY_list,
            WAX_list

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

                switch (child.GetComponent<Food>().state)
                {
                    //case Food.State.POLLEN:
                    //    POLLEN_list.Add(child.gameObject);
                    //    break;
                    case Food.State.POLLEN_FERTILIZED:
                        POLLEN_FERTILIZED_list.Add(child.gameObject);
                        break;
                    case Food.State.HONEY:
                        HONEY_list.Add(child.gameObject);
                        break;
                    case Food.State.WAX:
                        WAX_list.Add(child.gameObject);
                        break;
                    default:
                        break;
                }
            }
        }

        //���洢�۷�������ֵ����full is false
        if (Food_list.Count < Max_Count && Food_list.Count > 0)
        {
            state = State.EXIST;
        }
        if(Food_list.Count ==0){
            state = State.EMPTY;
        }
        if (Food_list.Count == Max_Count)
        {
            state = State.FULL;
        }

    }


}
