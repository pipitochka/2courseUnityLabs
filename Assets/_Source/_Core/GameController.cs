using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    
    public static int Points { get; set; }
    public static bool GameStarted { get; set; }

    [SerializeField] 
    private TextMeshProUGUI gameResult;
    [SerializeField]
    private TextMeshProUGUI pointsText;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        gameResult.text = "";
        SetPoints(0);
        GameField.Instance.GenerateField();
        
        GameStarted = true;
    }

    public void AddPoints(int points)
    {
        SetPoints(Points + points);
    }

    public void SetPoints(int points)
    {
        Points = points;
        pointsText.text = Points.ToString();
    }

    private void Win()
    {
        GameStarted = false;
        gameResult.text = "WIN";
    }

    private void Lose()
    {
        GameStarted = false;
        gameResult.text = "LOSE";
    }
}
