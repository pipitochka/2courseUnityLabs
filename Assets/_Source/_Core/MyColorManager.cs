using UnityEngine;

public class MyColorManager: MonoBehaviour
{
    public static MyColorManager Instance;

    public Color[] colors;
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
