using System;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class Cell
{
    public int X { get; private set; }
    public int Y {get; private set;}
    
    public int Value {get; set;}
    
    public int Points => Value == 0 ? 0 : (int)Mathf.Pow(2, Value);
    
    public bool IsEmpty => Value == 0;

    public const int MaxValue = 11;
    
    public event Action<int> OnValueChanged;
    public event Action<float, float> OnPositionChanged;



    public bool HasMerged { get; private set; }
    
    [SerializeField]
    private Image image;
    [SerializeField]
    TextMeshProUGUI points;

    public void SetValue(int x, int y, int value)
    {
        X = x;
        Y = y;
        Value = value;
        
        OnValueChanged?.Invoke(Value);
        
    }
    

    public void IncreaseValue()
    {
        Value++;
        HasMerged = true;

        if (GameControlller.Instance != null)
        {
            GameControlller.Instance.AddPoints(Points);
        }

        OnValueChanged?.Invoke(Value);
    }

    public void ResetFlags()
    {
        HasMerged = false;
    }

    public void MergeWithCell(Cell cell)
    {
        cell.IncreaseValue();
        SetValue(X, Y, 0);
        
        OnValueChanged?.Invoke(Value);
    }

    public void MoveToCell(Cell cell)
    {
        cell.SetValue(cell.X, cell.Y, Value);
        SetValue(X, Y, 0);
    }

    public Cell(int x, int y, int value)
    {
        X = x;
        Y = y;
        Value = value;
    }
}