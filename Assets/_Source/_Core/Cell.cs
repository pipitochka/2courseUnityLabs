using UnityEngine;
using System;

public class Cell
{
    public float X { get; private set; }
    public float Y { get; private set; }
    public int Value {get; private set;}
    
    public event Action<int> OnValueChanged;
    public event Action<float, float> OnPositionChanged;

    public Cell(float x, float y, int value)
    {
        X = x;
        Y = y;
        Value = value;
    }

    public void UpdateValue(int value)
    {
        Value = value;
        OnValueChanged?.Invoke(Value);
    }

    public void UpdatePosition(float x, float y)
    {
        if (X != x || Y != y)
        {
            X = x;
            Y = y;
            OnPositionChanged?.Invoke(X, Y);
        }
    }
    
}
