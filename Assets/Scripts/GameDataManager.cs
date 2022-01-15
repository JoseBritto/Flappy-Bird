using UnityEngine;
using static Constants;

public class GameDataManager : MonoBehaviour
{
    private int score;

    private int best;

    public static GameDataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        best = PlayerPrefs.GetInt(BEST_SCORE, 0);
    }

    public void IncrementScore() => score++;

    public int GetScore() => score;

    public int GetBestScore() => best;

    public void GameEndCallback()
    {
        saveBest();
    }

    private void saveBest()
    {
        if (score > best)
        {
            PlayerPrefs.SetInt(BEST_SCORE, score);
            best = score;            
            PlayerPrefs.Save();
        }
        score = 0;
    }


    private void OnApplicationQuit()
    {
        saveBest();
    }
}
