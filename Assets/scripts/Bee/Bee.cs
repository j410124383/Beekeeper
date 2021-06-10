using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bee : FindGM
{
    protected enum State
    {
        HUNGER,

        //woker
        PICKPOLLEN,
        UNLOAD,

        //builder
        TAKEHONEY,
        BREW,
        MOLD,
    }
    //����ű����𣬾�����ǰС�۷����Ϊ״̬����ʲôΪĿ�ꡣ

    [Tooltip("��ǰ״̬")] [SerializeField]protected State state = State.PICKPOLLEN;

    [Tooltip("��ǰĿ�������λ��")] public Vector3 Target_Point;

    //������
    [Tooltip("������-ж������")] [SerializeField] protected Vector3 Unload_Point;
    [Tooltip("������-�ɼ�����")] [SerializeField] protected Vector3 PickPollen_Point;
    [Tooltip("������-��ʳ����")] [SerializeField] protected Vector3 Eat_Point;
    [Tooltip("������-��������")] [SerializeField] protected Vector3 TakeHoney_Point;
    [Tooltip("������-��������")] [SerializeField] protected Vector3 Build_Point;


    //�ͷ���Ϣ��
    [Tooltip("��Ϣ��Ԥ����")] protected GameObject Pheromones_Obj;
    string phe_path = "Assets/Res/Prefab/Pheromones.prefab";

    //�ƶ�
    [Tooltip("�ƶ��ٶ�")] public float MoveSpeed = 1f;
    [Tooltip("ת���ٶ�,��ֵԽ��ת��Խ��")] public float RotateSpeed = 5f;

    //����
    [Tooltip("ʣ������ʱ��")] public float DieTime;

    //����
    [Tooltip("ʣ�༢��ʱ��")] public float HungerTime;
    protected float StartHungerTime;

    //��ײ�� ����� ����
    protected GameObject col_Obj;
    protected Storage storage ;
    protected Storage storage_col ;
    protected Food food_col ;
    protected BeeHive beeHive_col;

    protected float temp_RotateAngle = 0.0f;
    protected float temp_RotatedLerp = 0.0f;
    protected Vector3 temp_RotatedVec;

    

    public void Init()
    {

        Target_Point = transform.position;
        Eat_Point = transform.position;
        Unload_Point = transform.position;
        PickPollen_Point = transform.position;
        TakeHoney_Point = transform.position;
        Build_Point = transform.position;


        temp_RotateAngle = transform.eulerAngles.z;
        temp_RotatedVec = transform.eulerAngles;
        //�̳м���ֵ����
        StartHungerTime = HungerTime;

        storage = GetComponent<Storage>();

        Pheromones_Obj = AssetDatabase.LoadAssetAtPath("Assets/Res/Prefab/Pheromones.prefab", typeof(GameObject))as GameObject;
       

    }//���� ��ʼ��

    public void Receive()
    {
     
        List<GameObject> R = GetComponent<Bee_Search>().mark_list;
        if (R.Count > 0 && R[0])
        {
            Unload_Point = R[0].GetComponent<Pheromones>().Unload_Point;
            PickPollen_Point = R[0].GetComponent<Pheromones>().PickPollen_Point;
            Eat_Point = R[0].GetComponent<Pheromones>().Eat_Point;
            TakeHoney_Point= R[0].GetComponent<Pheromones>().TakeHoney_Point;
            Build_Point = R[0].GetComponent<Pheromones>().Build_Point;
        }
    }    //�������Ϣ�ػ�Ӱ�������Ĵ�������

    public void Move()
    {
        Vector3 v = Target_Point - transform.position;
        v.z = 0;
        
        if (Quaternion.Euler(0, 0, temp_RotateAngle) == transform.rotation)
        {
            temp_RotateAngle = Vector3.SignedAngle(Vector3.right, v, Vector3.forward);
            temp_RotatedLerp = 0.0f;
        }
        else
        {
            //��lerp��ɱ���++++++++++
            //���� Ŀ��֮��ĽǶ�
            if (temp_RotatedLerp >= 1)
            {
                temp_RotatedLerp = 1;
                transform.rotation = Quaternion.Euler(0, 0, temp_RotateAngle);
                return;
            }
            Quaternion ros = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, temp_RotateAngle), temp_RotatedLerp);
            transform.rotation = ros;
            temp_RotatedLerp += Time.deltaTime  * GM.TimeSpeed *RotateSpeed;
        }

        this.transform.Translate(Vector3.right * Time.deltaTime * MoveSpeed* GM.TimeSpeed);

    } //����Ŀ���ƶ���������


    public void Hunger()
    {
        if (GC && GC.NoHunger_WorkerBee != true)
        {
            HungerTime -= Time.deltaTime*GM.TimeSpeed;

            if (HungerTime <= 0)        //����Ǽ���״̬����ȥ�Է�
            {
                HungerTime = 0;
                state = State.HUNGER;

                List<GameObject> L =GetComponent<Bee_Search>().Beehive_NOEMPTY_list;
                if (L.Count > 0 && L[0] )
                {
                    Target_Point = L[0].transform.position;
                }
                else { Target_Point = Eat_Point; }

            }
        }

    }//�𽥼���
    public void Eat()
    {
        if (col_Obj.tag == "Beehive" && storage_col.HONEY_list[0])
        {
            //���ٷ��ۣ�ˢ�¼���ֵ
            storage_col.HONEY_list[0].GetComponent<Food>().Die();
            storage_col.Check();
            HungerTime = StartHungerTime;

            //�����滻����������
            Eat_Point = col_Obj.transform.position;
            state = State.PICKPOLLEN;

            Mark();
        }
    }




    public void Die()
    {
        if (GC && GC.NoDie_WorkerBee != true)
        {
            if (HungerTime <= 0) { DieTime -= 2 * Time.deltaTime* GM.TimeSpeed; }
            else { DieTime -= Time.deltaTime*GM.TimeSpeed; }

            if (DieTime <= 0)
            {
                Destroy(this.gameObject);
            }
        }

    }//��˥��
    protected void Mark() 
    {

        GameObject mark = Instantiate(Pheromones_Obj, transform.position, transform.rotation);
        Pheromones pheromenes = mark.GetComponent<Pheromones>();
        pheromenes.PickPollen_Point = PickPollen_Point;
        pheromenes.Unload_Point = Unload_Point;
        pheromenes.Eat_Point = Eat_Point;

    } //������Ϣ��

    protected void OnTriggerStay2D(Collider2D col)
    {
        col_Obj = col.gameObject;
        Behaviour();
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        //base.OnTriggerEnter2D( col);
        col_Obj = col.gameObject;
        Behaviour();

    } //��ײ���

    protected  virtual void Behaviour()
    {
        storage_col = col_Obj.GetComponent<Storage>();
        food_col = col_Obj.GetComponent<Food>();
        beeHive_col = col_Obj.GetComponent<BeeHive>();
    }

    protected virtual void OnDrawGizmos()
    {
        //Ŀ�ĵ���
        if (GM && GM.GetComponent<GameController>().Line_Target == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, Target_Point);
        }

        //��Ӱ����Ϣ����
        if (GM && GM.GetComponent<GameController>().Line_Pheromenes_Source == true)
        {
            if (GetComponent<Bee_Search>().mark_list[0])
            {
                GameObject z = GetComponent<Bee_Search>().mark_list[0];
                Gizmos.color = z.GetComponent<SpriteRenderer>().color;
                Gizmos.DrawLine(transform.position, z.transform.position);
            }
        }
    }
}