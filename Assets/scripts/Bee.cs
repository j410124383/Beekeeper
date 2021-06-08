using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : FindGM
{

    //����ű����𣬾�����ǰС�۷����Ϊ״̬����ʲôΪĿ�ꡣ

    [Tooltip("��ǰĿ�������λ��")] public Vector3 Target_Point;

    //������
    //[Tooltip("������-ж������")] public Vector3 Unload_Point;
    //[Tooltip("������-��������")] public Vector3 Work_Point;
    [Tooltip("������-��ʳ����")] public Vector3 Eat_Point;

    //�ͷ���Ϣ��
    [Tooltip("��Ϣ��Ԥ����")] public GameObject Pheromonesb_Obj;
    protected GameObject Pheromenes_Source;

    //�ƶ�
    [Tooltip("�ƶ��ٶ�")] public float MoveSpeed = 1f;
    [Tooltip("ת���ٶ�")] public float RotateSpeed = 5f;

    //����
    [Tooltip("ʣ������ʱ��")] public float DieTime;

    //����
    [Tooltip("ʣ�༢��ʱ��")] public float HungerTime;
    protected float StartHungerTime;

    //��Ϊ
    //[Tooltip("ȥ�ɼ�")] public bool ToWork;
    //[Tooltip("�ؼ�ж��")] public bool ToHive;
    //[Tooltip("ȥ�Զ���")] public bool ToEat;

    //��ײ�� ����� ����
    protected GameObject col_Obj;
    protected Storage storage ;
    protected Storage storage_col ;
    protected Food food_col ;



    protected float temp_RotateAngle = 0.0f;
    protected float temp_RotatedLerp = 0.0f;
    protected Vector3 temp_RotatedVec;

    

    public void Init()
    {

        Target_Point = transform.position;
        Eat_Point = transform.position;
        temp_RotateAngle = transform.eulerAngles.z;
        temp_RotatedVec = transform.eulerAngles;
        //�̳м���ֵ����
        StartHungerTime = HungerTime;



    }//���� ��ʼ��

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
            temp_RotatedLerp += Time.deltaTime * RotateSpeed * GM.TimeSpeed;
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

                List<GameObject> L = GetComponent<Bee_Search>().Honey_list;
                if (L.Count > 0 && L[0] )
                {
                    Target_Point = L[0].transform.position;
                }
                else { Target_Point = Eat_Point; }

            }
        }

    }//�𽥼���


    public void ToEat()
    {
        //��ʳ
        //�Ѿ���������ײ�������ʳ��Ϊ�ѷ��͵�
        if (HungerTime <= 0 && col_Obj.tag == "Food" && food_col.IsBrew == true)
        {
            //���ٷ��ۣ�ˢ�¼���ֵ
            food_col.Die();
            HungerTime = StartHungerTime;

            //�����滻����������
            Eat_Point = col_Obj.transform.position;
            //Mark();
        }

    }//����Ҫ�޸ģ���ʱʹ�ù��ı������ǵ����෽ʽ

    public void Die()
    {
        if (GC && GC.NoDie_WorkerBee != true)
        {
            if (HungerTime <= 0) { DieTime -= 2 * Time.deltaTime* GM.TimeSpeed; }
            else { DieTime -= Time.deltaTime*GM.TimeSpeed; }

            if (DieTime <= 0)
            {
                ToDie();
            }
        }

    }//��˥��

    public void ToDie()
    {
        Destroy(this.gameObject);
        //����һ����ֵ
    }//����


    public void Sleep()
    {


    }



    protected virtual void OnTriggerEnter2D(Collider2D col)
    {

    }

    protected virtual void OnDrawGizmos()
    {
        if (GM && GM.GetComponent<GameController>().Line_Target == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, Target_Point);
        }


    }
}