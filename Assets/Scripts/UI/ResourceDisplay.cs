using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField]
    Slider instantSlider = null;
    [SerializeField]
    Slider delaySlider = null;
    [SerializeField]
    float smoothTime = 0.5f;
    [SerializeField]
    float maxSmoothSpeed = 10;
    float currentVelocity;
    protected float _currentValue = 0;
    public float CurrentValue
    {
        get
        {
            return _currentValue;
        }
        set
        {
            instantSlider.value = value / MaxValue;
            _currentValue = value;
        }
    }
    public float MaxValue
    {
        get
        {
            return ResourceManager.Instance.resourceCap;
        }
    }

    void Start()
    {
        ResourceManager.OnResourceChange += OnValueChanged;
    }

    void Update()
    {
        if (delaySlider.value != instantSlider.value)
        {
            float deltaValue = delaySlider.value - instantSlider.value;
            delaySlider.value = Mathf.SmoothDamp(delaySlider.value, instantSlider.value, ref currentVelocity, smoothTime, maxSmoothSpeed);
        }
    }

    protected virtual void OnValueChanged(ResourceType rt, float amount)
    {

    }

    void OnDestroy()
    {
        StopAllCoroutines();
        ResourceManager.OnResourceChange -= OnValueChanged;
    }
}
