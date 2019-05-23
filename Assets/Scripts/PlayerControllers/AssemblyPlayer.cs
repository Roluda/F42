using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyPlayer : PlayerController
{
    [SerializeField]
    Vector3Int initialResources;
    [SerializeField]
    Vector3Int assembleCosts;
    public override void CustomSetup()
    {
        Electricity = initialResources.x;
        Gas = initialResources.y;
        Pieces = initialResources.z;
    }

    public void ConstructGun()
    {
        if (currentGun == null)
        {
            return;
        }
        if (currentGun.Completion < position)
        {
            int costElec = assembleCosts.x + Random.Range(0, assembleCosts.x);
            int costGas = assembleCosts.y + Random.Range(0, assembleCosts.y);
            int costPie = assembleCosts.z + Random.Range(0, assembleCosts.z);
            if (Electricity >= assembleCosts.x && Gas > assembleCosts.y && Pieces > assembleCosts.z)
            {
                currentGun.Completion++;
                Electricity -= costElec;
                Gas -= costGas;
                Pieces -= costPie;
            }
        }
    }
    
    public void RemoveRandomResources()
    {
        Electricity -= Random.Range(1, Electricity / 10);
        Gas -= Random.Range(1, Gas / 10);
        Electricity -= Random.Range(1, Electricity / 10);
    }
}
