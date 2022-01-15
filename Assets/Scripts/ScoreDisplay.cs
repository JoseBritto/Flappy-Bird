using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    private int displayedScore = -1;

    [SerializeField]
    private Sprite[] scoreSprites;

    private GameDataManager datamanager;

    [SerializeField]
    private Image ones;
    [SerializeField]
    private Image tens;

    private void Start()
    {
        datamanager = GameDataManager.Instance;
        tens.enabled = false;

        ones.sprite = scoreSprites[0];
        displayedScore = -1;
        print("Start");
    }

    private void FixedUpdate()
    {
        if (displayedScore == datamanager.GetScore())
            return;

        displayedScore = datamanager.GetScore();

        string s = displayedScore.ToString();

        if (s.Length == 1)
        {
            ones.sprite = scoreSprites[displayedScore];
            tens.enabled = false;
        }
        else if (s.Length == 2)
        {
            tens.enabled = true;
            ones.sprite = scoreSprites[int.Parse(s[1].ToString())];
        }
    }
}
