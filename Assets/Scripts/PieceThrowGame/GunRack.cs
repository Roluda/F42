using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRack : MonoBehaviour
{
    [SerializeField]
    GunModel fourPlayerPrefab;
    [SerializeField]
    GunModel fivePlayerPrefab;
    [SerializeField]
    GunModel sixPlayerPrefab;
    [SerializeField]
    int rackSize;
    [SerializeField]
    float spaceBetween;
    [SerializeField]
    Vector3 initialRotation;

    Plane basePlane = new Plane(Vector3.back, new Vector3(0, 0, 0));
    GunModel gunPrefab;

    GunModel[] guns;
    Transform[] rackSlots;

    // Start is called before the first frame update
    void Start()
    {
        AssemblyPlayer.OnAssembleStartet += FillUp;
        WeaponWorkload.OnGunReceived += FillLast;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/15, Screen.height/2));
        float distance;
        if (basePlane.Raycast(ray, out distance))
        {
            transform.position = ray.GetPoint(distance);
        }

        switch (PlayerController.assemblyPlayers)
        {
            case 3:
                gunPrefab = fourPlayerPrefab;
                break;
            case 4:
                gunPrefab = fivePlayerPrefab;
                break;
            case 5:
                gunPrefab = sixPlayerPrefab;
                break;
            default:
                gunPrefab = sixPlayerPrefab;
                break;
        }

        rackSlots = new Transform[rackSize];
        for(int i= 0; i<rackSize; i++)
        {
            rackSlots[i] = new GameObject("slot" + i).transform;
            rackSlots[i].transform.parent = transform;
            rackSlots[i].localPosition = new Vector3(-i * spaceBetween, 0, 0);
        }

        guns = new GunModel[rackSize];
        for(int i=0; i < rackSize; i++)
        {
            if (i - WeaponWorkload.Instance.workLoad.Count < 0)
            {
                guns[i] = Instantiate(gunPrefab, rackSlots[i].position, rackSlots[i].rotation);
                guns[i].ActivePieces = WeaponWorkload.Instance.workLoad[i].Completion+1;
                guns[i].homeLocation = rackSlots[i].position;
                guns[i].HomeRotation = Quaternion.Euler(initialRotation);
                if (i > 0)
                {
                    guns[i].IsDragable = false;
                }
            }
            else
            {
                guns[i] = null;
            }
        }
    }

    void FillUp()
    {
        for(int i=0; i<rackSize-1; i++)
        {
            guns[i] = guns[i + 1];
            if (guns[i] != null)
            {
                guns[i].homeLocation = rackSlots[i].position;
                if (i == 0)
                {
                    guns[i].IsDragable = true;
                }
            }
        }
        guns[rackSize-1] = null;
        if (WeaponWorkload.Instance.workLoad.Count >= rackSize)
        {
            FillLast(WeaponWorkload.Instance.workLoad[rackSize]);
        }
    }

    void FillLast(Gun gun)
    {
        for(int i=0; i<rackSize; i++)
        {
            if(guns[i] == null)
            {
                guns[i] = Instantiate(gunPrefab, rackSlots[i].position, rackSlots[i].rotation);
                guns[i].ActivePieces = gun.Completion+1;
                guns[i].homeLocation = rackSlots[i].position;
                guns[i].HomeRotation = Quaternion.Euler(initialRotation);
                if (i > 0)
                {
                    guns[i].IsDragable = false;
                }
                return;
            }
        }
    }

    // Update is called once per frame
    void OnDestroy()
    {
        AssemblyPlayer.OnAssembleStartet -= FillUp;
        WeaponWorkload.OnGunReceived -= FillLast;
    }
}
