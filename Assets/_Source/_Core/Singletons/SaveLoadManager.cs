using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Objects;
using UnityEngine;

namespace Singletons
{
    public class SaveLoadManager : MonoBehaviour
    {
        public static SaveLoadManager Instance;
    
    
        private string saveFilePath;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            saveFilePath = Path.Combine(Application.persistentDataPath, "fieldData.dat");
        }

        public void SaveArrayToFile(Cell[,] array, int Points)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            int n = array.GetLength(0);
            int[] arr = new int[n * n + 1];

            int index = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    arr[index] = array[i, j].Value;
                    index++;
                }
            }
            arr[n * n] = Points;

        
            using (FileStream file = File.Create(saveFilePath))
            {
                formatter.Serialize(file, arr);
            }

            Debug.Log("Array saved to " + saveFilePath);
        }

        public int[] LoadFieldData()
        {
            if (File.Exists(saveFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                using (FileStream file = File.Open(saveFilePath, FileMode.Open))
                {
                    int[] fieldData = (int[])formatter.Deserialize(file);
                    Debug.Log("Game data loaded from " + saveFilePath);
                    return fieldData;
                }
            }
            else
            {
                return null;
            }
        }
    }
}