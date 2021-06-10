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
    public bool HaveHoney = false;

    public int i, j;
    public int Wax_need = 4;



    private void Update()
    {
        ColorCheck();

        if (GetComponent<Storage>().HONEY_list.Count > 0)
        {
            HaveHoney = true;
        }
    }

    private void ColorCheck()
    {
        if(state == BeeHiveState.ROUGHCAST) { SR.color = UC.BeeHive_Roughcast; }
        if (state == BeeHiveState.STORAGEROOM) { SR.color = UC.BeeHive_SR_NoFill; }
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
        transform.parent.GetComponent<HoneyComb>().BG_Build();

    }



}
