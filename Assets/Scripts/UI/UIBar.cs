using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Souls
{
    public abstract class UIBar : MonoBehaviour
    {
        public Slider slider;

        void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public void SetMaxValue(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }

        public void SetCurrentValue(int value)
        {
            slider.value = value;
        }

        public abstract ResourceType GetResource();// { }
    }



}
