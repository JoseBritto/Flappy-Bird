using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestart : MonoBehaviour
{
    public void Restart()
    {        
        SceneManager.LoadScene(gameObject.scene.buildIndex);
        Time.timeScale = 1;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Restart();
    }
}
