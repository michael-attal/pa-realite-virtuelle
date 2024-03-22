using UnityEngine;
using UnityEngine.Events;

public class ScoreManager
{
    // ----- SINGLETON PATTERN -----
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
                instance = new ScoreManager();

            return instance;
        }
    }

    private static ScoreManager instance;
    // -----------------------------

    public UnityEvent OnScoreUpdate = new UnityEvent();

    public int Score
    {
        get => score;
        set
        {
            score = value;
            Debug.Log(value);
            OnScoreUpdate.Invoke();
        }
    }
    
    private int score = 0;
}
