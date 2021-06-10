using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class HoneyComb : FindGM
{
    //�������е���Ŀ¼
    [Tooltip("��Ѩ��Ϣ")] public List<GameObject> Beehive_list = new List<GameObject>();
  

    [Tooltip("�ػ�״̬�ķ�Ѩ")] public List<GameObject> Beehive_BG_list = new List<GameObject>();
    [Tooltip("ë��״̬�ķ�Ѩ")] public List<GameObject> Beehive_RC_list = new List<GameObject>();
    [Tooltip("�ֿ�״̬�ķ�Ѩ")] public List<GameObject> Beehive_SR_list = new List<GameObject>();

    [Tooltip("�յĵĳ�Ѩ��Ϣ")] public List<GameObject> Beehive_EMPTY_list = new List<GameObject>();
    [Tooltip("δ�յĳ�Ѩ��Ϣ")] public List<GameObject> Beehive_NOEMPTY_list = new List<GameObject>();
    [Tooltip("δ���ĳ�Ѩ��Ϣ")] public List<GameObject> Beehive_NOFULL_list = new List<GameObject>();
    [Tooltip("�����ĳ�Ѩ��Ϣ")] public List<GameObject> Beehive_FULL_list = new List<GameObject>();


    [Tooltip("�䳲Ԥ����")] protected GameObject Beehive_obj;
    [Tooltip("�䳲���")] public float Spacing=1F;

    //�����䳲��ͼ
    [SerializeField] public  GameObject[,] CombMap;
    public int CombSize;
    public int last_i, last_j;

    protected override void Awake()
    {
        base.Awake();

        Beehive_obj = AssetDatabase.LoadAssetAtPath("Assets/Res/Prefab/Beehive.prefab", typeof(GameObject)) as GameObject;

        //������ʼ��,0,0,0
        last_i = (int)Mathf.Floor(CombSize / 2);
        last_j = last_i;

        //���ɳ�ʼ�ĵ�ͼ�㣬��Щ��Ϊ��͸����͸��״
        CombMap = new GameObject[CombSize,CombSize];
        for(int i = 0; i < CombSize; i++)
        {
            for (int j = 0; j < CombSize; j++)
            {
                

                //���ɷ䳲����ʼ�������󣬵ص�
                GameObject nh = Instantiate(Beehive_obj, transform.position, transform.rotation);
                nh.transform.SetParent(this.transform);
                CombMap[i, j] = nh;
                nh.GetComponent<BeeHive>().i = i;
                nh.GetComponent<BeeHive>().j = j;

                //���н�������
                Vector3 v0 = new Vector3(Spacing * CombSize/2, Spacing * CombSize/2, 0);
                float x=0;
                if(i%2 == 0){
                    x = Spacing * j + Spacing / 2;
                }
                else if(i%2 ==1)
                {
                    x = Spacing * j;
                }
                nh.transform.Translate(new Vector3(Spacing * i, x, 0) - v0);

            }
        }



        CombMap[last_i, last_j].GetComponent<BeeHive>().state = BeeHive.BeeHiveState.STORAGEROOM;

        //Build();




    }

    private void Update()
    {

        Check();
        BG_Build();

    }

    void Check()
    {
        Beehive_list.Clear();

        Beehive_BG_list.Clear();
        Beehive_RC_list.Clear();
        Beehive_SR_list.Clear();

        Beehive_EMPTY_list.Clear();
        Beehive_NOEMPTY_list.Clear();
        Beehive_NOFULL_list.Clear();
        Beehive_FULL_list.Clear();



        //�����䳲����
        for (int i = 0; i < CombSize; i++)
        {
            for (int j = 0; j < CombSize; j++)
            {

                switch (CombMap[i, j].GetComponent<BeeHive>().state)
                {
                    case BeeHive.BeeHiveState.BASEGROUND:
                        Beehive_BG_list.Add(CombMap[i, j].gameObject);
                        break;
                    case BeeHive.BeeHiveState.ROUGHCAST:
                        Beehive_RC_list.Add(CombMap[i, j].gameObject);
                        break;
                    case BeeHive.BeeHiveState.STORAGEROOM:
                        Beehive_SR_list.Add(CombMap[i, j].gameObject);
                        break;
                    default:
                        break;
                }

                switch (CombMap[i, j].GetComponent<Storage>().state)
                {
                    case Storage.State.EMPTY:
                        Beehive_EMPTY_list.Add(CombMap[i, j].gameObject);
                        Beehive_NOFULL_list.Add(CombMap[i, j].gameObject);
                        break;
                    case Storage.State.EXIST:
                        Beehive_NOEMPTY_list.Add(CombMap[i, j].gameObject);
                        Beehive_NOFULL_list.Add(CombMap[i, j].gameObject);
                        break;
                    case Storage.State.FULL:
                        Beehive_FULL_list.Add(CombMap[i, j].gameObject);
                        Beehive_NOEMPTY_list.Add(CombMap[i, j].gameObject);
                        break;
                    default:
                        break;
                }

                Beehive_list.Add(CombMap[i, j].gameObject);
            }
        }
    }

    public void Build()
    {
        //���죬��ʵ���Ǹı�״̬������״̬����
        //����һ���µ�δ�����������飬�����������һ��
        List<GameObject> Next_List = new List<GameObject>();

        for(int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int y = 0;

                ////�ж�i���ڵ����л���˫���У���ȡ��𻯵�


                if (Mathf.Abs(i) == 1)
                {
                    if ((last_i % 2 == 0 && j == -1) || (last_i % 2 == 1 && j == 1)) //�����˫
                    {
                        y = -j;
                    }

                }


                GameObject obj = CombMap[last_i + i, last_j + y];
                if (obj.GetComponent<BeeHive>().state == BeeHive.BeeHiveState.ROUGHCAST)
                {
                    Next_List.Add(obj);
                }
            
                
            }
        }

        for(int i = 0; i < Next_List.Count; i++)
        {
            Next_List[i].SetActive(true);
        }


        //if (Next_List.Count > 0)
        //{
        //    GameObject BuildHive;
        //    BuildHive = Next_List[(int)Mathf.Round(Random.Range(0, Next_List.Count))];
        //    BuildHive.GetComponent<BeeHive>().state = BeeHive.BeeHiveState.STORAGEROOM;
        //    ObjToInt(BuildHive);
        //}
        

    }

    public void BG_Build()
    {
        if (Beehive_SR_list.Count > 0)
        {

            for (int x = 0; x < Beehive_SR_list.Count; x++)
            {
                //������ʾ����
                BeeHive beeHive = Beehive_SR_list[x].GetComponent<BeeHive>();


                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int y = 0;

                        ////�ж�i���ڵ����л���˫���У���ȡ��𻯵�
                        if (Mathf.Abs(i) == 1)
                        {
                            if ((last_i % 2 == 0 && j == -1) || (last_i % 2 == 1 && j == 1)) //�����˫
                            {
                                y = -j;
                            }

                        }
                        else { y = j; }

                        GameObject obj = CombMap[beeHive.i + i, beeHive.j + y];
                        //print((beeHive.i + i) + "," + (beeHive.j + y));
                        if (obj.GetComponent<BeeHive>().state ==BeeHive.BeeHiveState.BASEGROUND)
                        {
                            
                            obj.GetComponent<BeeHive>().state = BeeHive.BeeHiveState.ROUGHCAST;
                        }
                        Check();
                    }
                }
            }

        }


    }


    public void ObjToInt(GameObject obj)
    {
        for (int i =0;i<CombSize;i++)
        {
            for (int j =0;j< CombSize; j++)
            {

                if (CombMap[i,j] ==obj.gameObject)
                {
                    //print(obj.gameObject.name +"("+ i +","+ j+")");
                    last_i = i;
                    last_j = j;
                }
            }
        }

    }



}
