
using UnityEngine;
using System;
public class BindableProperty<T>
{
    private T mValue = default;

    public T Value
    {
        get => mValue;
        set
        {
            if (mValue != null && mValue.Equals(value)) return;
            mValue = value;
            OnValueChanged.Invoke(mValue);
        }
    }

    public event Action<T> OnValueChanged = _ => { };
}