using UnityEngine;

public class ColorManager: MonoBehaviour
{
    public static ColorManager Instance;

    public Color[] colors;
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
