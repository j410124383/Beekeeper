using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : FindGM
{
    public enum State
    {

        POLLEN, //δ�ܾ��Ļ���
        POLLEN_FERTILIZED, //���ܾ��Ļ���
        HONEY, //���ۣ�����ʳ�ã�Ҳ������������
        WAX, //����

    }

    //���ɼ���ʼ����
    //Խ����Խ��
    public State state = State.POLLEN;

    [SerializeField]protected float ToHoney;
    public float ToHoney_Max =30f;

    [SerializeField] protected float ToWax;
    public float ToWax_Max = 30f;




    protected void Update()
    {

        //��������ֵ���ж����Ƿ��������
        if (state == State.POLLEN_FERTILIZED)
        {

            this.ToHoney +=  Time.deltaTime*GM.TimeSpeed;
            transform.GetComponent<CircleCollider2D>().enabled = false;
        }

        if (ToHoney >= ToHoney_Max)
        {
            ToHoney = ToHoney_Max;
            state = State.HONEY;
            
        }

        Ferment();

        ColorCheck();
        if(transform.parent.GetComponent<SpriteRenderer>())
        SR.sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder;

    }

    public void Ferment()
    {
        //�������Ƿ���״̬���Ҹ������ǽ�����ʱ����ʼ�����
        if (state == State.HONEY && transform.parent.tag == "Builder")
        {
            //print(name + "��������");
            ToWax += Time.deltaTime * GM.TimeSpeed;
        }

        if (ToWax >= ToWax_Max)
        {
            ToWax = ToWax_Max;
            state = State.WAX;
        }
    }

    public void ColorCheck()
    {

        switch (state)
        {
            case State.POLLEN:
                SR.color = UC.Food_Pollen;
                break;
            case State.POLLEN_FERTILIZED:
                SR.color = Color.Lerp(UC.Food_Pollen, UC.Food_Honey, ToHoney / ToHoney_Max);
                break;
            case State.HONEY:
                SR.color = Color.Lerp(UC.Food_Honey, UC.Food_Wax, ToWax / ToWax_Max);
                break;
            case State.WAX:
                SR.color = UC.Food_Wax;
                break;
            default:
                break;
        }
    }

    public void Die() //����˥��
    {
        Destroy(this.gameObject);

    }

}
