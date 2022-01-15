using System;
using System.Collections;
using UnityEngine;
using static Constants;

public class PlayerBehaviour : MonoBehaviour
{

    [SerializeField]
    private Rigidbody2D playerRigidbody;

    [SerializeField]
    private float upForce;

    [SerializeField]
    private float switchToFallVelocity = -8.2f;

    private Coroutine fallingAnimation;

    private Coroutine risingAnimation;

    [SerializeField]
    private float fallingAngle = -90;


    [SerializeField]
    private float risingAngle = 45;

    [SerializeField]
    private float risingAnimationSpeed = 1;

    [SerializeField]
    private float fallingAnimationSpeed = 1;


    [SerializeField]
    private KeyCode[] triggerKeys = new KeyCode[]
    {
        KeyCode.Space,
        KeyCode.KeypadEnter,
        KeyCode.Return
    };

    [SerializeField]
    private bool triggerOnMouseClick = true;

    [SerializeField]
    private bool triggerOnTap = true;

    private GameManager manager;

    private bool started = false;
    bool ended = false;

    private GameObject lastPoint;
    
    private void Start()
    {
        manager = FindObjectOfType<GameManager>(true);
        started = manager.isActiveAndEnabled;
    }

    private void Update()
    {
        if (ended)
            return;
        handleInput();
    }

    private void FixedUpdate()
    {
        if (ended)
            return;
        
        handleFallAnimation();
    }

    private void handleFallAnimation()
    {
        if (playerRigidbody.velocity.y < switchToFallVelocity)
        {
            if (risingAnimation != null)
            {
                StopCoroutine(risingAnimation);
                risingAnimation = null;
            }

            if (fallingAnimation == null)
                fallingAnimation = StartCoroutine(fallingAnimationCoroutine());

            return;
        }

    }

    private IEnumerator fallingAnimationCoroutine()
    {
        var target = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, fallingAngle);

        while (transform.rotation.eulerAngles.z != fallingAngle)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, target, fallingAnimationSpeed);
            yield return new WaitForFixedUpdate();
        }

        fallingAnimation = null;
    }

    private void handleInput()
    {

        if (triggerOnMouseClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                movePlayerUp();
                return;
            }
        }

        if (triggerOnTap)
        {
            var touches = Input.touches;

            if (touches.Length > 0)
            {
                for (int i = 0; i < touches.Length; i++)
                {
                    if (touches[i].phase == TouchPhase.Began)
                    {
                        movePlayerUp();
                        return;
                    }
                }
            }
        }

        if (triggerKeys.Length > 0)
        {
            for (int i = 0; i < triggerKeys.Length; i++)
            {
                if (Input.GetKeyDown(triggerKeys[i]))
                {
                    movePlayerUp();
                    return;
                }
            }
        }
    }

    private void movePlayerUp()
    {
        if(!started)
        {
            startGame();
        }

        AudioManager.Instance.PlayFlap();

        playerRigidbody.velocity = upForce * Vector2.up;

        if (risingAnimation != null)
            return;

        if (fallingAnimation != null)
            StopCoroutine(fallingAnimation);

        fallingAnimation = null;

        risingAnimation = StartCoroutine(risingAnimationCoroutine());
    }

    private void startGame()
    {
        manager.gameObject.SetActive(true);
    }

    private IEnumerator risingAnimationCoroutine()
    {
        var target = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, risingAngle);

        while (transform.rotation.eulerAngles.z != risingAngle)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, target, risingAnimationSpeed);
            yield return new WaitForFixedUpdate();
        }

        risingAnimation = null;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(POINT_TAG))
        {
            if(lastPoint != collision.gameObject)
            {
                lastPoint = collision.gameObject;

                GameDataManager.Instance.IncrementScore();

                AudioManager.Instance.PlayPoint();
            }    
        }

        if (!collision.CompareTag(OBSTACLE_TAG))
            return;
        AudioManager.Instance.PlayDie();

        playerRigidbody.isKinematic = true;
        playerRigidbody.velocity = Vector3.zero;
        Debug.Log("DEAD!!");
        manager.StopGame();
        StopAllCoroutines();
        enabled = false;
        var components = gameObject.GetComponentsInChildren<Behaviour>();

        foreach (var component in components)
        {
            if (component.GetType() == typeof(SpriteRenderer))
                continue;

            component.enabled = false;
        }
    }
}
