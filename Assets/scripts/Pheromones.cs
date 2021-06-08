using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromones : FindGM
{
    //��Ϣ�ش��в�ͬ�����ԣ�С�۷�ͨ���ű���������ԣ�����������
    [Tooltip("����ʳ����")] public Vector3 Eat_Point;

    [Tooltip("��󾭹���ж������ ")]public Vector3 Unload_Point;
    [Tooltip("��󾭹��Ĳɼ����� ")] public Vector3 Work_Point;


    [Tooltip("����ʱ��")]public float DieTime;
    private float StartDieTime;




    //��Ϣ�ػ����٣������ٳ̶�����ɫ����ϵ
    protected override void Awake()
    {
        base.Awake();

        //�������趨������������
        GameObject par = GameObject.Find("Mark");
        if(par )
        {
            this.transform.SetParent(par.transform);
        }

        StartDieTime = DieTime;

        
    }

    private void Update()
    {
        Color c = GetComponent<SpriteRenderer>().color;
        c.a = DieTime / StartDieTime;
        GetComponent<SpriteRenderer>().color = c;


        Die();

    }

   
    private void OnDrawGizmos()  //������Ϣ������¼����
    {
        if (GM && GM.GetComponent<GameController>().Line_Pheromenes == true)
        {
            Color c = GetComponent<SpriteRenderer>().color;
            Gizmos.color = c;
            Gizmos.DrawLine(transform.position, Unload_Point);
            Gizmos.DrawLine(transform.position, Work_Point);
            Gizmos.DrawLine(transform.position, Eat_Point);
        }

    }

    void Die() //����˥��
    {
        if (GM.GetComponent<GameController>().NoDie_Pheromones != true) {
            DieTime -= Time.deltaTime;
            if (DieTime <= 0)
            {
                Destroy(this.gameObject);
            } 
        }
    }



}

