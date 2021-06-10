using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromones : FindGM
{
    //��Ϣ�ش��в�ͬ�����ԣ�С�۷�ͨ���ű���������ԣ�����������


    //������
    [Tooltip("������-ж������")] public Vector3 Unload_Point;
    [Tooltip("������-�ɼ�����")] public Vector3 PickPollen_Point;
    [Tooltip("������-��ʳ����")] public Vector3 Eat_Point;
    [Tooltip("������-��������")] public Vector3 TakeHoney_Point;
    [Tooltip("������-��������")] public  Vector3 Build_Point;

    [Tooltip("����ʱ��")]public float DieTime;
    public float StartDieTime ;




    //��Ϣ�ػ����٣������ٳ̶�����ɫ����ϵ
    protected override void Awake()
    {
        base.Awake();

        StartDieTime = DieTime;

        //�������趨������������
        GameObject par = GameObject.Find("Mark");
        if(par )
        {
            this.transform.SetParent(par.transform);
        }
        
    }

    private void Update()
    {
        Color c = SR.color;
        c.a = DieTime / StartDieTime;
        SR.color = c;


        Die();

    }

   
    protected void OnDrawGizmos()  //������Ϣ������¼����
    {
        if (GM && GM.GetComponent<GameController>().Line_Pheromenes == true)
        {
            Color c = GetComponent<SpriteRenderer>().color;
            Gizmos.color = c;
            Gizmos.DrawLine(transform.position, Unload_Point);
            Gizmos.DrawLine(transform.position, PickPollen_Point);
            Gizmos.DrawLine(transform.position, Eat_Point);
        }

    }

    void Die() //����˥��
    {
        if (GM.GetComponent<GameController>().NoDie_Pheromones != true) {
            DieTime -= Time.deltaTime*GM.TimeSpeed;
            if (DieTime <=0)
            {
                Destroy(this.gameObject);
            } 
        }
    }



}

