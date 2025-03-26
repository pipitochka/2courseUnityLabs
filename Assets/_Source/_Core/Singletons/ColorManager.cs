using UnityEngine;

namespace Singletons
{
    public class ColorManager : MonoBehaviour
    {
        public static ColorManager Instance;

        public Color[] CellColors; 
    
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}
