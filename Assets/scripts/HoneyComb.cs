using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HoneyComb : MonoBehaviour
{
    //�������е���Ŀ¼
    [Tooltip("�䳲����")] public List<GameObject> Beehive_list = new List<GameObject>();
    [Tooltip("�����۵ķ䳲����")] public List<GameObject> Beehive_full_list = new List<GameObject>();

    [Tooltip("����")] [SerializeField] private float full_Per;

    [Tooltip("�䳲Ԥ����")] public GameObject Beehive_obj;


    //�����䳲��ͼ
    [SerializeField] public  int[,] CombMap;
    public int CombSize;


    private void Awake()
    {
        //�ж��Ƿ�Ϊ����������ż�����һ
        if (CombSize % 2 == 0)
        {
            CombSize++;
        }

 

    }

    private void Update()
    {

       


        Check();

        full_Per = Beehive_full_list.Count / Beehive_list.Count ;
        if (full_Per>0.9f)
        {
            GameObject newhive = Instantiate(Beehive_obj, transform.position, transform.rotation);
            newhive.transform.SetParent(this.transform);
        }
    }

    void Check()
    {
        Beehive_list.Clear();
        Beehive_full_list.Clear();

        //�������������壬���ɷ䳲
        foreach(Transform child in this.transform)
        {
           
            if (child.gameObject.tag == "Beehive")
            {
                Beehive_list.Add(child.gameObject);
                if (child.gameObject.GetComponent<Storage>().IsFull == true)
                {
                    Beehive_full_list.Add(child.gameObject);
                }

            }
        }
    }

    void Build()
    {

    }


}
