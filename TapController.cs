using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]

public class TapController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float tapForce = 200;
    public float tiltSmooth = 2;
    public Vector3 startPos;
    

    Rigidbody2D rigidBody;
    Quaternion downRot;
    Quaternion forRot;

    GameManager game;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        downRot = Quaternion.Euler(0, 0, -85);
        forRot = Quaternion.Euler(0, 0, 35);
        game = GameManager.Instance;
        rigidBody.simulated = false;
    }

    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }
    void Update()
    {
        if (game.GameOver) return;

        
        
        if (Input.GetMouseButtonDown(0))
        {
            transform.rotation = forRot;
            rigidBody.velocity = Vector3.zero;
            rigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }

        
        transform.rotation = Quaternion.Lerp(transform.rotation, downRot, tiltSmooth * Time.deltaTime);
        
    }
 
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "ScoreZone")
        {
            OnPlayerScored();
        }

        if (col.gameObject.tag == "DeadZone")
        {
            rigidBody.simulated = false;
            OnPlayerDied();
        }
    }
}
