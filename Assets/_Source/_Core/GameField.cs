using System;
using UnityEngine;
using System.Collections.Generic;

public class GameField : MonoBehaviour
{
    [Header("Game Field sizes")] 
    public float SplacingTop;
    public float CellSize;
    public float Spacing;
    public int FieldSize;
    
    [Space(10)]
    [SerializeField] 
    private CellView CellPrefab;
    [SerializeField]
    private RectTransform rt;
    
    
    private CellView[,] Field;
    private Cell[,] Cells;

    private void Start()
    {
        GenerateField();
    }
    
    private void CreateField()
    {
        Field = new CellView[FieldSize, FieldSize];
        Cells = new Cell[FieldSize, FieldSize];
        
        float fieldWidth = FieldSize * (CellSize + Spacing) + Spacing;
        rt.anchoredPosition -= new Vector2(0, SplacingTop);
        rt.sizeDelta = new Vector2(fieldWidth, fieldWidth);   
        
        
        float startX = - (fieldWidth/2) + (CellSize/2) + Spacing;
        float startY = (fieldWidth/2) - (CellSize/2) - Spacing ;

        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                var cell = new Cell(startX + x * (CellSize + Spacing), startY - y * (CellSize + Spacing), 0);
                var cellView = Instantiate(CellPrefab, rt);
                
                cellView.Init(cell);
                
                Cells[x, y] = cell;
                Field[x, y] = cellView;
            }
        }
    }

    public void GenerateField()
    {
        if (Field == null)
        {
            CreateField();
        }

        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                Cells[x, y].UpdateValue(0);;
            }
        }
    }
    
}
