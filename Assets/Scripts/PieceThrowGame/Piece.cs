using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Piece : MonoBehaviour
{
    [SerializeField]
    LineRenderer render;
    [SerializeField]
    float maxPowerDistance;
    [SerializeField]
    float maxPower;
    [SerializeField]
    float minimumPowerRatio;
    float currentDistance;
    Vector3 dragStart;
    Vector3 currentDragPoint;

    public bool wasShot;

    public event UnityAction OnPieceDestroyed;

    [SerializeField]
    public ResourceType resourceType;
    public bool damaged;

    Plane basePlane = new Plane(Vector3.back, new Vector3(0, 0, 0));

    Rigidbody rbody;

    void Awake()
    {
        rbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Destroy itself
    }

    void OnMouseDrag()
    {
        if (!AssemblyPlayer.Instance.blockedInput)
        {
            Touch firstTouch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(firstTouch.position);
            float distance;
            if (basePlane.Raycast(ray, out distance))
            {
                currentDragPoint = ray.GetPoint(distance);
                currentDistance = (currentDragPoint - dragStart).magnitude;
                render.SetPosition(1, currentDragPoint);
                render.endColor = new Color(currentDistance / maxPowerDistance, 0, 0);
                if (currentDistance >= maxPowerDistance)
                {
                    Shoot((currentDragPoint - dragStart).normalized);
                }
            }
        }
    }

    void OnMouseDown()
    {

        Touch firstTouch = Input.GetTouch(0);
        Ray ray = Camera.main.ScreenPointToRay(firstTouch.position);
        float distance;
        if (basePlane.Raycast(ray, out distance))
        {
            dragStart = ray.GetPoint(distance);
        }
        render.SetPosition(0, transform.position);
    }

    void OnMouseUp()
    {
        render.endColor = Color.clear;
        if (!AssemblyPlayer.Instance.blockedInput)
        {
            Shoot(-(currentDragPoint - dragStart).normalized);
        }
    }

    void Shoot(Vector3 direction)
    {
        render.endColor = Color.clear;
        if (!wasShot)
        {
            wasShot = true;
            rbody.AddForce(direction * Mathf.Max(minimumPowerRatio, (currentDistance / maxPowerDistance)) * maxPower, ForceMode.Impulse);
            Invoke("Destroy", 7);
        }
    }

    public void Disappear()
    {

    }

    public void Destroy()
    {
        OnPieceDestroyed?.Invoke();
        Destroy(gameObject);
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("PlayArea"))
        {
            Destroy();
        }
    }
}
