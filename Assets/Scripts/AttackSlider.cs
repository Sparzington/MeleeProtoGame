using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackSlider : MonoBehaviour
{
    private Slider _slider;

    public static AttackSlider instance;
    

    public float timerValue=0;
    public float max=10;

    private bool reset;
    private void Awake()
    {
        instance = this;
        _slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSliderValue(ref timerValue);
    }
    private void UpdateSliderValue(ref float cooldown)
    {
        if (cooldown < max)
        {
            reset = false;
            cooldown += Time.deltaTime;

            _slider.value = Mathf.Lerp(0, 1, cooldown/max);
        }

        if (_slider.value == 1 && !reset)
        {
            cooldown = 0.0f;
            max = 0.0f;
            _slider.value = 0;
            reset = true;
        }
    }

    public void SetTimer(float value)
    {
        max = value;
    }
}
