using UnityEngine;

public class PipeBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform nextSpawn;

    [SerializeField]
    private Transform pipe;

    public Vector3 NextSpawnPos => nextSpawn.position;
    public Transform Pipe => pipe;

    
}
