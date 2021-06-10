using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeHive : FindGM
{

    public enum BeeHiveState
    {
        BASEGROUND,
        ROUGHCAST,
        STORAGEROOM,
    }

    public BeeHiveState state = BeeHiveState.BASEGROUND;

    public int i, j;
    public int Wax_need = 4;



    private void Update()
    {
        ColorCheck();
        ToSRCheck();

        //¸÷ÖÖ×´Ì¬
        switch (state)
        {
            case BeeHiveState.BASEGROUND:
                GetComponent<CircleCollider2D>().enabled = false;
                SR.color = UC.BeeHive_BG;
                break;
            case BeeHiveState.ROUGHCAST:
                GetComponent<CircleCollider2D>().enabled = true;
                SR.color = UC.BeeHive_Roughcast;
                break;
            case BeeHiveState.STORAGEROOM:
                GetComponent<CircleCollider2D>().enabled = true;
                SR.color = UC.BeeHive_SR_NoFill;
                break;
            default:
                break;
        }

    }

    public void ColorCheck()
    {

        switch (state)
        {
            case BeeHiveState.ROUGHCAST:
                SR.color = UC.BeeHive_Roughcast;
                break;
            case BeeHiveState.STORAGEROOM:
                SR.color = UC.BeeHive_SR_NoFill;
                break;
            default:
                break;
        }
    }


    public void ToSRCheck()
    {
        if (state == BeeHiveState.ROUGHCAST && GetComponent<Storage>().WAX_list.Count >= Wax_need)
        {
            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }
            state = BeeHiveState.STORAGEROOM;
        }
    }
}
