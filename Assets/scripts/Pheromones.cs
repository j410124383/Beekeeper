using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromones : FindGM
{
    //信息素带有不同的属性，小蜜蜂通过脚本定义的属性，来进行区分


    //记忆体
    [Tooltip("记忆体-卸货坐标")] public Vector3 Unload_Point;
    [Tooltip("记忆体-采集坐标")] public Vector3 PickPollen_Point;
    [Tooltip("记忆体-进食坐标")] public Vector3 Eat_Point;
    [Tooltip("记忆体-拿蜜坐标")] public Vector3 TakeHoney_Point;
    [Tooltip("记忆体-建造坐标")] public  Vector3 Build_Point;

    [Tooltip("存留时间")]public float DieTime;
    public float StartDieTime ;




    //信息素会销毁，且销毁程度与颜色相联系
    protected override void Awake()
    {
        base.Awake();

        StartDieTime = DieTime;

        //出生后，设定死亡，父对象
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

   
    protected void OnDrawGizmos()  //绘制信息素所记录的线
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

    void Die() //生命衰减
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

