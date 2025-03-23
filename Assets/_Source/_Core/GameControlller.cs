using System;
using TMPro;
using UnityEngine;

public class GameControlller : MonoBehaviour
{
    public static GameControlller Instance;
    
    public static int Points {get; private set;}
    public static bool GameStarted {get; private set;}
    
    [SerializeField] 
    private TextMeshProUGUI gameResult;
    [SerializeField] 
    private TextMeshProUGUI pointsText;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StartGame()
    {
        gameResult.text = "";
        
        SetPoints(0);
        GameStarted = true;
        
        Field.Instance.GenerateField();
    }

    public void AddPoints(int points)
    {
        SetPoints(Points + points);
    }

    private void SetPoints(int points)
    {
        Points = points;
        pointsText.text = Points.ToString();
    }

    private void Start()
    {
        StartGame();
    }

    public void Win()
    {
        GameStarted = false;
        gameResult.text = "WIN";
    }

    public void Lose()
    {
        GameStarted = false;
        gameResult.text = "LOSE";
    }
}
