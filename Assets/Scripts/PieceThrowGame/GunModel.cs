using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunModel : MonoBehaviour
{

    [SerializeField]
    GameObject[] pieces;
    [SerializeField]
    GameObject piecesParent;
    [SerializeField]
    float smoothTime;
    [SerializeField]
    float rotateSpeed;
    [SerializeField]
    float initialScale;
    Vector3 smoothVector;
    Plane basePlane = new Plane(Vector3.back, new Vector3(0, 0, 0));
    Vector3 dragPivot;
    public Bounds bounds;

    public bool onBench;
    bool isDragged;
    bool gotPiece;
    bool gotElectricity;
    bool gotGas;
    bool GotPiece
    {
        get
        {
            return gotPiece;
        }
        set
        {
            gotPiece = value;
            if (gotPiece && GotElectricity&&gotGas)
            {
                AssemblyPlayer.Instance.ConstructGun();
            }
        }
    }
    bool GotElectricity
    {
        get
        {
            return gotElectricity;
        }
        set
        {
            gotElectricity = value;
            if (GotPiece && gotElectricity&&gotGas)
            {
                AssemblyPlayer.Instance.ConstructGun();
            }
        }
    }
    bool GotGas
    {
        get
        {
            return gotGas;
        }
        set
        {
            gotGas = value;
            if (GotPiece && gotElectricity&&gotGas) 
            {
                AssemblyPlayer.Instance.ConstructGun();
            }
        }
    }


    public Vector3 homeLocation = new Vector3(0, 0, 0);
    Quaternion homeRotation;
    public Quaternion HomeRotation
    {
        get
        {
            return homeRotation;
        }
        set
        {
            homeRotation = value;
            //StartCoroutine(Rotate(value));
        }
    }

    bool isDragable;
    public bool IsDragable
    {
        get
        {
            return isDragable;
        }
        set
        {
            isDragable = value;
            GetComponent<Collider>().enabled = value;
        }        
    }

    int activePieces;
    public int ActivePieces
    {
        get
        {
            return activePieces;
        }
        set
        {
            activePieces = value;
            Quaternion oldRotation = transform.rotation;
            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.identity;
            Vector3 oldParentPosition = piecesParent.transform.position;
            Vector3 oldPosition = transform.position;
            bounds = pieces[0].GetComponent<MeshRenderer>().bounds;
            for(int i = 0; i < pieces.Length; i++)
            {
                if (i < activePieces)
                {
                    pieces[i].SetActive(true);
                    bounds.Encapsulate(pieces[i].GetComponent<MeshRenderer>().bounds);
                }
                else
                {
                    pieces[i].SetActive(false);
                }
            }
            transform.position = bounds.center;
            piecesParent.transform.position = oldParentPosition;
            transform.rotation = oldRotation;
            transform.position = oldPosition;
            float scale = Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);
            transform.localScale /= scale/initialScale;
            GetComponent<BoxCollider>().size = bounds.size;
        }
    }

    bool done;
    public bool Done
    {
        get
        {
            return done;
        }
        set
        {
            done = value;
            isDragged = false;
            //StartCoroutine(SmoothMove(homeLocation));
        }
    }

    void Start()
    {
        AssemblyPlayer.OnGunConstructed += AddPart;
    }

    void Update()
    {
        if(!isDragged && (homeLocation - transform.position).sqrMagnitude > 0.003f){
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, homeLocation, ref smoothVector, smoothTime);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, HomeRotation, Time.deltaTime * rotateSpeed);
    }

    void OnMouseDown()
    {
        Debug.Log("Touch");
        Touch firstTouch = Input.GetTouch(0);
        Ray ray = Camera.main.ScreenPointToRay(firstTouch.position);
        float distance;
        if (basePlane.Raycast(ray, out distance))
        {
            dragPivot = transform.position-ray.GetPoint(distance);
        }
    }

    void OnMouseDrag()
    {
        if (!Done && !AssemblyPlayer.Instance.blockedInput)
        {
            isDragged = true;
            Touch firstTouch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(firstTouch.position);
            float distance;
            if (basePlane.Raycast(ray, out distance))
            {
                transform.position = dragPivot + ray.GetPoint(distance);
            }
        }
    }

    void OnMouseUp()
    {
        if (!Done)
        {
            isDragged = false;
            //StartCoroutine(SmoothMove(homeLocation));
        }
    }

    IEnumerator SmoothMove(Vector3 target)
    {
        while ((target - transform.position).sqrMagnitude > 0.003f)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, target, ref smoothVector, smoothTime);
            yield return null;
        }
    }

    IEnumerator Rotate(Quaternion target)
    {
        while((target.eulerAngles - transform.rotation.eulerAngles).sqrMagnitude > 0.03f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * rotateSpeed);
            yield return null;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        Piece piece = collider.GetComponent<Piece>();
        if (piece != null&&onBench && piece.wasShot)
        {
            if (piece.resourceType == ResourceType.pieces)
            {
                GotPiece = true;
                piece.Destroy();
            }else if(piece.resourceType == ResourceType.electricity)
            {
                GotElectricity = true;
                piece.Destroy();
            }else if(piece.resourceType == ResourceType.gas)
            {
                GotGas = true;
                piece.Destroy();
            }
        }
    }

    public void AddPart()
    {
        if (onBench)
        {
            ActivePieces++;
        }
    }

    public void Destroy()
    {
        AssemblyPlayer.OnGunConstructed -= AddPart;
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
