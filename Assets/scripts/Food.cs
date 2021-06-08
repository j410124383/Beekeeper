using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : FindGM
{
    //���ɼ���ʼ����
    //Խ����Խ��

    public bool IsBeActivate = false;
    public bool IsBrew = false;

    public float Brew;
    public float Brew_Max =100f;

    public Color ThisColor;


    private void Update()
    {

        //��������ֵ���ж����Ƿ��������
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

    public void Die() //����˥��
    {
        Destroy(this.gameObject);

    }

}
