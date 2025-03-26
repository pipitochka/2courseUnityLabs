using System.Collections.Generic;
using Objects;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace Singletons
{
   public class Field : MonoBehaviour
   {
      public static Field Instance;
   
      public float CellSize;
      public float Splacing;
      public int FieldSize;
      public int StartCellsCount;
   
      [FormerlySerializedAs("_cellPrefab")] [SerializeField]
      public CellView cellViewPrefab;
      [SerializeField] 
      public RectTransform rt;
      [SerializeField]
      public SaveLoadManager saveLoadManager;

      private Cell[,] field;
      private CellView[,] cellViews;
   
      private bool anyCellMoved;

      public bool isTest = false;
   
      public void Awake()
      {
         if (Instance == null)
         {
            Instance = this;
         }
      }
   

      public void CreateField()
      {
         field = new Cell[FieldSize, FieldSize];
         cellViews = new CellView[FieldSize, FieldSize];
      
         float fieldWith = FieldSize * (CellSize + Splacing) + Splacing;
         rt.sizeDelta = new Vector2(fieldWith, fieldWith);
      
         float startX = -(fieldWith / 2) + (CellSize / 2) + Splacing;
         float startY = (fieldWith / 2) - (CellSize / 2) - Splacing;

         for (int x = 0; x < FieldSize; x++)
         {
            for (int y = 0; y < FieldSize; y++)
            {
               CellView cellView = Instantiate(cellViewPrefab, transform, false);
               var position = new Vector2(x * (CellSize + Splacing) + startX, - y * (CellSize + Splacing) + startY);
               cellView.transform.localPosition = position;

               Cell cell = new Cell(x, y, 0);
               cellView.Init(cell);
            
               field[x, y] = cell;
               cellViews[x, y] = cellView;
            
               cell.SetValue(x, y, 0);
            }
         }
      }

      public void GenerateField()
      {
         if (field == null)
         {
            CreateField();
         }
      
         var t = ReadFromFile();
         if (t == true)
         {
            return;
         }

         for (int x = 0; x < FieldSize; x++)
         {
            for (int y = 0; y < FieldSize; y++)
            {
               field[x, y].SetValue(x, y, 0);
            }
         }

         for (int x = 0; x < StartCellsCount; x++)
         {
            CreateCell();
         }
      }
   
      public void GenerateNewField()
      {
         if (field == null)
         {
            CreateField();
         }
      
         for (int x = 0; x < FieldSize; x++)
         {
            for (int y = 0; y < FieldSize; y++)
            {
               field[x, y].SetValue(x, y, 0);
            }
         }

         for (int x = 0; x < StartCellsCount; x++)
         {
            CreateCell();
         }
      }

      public Vector2Int GetEmptyPosition()
      {
         var emptyList = new List<Cell>();

         for (int x = 0; x < FieldSize; x++)
         {
            for (int y = 0; y < FieldSize; y++)
            {
               if (field[x, y].IsEmpty)
               {
                  emptyList.Add(field[x, y]);
               }
            }
         }

         if (emptyList.Count == 0)
         {
            throw new System.Exception("Field is empty");
         }
      
         var cell = emptyList[Random.Range(0, emptyList.Count)];
      
         return new Vector2Int(cell.X, cell.Y);
      }
   
      private void CreateCell()
      {
         var position = GetEmptyPosition();
         int value = UnityEngine.Random.value < 0.9f ? 1 : 2;
         var cell = field[position.x, position.y];
         cell.SetValue(cell.X, cell.Y, value);

      }

      public void OnInput(Vector2 direction)
      {
         if (!GameControlller.GameStarted)
         {
            return;
         }
         anyCellMoved = false;
         ResetCellFlags();
      
         Move(direction);

         if (anyCellMoved)
         {
            CreateCell();
            CheckGameResult();
         }

         if (!isTest)
         {
            saveLoadManager.SaveArrayToFile(field, GameControlller.Instance.GetPoints());
         }
      }

      public void Move(Vector2 direction)
      {
         int startXY = direction.x > 0 || direction.y < 0 ? FieldSize - 1 : 0;
         int dir = direction.x != 0 ? (int)direction.x : -(int)direction.y;

         for (int i = 0; i < FieldSize; i++)
         {
            for (int k = startXY; k < FieldSize && k >= 0; k -= dir)
            {
               var cell = direction.x != 0 ? (field[k, i]) : field[i, k];

               if (cell.IsEmpty)
               {
                  continue;
               }

               var cellToMerge = FindCellToMerge(cell, direction);
               if (cellToMerge != null)
               {
                  cell.MergeWithCell(cellToMerge);
                  anyCellMoved = true;
               
                  continue;
               }
            
               var emptyCell = FindEmptyCell(cell, direction);
               if (emptyCell != null)
               {
                  cell.MoveToCell(emptyCell);
                  anyCellMoved = true;
               
                  continue;
               }

            }
         }
      
      
      }

      private Cell FindCellToMerge(Cell cell, Vector2 direction)
      {
         int startX = cell.X + (int)direction.x;
         int startY = cell.Y - (int)direction.y;

         for (int x = startX, y = startY;
              x < FieldSize && x >= 0 && y >= 0 && y < FieldSize;
              x += (int)direction.x, y -= (int)direction.y)
         {
            if (field[x, y].IsEmpty)
            {
               continue;
            }

            if (field[x, y].Value == cell.Value && field[x, y].HasMerged == false)
            {
               return field[x, y];
            }
         
            break;
         }
      
         return null;
      }


      private Cell FindEmptyCell(Cell cell, Vector2 direction)
      {
         Cell emptyCell = null;
      
         int startX = cell.X + (int)direction.x;
         int startY = cell.Y - (int)direction.y;

         for (int x = startX, y = startY;
              x < FieldSize && x >= 0 && y >= 0 && y < FieldSize;
              x += (int)direction.x, y -= (int)direction.y)
         {
            if (field[x, y].IsEmpty)
            {
               emptyCell = field[x, y];
            }
            else
            {
               break;
            }
         
         }
      
         return emptyCell;
      }

      private void CheckGameResult()
      {
         bool lose = true;

         for (int x = 0; x < FieldSize; x++)
         {
            for (int y = 0; y < FieldSize; y++)
            {
               if (field[x, y].Value == Cell.MaxValue)
               {
                  GameControlller.Instance.Win();
                  return;
               }
            }
         }

         for (int x = 0; x < FieldSize; x++)
         {
            for (int y = 0; y < FieldSize; y++)
            {
               if (lose && 
                   field[x, y].IsEmpty || 
                   FindCellToMerge(field[x, y], Vector2.left) != null || 
                   FindCellToMerge(field[x, y], Vector2.right) != null || 
                   FindCellToMerge(field[x, y], Vector2.up) != null|| 
                   FindCellToMerge(field[x, y], Vector2.down) != null)
               {
                  lose = false;
                  return;
               }
            }

         
         }
      
         if (lose)
         {
            GameControlller.Instance.Lose();
         }
      
      }

//    public void Update()
//    {
// #if UNITY_EDITOR
//       if (Input.GetKeyDown(KeyCode.A))
//       {
//          OnInput(Vector2.left);
//       }
//
//       if (Input.GetKeyDown(KeyCode.D))
//       {
//          OnInput(Vector2.right);
//       }
//
//       if (Input.GetKeyDown(KeyCode.W))
//       {
//          OnInput(Vector2.up);
//       }
//
//       if (Input.GetKeyDown(KeyCode.S))
//       {
//          OnInput(Vector2.down);
//       }
//
//       if (Input.GetKeyDown(KeyCode.LeftArrow))
//       {
//          OnInput(Vector2.left);
//       }
//
//       if (Input.GetKeyDown(KeyCode.RightArrow))
//       {
//          OnInput(Vector2.right);
//       }
//
//       if (Input.GetKeyDown(KeyCode.UpArrow))
//       {
//          OnInput(Vector2.up);
//       }
//
//       if (Input.GetKeyDown(KeyCode.DownArrow))
//       {
//          OnInput(Vector2.down);
//       }
// #endif      
//    }

      private void ResetCellFlags()
      {
         for (int x = 0; x < FieldSize; x++)
         {
            for (int y = 0; y < FieldSize; y++)
            {
               field[x, y].ResetFlags();
            }
         }
      }

      private bool ReadFromFile()
      {
         if (isTest)
         {
            return false;
         }
      
         var t = saveLoadManager.LoadFieldData();

         if (t != null)
         {
            for (int x = 0; x < FieldSize; x++)
            {
               for (int y = 0; y < FieldSize; y++)
               {
                  field[x, y].SetValue(field[x, y].X, field[x, y].Y, t[x * FieldSize + y]);
               }
            }
            GameControlller.Instance.ChangePoints(t[FieldSize*FieldSize]);
            return true;
         }
         return false;
      }

      public int[,] getField()
      {
         int[,] data = new int[FieldSize, FieldSize];
         for (int x = 0; x < FieldSize; x++)
         {
            for (int y = 0; y < FieldSize; y++)
            {
               data[y, x] = field[x, y].Points;
            }
         }
         return data;
      }

      public void setField(int[,] data)
      {
         for (int x = 0; x < FieldSize; x++)
         {
            for (int y = 0; y < FieldSize; y++)
            {
               int value = data[y, x] > 0 ? (int)Mathf.Log(data[y, x], 2) : 0;
               field[x, y].Value = value;
            }
         }
      }

      public void setRt(RectTransform rt)
      {
         this.rt = rt;
      }

      public void setCellView(CellView cellView)
      {
         cellViewPrefab = cellView;
      }

      public void setSaveLoadManager(SaveLoadManager saveLoadManager)
      {
         this.saveLoadManager = saveLoadManager;
      }
   }
}
