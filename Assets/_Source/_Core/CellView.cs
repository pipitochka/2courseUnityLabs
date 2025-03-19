using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Serialization;

public class CellView : MonoBehaviour
{
    private Cell cell;
    
    [SerializeField] 
    private Image image;
 
    [SerializeField]
    private TextMeshProUGUI points;
    
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
            
            if (newValue != 0)
            {
                points.text = newValue.ToString();
                int logValue = Mathf.FloorToInt(Mathf.Log(newValue, 2));
                image.color = MyColorManager.Instance.colors[logValue + 1];

            }
            else
            {
                points.text = "";
                image.color = MyColorManager.Instance.colors[0];

            }
        }
    }

    private void UpdatePosition(float x, float y)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(x, y);
        
    }
}
