using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

namespace Lobby {
    public class PlayerList : MonoBehaviour
    {
        public List<DragableEntry> players;
        [SerializeField]
        GameObject panel;
        [SerializeField]
        public RectTransform parentTransform;
        [SerializeField]
        DragableEntry entryPrefab;
        [SerializeField]
        public float distanceBetweenEntries;
        [SerializeField]
        public float swapMargin;
        [SerializeField]
        public float smoothTime;
        [SerializeField]
        public float showSpeed;
        [SerializeField]
        public float movingBorder;


        // Start is called before the first frame update
        void Start()
        {
            players = new List<DragableEntry>();
            for(int i = 0; i < 6; i++)
            {
                AddPlayer();
            }
            showSpeed = showSpeed * Screen.height / 100;
        }

        public void AddPlayer()
        {
            DragableEntry newEntry = Instantiate(entryPrefab, parentTransform);
            newEntry.GetComponent<RectTransform>().localPosition = new Vector3(0, -distanceBetweenEntries * players.Count, 0);
            newEntry.parentList = this;
            players.Add(newEntry);
        }

        public void RemovePlayer()
        {

        }

        public void SwapEntries(int firstIndex, int secondIndex)
        {
            DragableEntry temp = players[firstIndex];
            players[firstIndex] = players[secondIndex];
            players[secondIndex] = temp;
        }
    }
}
