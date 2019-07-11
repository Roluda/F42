using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyAgent
{
    [RequireComponent(typeof(Animator))]
    public class Agent : MonoBehaviour
    {
        public static Agent Instance;

        [SerializeField]
        public GameObject inputDisablePanel;
        Animator stateMachine;

        private bool _active;
        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                if (value)
                {
                    ResourceManager.OnResourceChange += OnResourceChange;
                    ListEntry.OnEntryChange += OnListEntryChange;
                }
                else
                {
                    ResourceManager.OnResourceChange -= OnResourceChange;
                    ListEntry.OnEntryChange -= OnListEntryChange;
                }
                stateMachine.SetBool("Active", value);
                _active = value;
            }
        }

        void Awake()
        {
            stateMachine = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
            }

            Active = false;
        }

        void Update()
        {
            stateMachine.SetFloat("TimeUntilResupply", ResourceManager.Instance.timeTillSupply);
        }

        void OnResourceChange(ResourceType type, float amount)
        {
            switch (type)
            {
                case ResourceType.electricity:
                    if (amount < ResourceManager.Instance.Gas && amount < ResourceManager.Instance.Pieces)
                    {
                        stateMachine.SetBool("NeedElectricity", true);
                        stateMachine.SetBool("NeedGas", false);
                        stateMachine.SetBool("NeedPieces", false);
                    }
                    break;
                case ResourceType.gas:
                    if (amount < ResourceManager.Instance.Electricity && amount < ResourceManager.Instance.Pieces)
                    {
                        stateMachine.SetBool("NeedElectricity", false);
                        stateMachine.SetBool("NeedGas", true);
                        stateMachine.SetBool("NeedPieces", false);
                    }
                    break;
                case ResourceType.pieces:
                    if (amount < ResourceManager.Instance.Electricity && amount < ResourceManager.Instance.Gas)
                    {
                        stateMachine.SetBool("NeedElectricity", false);
                        stateMachine.SetBool("NeedGas", false);
                        stateMachine.SetBool("NeedPieces", true);
                    }
                    break;
            }
        }

        void OnListEntryChange(ListEntry entry)
        {
            if (entry.position != PlayerController.position)
            {
                return;
            }
            switch (entry.State)
            {
                case ListEntryState.electricity:
                    stateMachine.SetBool("GettingElectricity", true);
                    stateMachine.SetBool("GettingGas", false);
                    stateMachine.SetBool("GettingPieces", false);
                    break;
                case ListEntryState.gas:
                    stateMachine.SetBool("GettingElectricity", false);
                    stateMachine.SetBool("GettingGas", true);
                    stateMachine.SetBool("GettingPieces", false);
                    break;
                case ListEntryState.pieces:
                    stateMachine.SetBool("GettingElectricity", false);
                    stateMachine.SetBool("GettingGas", false);
                    stateMachine.SetBool("GettingPieces", true);
                    break;
                case ListEntryState.empty:
                    stateMachine.SetBool("GettingElectricity", false);
                    stateMachine.SetBool("GettingGas", false);
                    stateMachine.SetBool("GettingPieces", false);
                    break;
            }
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}

