using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby
{
    public class DragableEntry : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public PlayerList parentList;
        public bool moving = false;

        Vector3 currentVelocity;

        int Position
        {
            get
            {
                return parentList.players.IndexOf(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("Startet Drag");
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 horizontalBorder = new Vector2(parentList.parentTransform.position.x - parentList.movingBorder, parentList.parentTransform.position.x + parentList.movingBorder);
            transform.position = new Vector2(Mathf.Clamp(eventData.position.x, horizontalBorder.x, horizontalBorder.y), eventData.position.y);
            foreach (DragableEntry otherEntry in parentList.players)
            {
                float verticalTransformDelta = otherEntry.transform.position.y - transform.position.y;
                if ((verticalTransformDelta + parentList.swapMargin > 0 && otherEntry.Position > Position) || verticalTransformDelta - parentList.swapMargin < 0 && otherEntry.Position < Position)
                {
                    if (!otherEntry.moving)
                    {
                        Debug.Log("Swap Position with Entry");
                        parentList.SwapEntries(Position, otherEntry.Position);
                        otherEntry.MoveToPosition();
                    }
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            MoveToPosition();
        }

        public void MoveToPosition()
        {
            moving = true;
            Debug.Log(name + " is moving");
            Vector3 target = new Vector3(0, -parentList.distanceBetweenEntries*Position, 0);
            StartCoroutine(MoveTransform(GetComponent<RectTransform>(), target));
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator MoveTransform(RectTransform rect, Vector3 target)
        {
            while ((rect.localPosition - target).sqrMagnitude >= 1f && moving)
            {
                yield return null;
                rect.localPosition = Vector3.SmoothDamp(rect.localPosition, target, ref currentVelocity, parentList.smoothTime, parentList.showSpeed);
            }
            moving = false;
        }
    }
}
