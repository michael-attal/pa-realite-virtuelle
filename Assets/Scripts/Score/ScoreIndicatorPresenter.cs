using UnityEngine;
using UnityEngine.UI;

public class ScoreIndicatorPresenter : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    
    private ScoreManager scoreManager;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = ScoreManager.Instance;
        
        ChangeScore();
        scoreManager.OnScoreUpdate.AddListener(ChangeScore);
    }

    private void ChangeScore()
    {
        scoreText.text = scoreManager.Score.ToString();
    }
}
