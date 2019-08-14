using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishArea : MonoBehaviour
{
    [SerializeField]
    Transform startPos;
    [SerializeField]
    Transform targetPos;
    [SerializeField]
    float transitionTime;
    [SerializeField]
    Vector3 rotation;

    Plane basePlane = new Plane(Vector3.back, new Vector3(0, 0, 0));

    void Start()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width*0.85f, Screen.height / 2));
        float distance;
        if (basePlane.Raycast(ray, out distance))
        {
            transform.position = ray.GetPoint(distance);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        GunModel lastCollidedModel = collider.GetComponent<GunModel>();
        if (lastCollidedModel != null)
        {
            if (lastCollidedModel.onBench && !lastCollidedModel.Done)
            {
                lastCollidedModel.HomeRotation = Quaternion.Euler(rotation);
                lastCollidedModel.Done = true;
                lastCollidedModel.homeLocation = startPos.position;
                StartCoroutine(MoveGun(lastCollidedModel));
            }
        }
    }

    IEnumerator MoveGun(GunModel model)
    {
        yield return new WaitForSeconds(1f);
        AssemblyPlayer.Instance.FinishAssembly();
        float time = 0;
        while (time < transitionTime)
        {
            model.homeLocation = Vector3.Lerp(startPos.position, targetPos.position, time / transitionTime);
            time += Time.deltaTime;
            yield return null;
        }
        model.Destroy();
    }
}
