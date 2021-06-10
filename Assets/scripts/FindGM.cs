using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FindGM : MonoBehaviour
{

    protected GameManager GM;
    protected GameController GC;
    protected UIColor UC;

    protected SpriteRenderer SR;

    protected virtual void Awake()
    {
        //≤È’“gmµƒŒª÷√
        if (GameObject.FindWithTag("GameManager"))
        {
            GM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            GC = GM.GetComponent<GameController>();
            UC = GM.GetComponent<UIColor>();
        }


        if (transform.GetComponent<SpriteRenderer>())
        {
            SR = transform.GetComponent<SpriteRenderer>();
        }
    }


}
