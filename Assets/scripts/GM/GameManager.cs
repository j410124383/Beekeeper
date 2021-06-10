using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum Season{
        SPRING,
        SUMMER,
        AUTUMN,
        WINTER

    }

    [Tooltip("������Ԥ����")] public GameObject InsOBJ ;
    [Tooltip("ʳ��Ԥ����")] public GameObject Food_InsObj;
    [Tooltip("����Ԥ����")] public GameObject Worker_InsObj;



    //��Ϣ����
    public int TimeSpeed = 1;
    [Tooltip("Сʱ����������")] public float[] bee_Time = new float[3] { 0, 1, 1};
    public Season season = Season.SPRING;

    [Tooltip("���������")] public GameObject[] WorkerBee_Desks;
    [Tooltip("��������")] public GameObject[] Queen_Desks;
    [Tooltip("��Ϣ�ص�����")] public GameObject[] Mark_Desks;
    [Tooltip("�䳲������")] public GameObject[] BeeHive_Desks;
    [Tooltip("���۵�����")] public GameObject[] Food_Desks;



    private string[] TagName = new string[5]{
            "WorkerBee",
            "Queen",
            "Mark",
            "Beehive",
            "Food"
    };


    public Text text;



    void Update()
    {
        //ʱ��ϵͳ
        bee_Time[0] += Time.deltaTime* TimeSpeed;
        if (bee_Time[0]>= 24)
        {
            bee_Time[0] = 0 ;
            bee_Time[1]++;
        }
        if (bee_Time[1]>= 365)
        {
            bee_Time[1] = 0;
            bee_Time[2]++;
        }


        //�����ж�
        float day = bee_Time[1];
        if (day >= 0 && day < 90)
        {
            season = Season.SPRING;
        }
        if (day >= 90 && day < 180)
        {
            season = Season.SUMMER;
        }
        if (day >= 180 && day < 270)
        {
            season = Season.AUTUMN;
        }
        if (day >= 270 && day < 360)
        {
            season = Season.WINTER;
        }



        if (Input.GetMouseButtonDown(1) &&InsOBJ )
        {
            
            Vector3 v1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            v1.z = 0;
            GameObject instance = Instantiate(InsOBJ, v1, transform.rotation);
        }


        Check();

        Print();

    }

    private List<GameObject[]> ObjList;
    private string ListString;

    public void Check()
    {


        ////���߳���һ���ж��ٸ�list��Ҫ����
        //ObjList = new List<GameObject[]>
        //{
        //    WorkerBee_Desks,
        //    Queen_Desks,
        //    Mark_Desks,
        //    BeeHive_Desks,
        //    Food_Desks,
        //};

        //for (int i = 0; i < ObjList.Count; i++)
        //{
        //    if (GameObject.FindGameObjectsWithTag(TagName[i]) !=null)
        //    {
        //        ObjList[i] = GameObject.FindGameObjectsWithTag(TagName[i]);
        //        //print("�ҵ�"+TagName[i]+"����");
        //    }

        //}
    }

    void Print()
    {
        //ListString = null;
        //for(int i = 0; i < ObjList.Count; i++)
        //{
        //    ListString += "\n" + TagName[i] + "_Count " + ObjList[i].Length;
        //}




        //��ӡ��������Ϣ
        text.text = "Hour "+ Mathf.Floor(bee_Time[0])
            + "\n"+ "Day " + bee_Time[1]
            + "\n" + "Season " + season
            + "\n" + "Year " + bee_Time[2]

          /*  + "\n" + ListString*/;

    }
}
