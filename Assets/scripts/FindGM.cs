using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FindGM : MonoBehaviour
{

    protected GameManager GM;
    protected GameController GC;

    protected virtual void Awake()
    {
        //≤È’“gmµƒŒª÷√
        if (GameObject.FindWithTag("GameManager"))
        {
            GM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            GC = GM.GetComponent<GameController>();
        }
    }


}
