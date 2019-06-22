using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyAgent
{
    public class InactiveState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Agent.Instance.inputDisablePanel.SetActive(false);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Agent.Instance.inputDisablePanel.SetActive(true);
        }
    }
}
