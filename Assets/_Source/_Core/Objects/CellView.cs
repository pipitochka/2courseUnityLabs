using System;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class CellView : MonoBehaviour
{
    private Cell cell;
    
    [SerializeField]
    private Image image;
    [SerializeField]
    TextMeshProUGUI points;

    
    public void Init(Cell cell)
    {
        if (this.cell != null)
        {
            this.cell.OnValueChanged -= UpdateValue;
            this.cell.OnPositionChanged -= UpdatePosition;
        }
        
        this.cell = cell;
        this.cell.OnValueChanged += UpdateValue;
        this.cell.OnPositionChanged += UpdatePosition;
        
        UpdateValue(cell.Value);
        UpdatePosition(cell.X, cell.Y);
        
    }
    
    private void UpdateValue(int newValue)
    {
        if (cell != null && image != null && points != null)
        {
            points.text = cell.IsEmpty ? String.Empty : cell.Points.ToString();
            image.color = ColorManager.Instance.CellColors[cell.Value];
        }
    }

    private void UpdatePosition(float x, float y)
    {
    }
}
