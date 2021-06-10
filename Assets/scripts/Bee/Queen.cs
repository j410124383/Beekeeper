using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Queen : MonoBehaviour
{
    //����ű����𣬾�����ǰС�۷����Ϊ״̬����ʲôΪĿ�ꡣ

    [Tooltip("��ǰĿ�������λ��")] public Vector3 Target_Point;

    //������
    //[Tooltip("������-ж������")] public Vector3 Unload_Point;
    [Tooltip("������-��������")] public Vector3 Born_Point;
    [Tooltip("������-��ʳ����")] public Vector3 Eat_Point;

    [Tooltip("�ƶ��ٶ�")] public float MoveSpeed = 1f;

    [Tooltip("ʣ������ʱ��")] public float DieTime;
    [Tooltip("ʣ�༢��ʱ��")] public float HungerTime;
    private float StartHungerTime;

    [Tooltip("ʣ�༢��ʱ��")] public float BornTime;
    private float StartBornTime;

    [Tooltip("����Ԥ����")] public GameObject WorkerBee_InsOBJ;
    [Tooltip("�۷�Ԥ����")] public GameObject Drone_InsOBJ;
    //[Tooltip("�����ٶ�")] public float BornSpeed;

    private GameObject GM;

    private void Awake()
    {
        //���ø�������
        Target_Point = transform.position;
        Born_Point = transform.position;
        Eat_Point = transform.position;

        //���ø���ʱ��
        StartHungerTime = HungerTime;
        StartBornTime = BornTime;

        //����gm��λ��
        if (GameObject.FindWithTag("GameManager"))
        {
            GM = GameObject.FindWithTag("GameManager");
        }

    }


    private void Update()
    {
        Action();
        Move();

        Hunger();

        Born();
    }




    public void Action()
    {
        //����Ǽ���״̬����ȥ�Է���������
        //if (HungerTime <= 0)
        //{
        //    //��ʳ
        //    List<GameObject> L = GetComponent<Bee_Search>().HONEY_list;
        //    if (L.Count > 0 && L[0] != null)
        //    {
        //        Target_Point = L[0].transform.position;
        //    }
        //    else { Target_Point = Eat_Point; }

        //}
        //else
        //{
        //    //�����Ļ���������
        //    if (BornTime<=0)
        //    {
        //        //�ؼ�
        //        List<GameObject> L = GetComponent<Bee_Search>().Beehive_list;
        //        if (L.Count > 0)
        //        {
        //            Target_Point = L[0].transform.position;
        //        }
        //        else { Target_Point = Born_Point; }
        //    }
        //}






    }//ָ����ΪĿ�꣬������δ����Ŀ����food���洢������Ŀ���Ǽ���

    private void OnTriggerEnter2D(Collider2D col)
    {
        Food food_col = col.GetComponent<Food>();
        //��ʳ
        if (col.tag == "Food" && HungerTime <= 0 && food_col.state==Food.State.HONEY)
        {
            //���ٷ��ۣ�ˢ�¼���ֵ
            food_col.Die();
            HungerTime = StartHungerTime;

            //�����滻����������
            Eat_Point = col.transform.position;
          
        }

        //����
        if (col.tag == "Beehive" && BornTime <=0)
        {

            //�������䳲��
                BornTime = StartBornTime;
                Vector3 v1 = transform.position + Vector3.right;
                GameObject instance = Instantiate(WorkerBee_InsOBJ, v1, transform.rotation);
            


            ////��Ҫ�ȼ��㣬�۳���ʣ��ռ䣬ѭ��ִ�У�ֱ�����۳������
            //if (storage.Food_list.Count > 0)
            //{
            //    while (storage.IsEmpty != true && storage_col.IsFull != true)
            //    {

            //        storage.Food_list[0].transform.SetParent(col.transform);
            //        storage.Check();
            //        storage_col.Check();

            //        Unload_Point = col.transform.position;
            //        Mark();
            //    }


            //}
        }




    } //��ײ���

    public void Move()
    {

        Vector3 v = Target_Point - transform.position;
        v.z = 0;
        float angle = Vector3.SignedAngle(Vector3.right, v, Vector3.forward);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, angle), 1f);
        this.transform.Translate(Vector3.right * Time.deltaTime * MoveSpeed);
    } //����Ŀ���ƶ���������

    void Hunger()
    {
        if (GM.GetComponent<GameController>().NoDie_WorkerBee != true)
        {
            HungerTime -= Time.deltaTime;
            if (HungerTime <= 0)
            {
                HungerTime = 0;

            }
        }
    }


    public void Born()
    {
        BornTime -= Time.deltaTime;
        if (BornTime <= 0)
        {
            BornTime = 0;

        }
    }



}
