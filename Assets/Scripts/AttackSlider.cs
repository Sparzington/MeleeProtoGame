using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackSlider : MonoBehaviour
{
    private Slider _slider;
    private TextMeshProUGUI _text;

    public static AttackSlider instance;
    

    public float timerValue=0;
    public float max=10;

    private bool reset;
    private void Awake()
    {
        instance = this;
        _slider = GetComponentInChildren<Slider>();
        _text = GetComponentInChildren<TextMeshProUGUI>();

        _text.text = "0";
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
        _slider.value = 0;
        timerValue = 0.0f;
        max = value;
    }

    public void UpdateComboCount(int newCount)
    {
        _text.text = newCount.ToString();
    }

    public void UpdateCanComboColor(bool canCombo)
    {
        if (canCombo)
        {
            _text.color = Color.green;
        }
        else
        {
            _text.color = Color.red;
        }
    }
}
