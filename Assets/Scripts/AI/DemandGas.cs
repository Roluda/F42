using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemandGas : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Demand Gas");
        SupplyList.Instance.photonView.RPC("ChangeState", Photon.Pun.RpcTarget.MasterClient, PlayerController.position, ListEntryState.gas);
    }
}
