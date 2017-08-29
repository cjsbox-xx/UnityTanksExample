﻿using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int playerNumber = 1;         
    public float speed = 12.0f;            
    public float turnSpeed = 180.0f;       
    public AudioSource movementAudio;    
    public AudioClip engineIdling;       
    public AudioClip engineDriving;      
    public float pitchRange = 0.2f;

    private string movementAxisName;     
    private string turnAxisName;         
    private Rigidbody rigidBody;         
    private float movementInputValue;    
    private float turnInputValue;        
    private float originalPitch;         


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        rigidBody.isKinematic = false;
        movementInputValue = 0.0f;
        turnInputValue = 0.0f;
    }


    private void OnDisable ()
    {
        rigidBody.isKinematic = true;
    }


    private void Start()
    {
        movementAxisName = "Vertical" + playerNumber;
        turnAxisName = "Horizontal" + playerNumber;

        originalPitch = movementAudio.pitch;
    }

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        movementInputValue = Input.GetAxis(movementAxisName);
        turnInputValue = Input.GetAxis(turnAxisName);

        EngineAudio();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        if (Mathf.Abs(movementInputValue) < 0.1f && Mathf.Abs(turnInputValue) < 0.1f)
        {
            if (movementAudio.clip == engineDriving)
            {
                movementAudio.clip = engineIdling;
                movementAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                movementAudio.Play();
            }
        }
        else
        {
            if (movementAudio.clip == engineIdling)
            {
                movementAudio.clip = engineDriving;
                movementAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                movementAudio.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
    }


    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        Vector3 movement = transform.forward * movementInputValue * speed * Time.deltaTime;
        rigidBody.MovePosition(rigidBody.position + movement);
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = turnInputValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0.0f, turn, 0.0f);
        rigidBody.MoveRotation(rigidBody.rotation * turnRotation);
    }
}