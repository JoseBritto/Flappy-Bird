using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private bool paused = false;

    [SerializeField] Sprite pause;
    [SerializeField] Sprite play;

    [SerializeField] Image image;
    public void PausePlayGame()
    {
        if(paused)
        {
            image.sprite = pause;
            paused = false;
            Time.timeScale = 1;
        }
        else
        {
            image.sprite = play;
            paused = true;
            Time.timeScale = 0;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PausePlayGame();
    }
}
