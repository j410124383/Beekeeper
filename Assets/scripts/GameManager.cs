using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Tooltip("������Ԥ����")] public GameObject[] InsOBJ ;
    [Tooltip("��ǰѡ��Ԥ����")] [SerializeField] private GameObject NowInsOBJ;


    //��Ϣ����
    public int TimeSpeed = 1;
    [Tooltip("Сʱ����������")] public float[] bee_Time = new float[4] { 0, 1, 1,0};
    [Tooltip("����")] private string[] Season =new string[4] { "Spring" , "Summer" , "Autumn" , " Winter" };

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
            bee_Time[3] = 0;
        }
        if (day >= 90 && day < 180)
        {
            bee_Time[3] = 1;
        }
        if (day >= 180 && day < 270)
        {
            bee_Time[3] = 2;
        }
        if (day >= 270 && day < 360)
        {
            bee_Time[3] = 3;
        }


        //ѡ�����ּ�����Ӧ���ɵ�����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            NowInsOBJ = null;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NowInsOBJ = InsOBJ[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            NowInsOBJ = InsOBJ[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            NowInsOBJ = InsOBJ[2];
        }


        if (Input.GetMouseButtonDown(0) &&NowInsOBJ )
        {
            
            Vector3 v1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            v1.z = 0;
            GameObject instance = Instantiate(NowInsOBJ, v1, transform.rotation);
        }


        Check();

        Print();

    }

    private List<GameObject[]> ObjList;
    private string ListString;

    public void Check()
    {


        //���߳���һ���ж��ٸ�list��Ҫ����
        ObjList = new List<GameObject[]>
        {
            WorkerBee_Desks,
            Queen_Desks,
            Mark_Desks,
            BeeHive_Desks,
            Food_Desks,
        };




        for (int i = 0; i < ObjList.Count; i++)
        {

            ObjList[i] = GameObject.FindGameObjectsWithTag(TagName[i]);
            //print("�ҵ�"+TagName[i]+"����");


        }
    }

    void Print()
    {
        ListString = null;
        for(int i = 0; i < ObjList.Count; i++)
        {
            ListString += "\n" + TagName[i] + "_Count " + ObjList[i].Length;
        }

        string nowuse ="Null";
        if (NowInsOBJ!=null) {
            nowuse = NowInsOBJ.name;
        }


        //��ӡ��������Ϣ
        text.text = "Hour "+ Mathf.Floor(bee_Time[0])
            + "\n"+ "Day " + bee_Time[1]
            + "\n" + "Season " + Season[(int)bee_Time[3]]
            + "\n" + "Year " + bee_Time[2]
            + "\n" + "\n" + nowuse

            + "\n" + ListString;

    }
}
