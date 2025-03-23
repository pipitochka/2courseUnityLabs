using System;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class Cell : MonoBehaviour
{
    public int X { get; private set; }
    public int Y {get; private set;}
    
    public int Value {get; private set;}
    
    public int Points => Value == 0 ? 0 : (int)Mathf.Pow(2, Value);
    
    public bool IsEmpty => Value == 0;

    public const int MaxValue = 11;

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
        
        UpdateCell();
    }

    public void UpdateCell()
    {
        points.text = IsEmpty ? String.Empty : Points.ToString();
        image.color = ColorManager.Instance.CellColors[Value];
    }

    public void IncreaseValue()
    {
        Value++;
        HasMerged = true;
        
        GameControlller.Instance.AddPoints(Points);
        
        UpdateCell();
    }

    public void ResetFlags()
    {
        HasMerged = false;
    }

    public void MergeWithCell(Cell cell)
    {
        cell.IncreaseValue();
        SetValue(X, Y, 0);
        
        UpdateCell();
    }

    public void MoveToCell(Cell cell)
    {
        cell.SetValue(cell.X, cell.Y, Value);
        SetValue(X, Y, 0);
    }
}
