using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Serialization;

public class CellView : MonoBehaviour
{
    private Cell cell;
 
    [SerializeField]
    private TextMeshProUGUI points;
    
    [SerializeField] 
    private Image image;
    
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
        if (points != null)
        {
            points.text = newValue.ToString();
            points.color = ColorManager.Instance.colors[newValue];
        }
    }

    private void UpdatePosition(float x, float y)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(x, y);
        
    }
}
