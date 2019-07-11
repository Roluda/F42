﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyAgent
{
    public class AssembleGun : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AssemblyPlayer.Instance.ConstructGun();
        }
    }
}
