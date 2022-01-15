using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;


public class GameManager : MonoBehaviour
{    
    [SerializeField]
    private Rigidbody2D playerRb;

    [SerializeField]
    private GameObject pipePrefab;


    [SerializeField]
    private float speed;

    private GameObject pipesParent;

    private LinkedList<PipeBehaviour> activePipes;

    private Stack<PipeBehaviour> inactivePipes;

    [SerializeField]
    private float pipeDestroyX = -20.0f;

    [SerializeField]
    private float minPipeCount = 10;

    [SerializeField]
    private GameObject[] gameOverStuff;

    private void Start()
    {        
        playerRb.isKinematic = false;
        spawnPipesParent();

        activePipes ??= new LinkedList<PipeBehaviour>();
        inactivePipes ??= new Stack<PipeBehaviour>();


    }

    private void spawnPipesParent()
    {
        pipesParent = new GameObject("SPAWNED_PIPES");
        pipesParent.transform.position = new Vector3(0, 0, 0);
        pipesParent.SetActive(true);
    }

    private void handlePipeSpawn()
    {
        if (activePipes.Count >= minPipeCount)
            return;

        var point = activePipes.Count != 0 ? activePipes.First.Value.NextSpawnPos : Vector3.zero;


        var pipe = inactivePipes.Count == 0 ?
            Instantiate(pipePrefab, pipesParent.transform).GetComponent<PipeBehaviour>()
            : inactivePipes.Pop();

        pipe.gameObject.SetActive(true);

        pipe.transform.position = point;

        //float heightOffset = (Mathf.Clamp01(Mathf.PerlinNoise(Time.time * perlinScale, seed)) * 2) - 1;
        float heightOffset = UnityEngine.Random.Range(-1f, 1f);
        pipe.Pipe.transform.position = new Vector3(pipe.Pipe.transform.position.x, GetPipeHeight(heightOffset), pipe.Pipe.transform.position.z);

        activePipes.AddFirst(pipe);
    }

    private void FixedUpdate()
    {
        pipesParent.transform.position += speed * Vector3.left;

        handlePipeDestroy();

        handlePipeSpawn();
    }

    private void handlePipeDestroy()
    {
        if (activePipes.Count == 0)
            return;

        var pipe = activePipes.Last.Value;

        if (pipe.transform.position.x < pipeDestroyX)
        {
            pipe.gameObject.SetActive(false);
            activePipes.RemoveLast();
            inactivePipes.Push(pipe);
        }
    }

    public void StopGame()
    {
        Time.timeScale = 0;
        gameObject.SetActive(false);

        if (gameOverStuff != null)
        {
            foreach (var item in gameOverStuff)
            {
                item.SetActive(true);
            }
        }

        GameDataManager.Instance.GameEndCallback();
    }
}
