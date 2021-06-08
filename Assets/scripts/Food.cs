using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : FindGM
{
    //被采集后开始激活
    //越成熟越黄

    public bool IsBeActivate = false;
    public bool IsBrew = false;

    public float Brew;
    public float Brew_Max =100f;

    public Color ThisColor;


    private void Update()
    {

        //根据酿造值，判断是是否酿造完成
        if (Brew >= Brew_Max)
        {
            Brew = Brew_Max;
            IsBrew = true;
        }

        if (IsBeActivate == true)
        {
            Brew +=  Time.deltaTime*GM.TimeSpeed;
            transform.GetComponent<Collider2D>().enabled = false;
        }

        float h, s, v;
        Color.RGBToHSV(ThisColor,out h, out s, out v);

        s = Brew / Brew_Max;
        GetComponent<SpriteRenderer>().color = Color.HSVToRGB(h, s, v);

    }

    public void Die() //生命衰减
    {
        Destroy(this.gameObject);

    }

}
