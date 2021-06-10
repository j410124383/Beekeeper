using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Garden : FindGM
{


    [Tooltip("��԰��С")] public float grow_size;

    [Tooltip("�����ٶ�")] public float GrowSpeed =2f;
    [Tooltip("ʣ������ʱ��")] public float GrowTime;
    protected float StartGrowTime;


    [Tooltip("��ʼ���۵�����")] public int StartGrowCount;

    [Tooltip("�������۵�Ԥ����")] protected GameObject Food_obj;


    

    //�����������
    protected  override void  Awake()
    {
        base.Awake();

        StartGrowTime = GrowTime;
        Food_obj = AssetDatabase.LoadAssetAtPath("Assets/Res/Prefab/Food.prefab", typeof(GameObject)) as GameObject;

        //��ʼ���ɻ��۵�ͼ
        for (int i=0;i<StartGrowCount;i++)
        {
            Grow();
        }


    }


    void Update()
    {
        GrowTime -= Time.deltaTime * GrowSpeed * GM.TimeSpeed;
        if (GrowTime <=0 )
        {
            GrowTime = StartGrowTime;
            Grow();
        }

    }

    void Grow()
    {
        float x1 = Random.Range(-grow_size/2,grow_size/2);
        float x2 = Random.Range(-grow_size/2,grow_size/2);
        Vector3 v1 = transform.position + new Vector3(x1,x2,0);

        GameObject food = Instantiate(Food_obj, v1, transform.rotation);
        food.transform.SetParent(this.transform);

        //print("��������");

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(grow_size, grow_size, 0));


    }

}
