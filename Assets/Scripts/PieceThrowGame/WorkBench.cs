using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkBench : MonoBehaviour
{
    GunModel currentModel;
    GunModel lastCollidedModel;
    [SerializeField]
    Transform gunPosition;
    [SerializeField]
    float speed;
    [SerializeField]
    float movingMargin;

    float time;

    void Start()
    {
        AssemblyPlayer.OnAssembleStartet += PlaceGunModel;
    }

    void Update()
    {
        time += Time.deltaTime;
        float posX = Mathf.Cos(time * speed) * movingMargin;
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        if (currentModel != null &&!currentModel.Done)
        {
            currentModel.homeLocation = gunPosition.position;
        }
    }

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.GetComponent<GunModel>() != null)
        {
            if (!collider.gameObject.GetComponent<GunModel>().Done)
            {
                lastCollidedModel = collider.gameObject.GetComponent<GunModel>();
                AssemblyPlayer.Instance.StartAssembly();
            }
        }
    }

    void PlaceGunModel()
    {
        Debug.Log("SetGun");        
        currentModel = lastCollidedModel;
        currentModel.onBench = true;
        currentModel.homeLocation = gunPosition.position;
        currentModel.HomeRotation = gunPosition.localRotation;
    }

    void OnDestroy()
    {
        AssemblyPlayer.OnAssembleStartet -= PlaceGunModel;
    }
}
