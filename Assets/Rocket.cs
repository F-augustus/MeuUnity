﻿using UnityEngine;
using UnityEngine.SceneManagement;
// todo: fix lighting bug
public class Rocket : MonoBehaviour {
	
    [SerializeField] float rcsT = 100f;
    [SerializeField] float mainT = 100f;
    [SerializeField] AudioClip mainEngine;
	Rigidbody rigidBody;
	AudioSource audioSource;
    enum State {Alive, Dead, Transcend};
    State state = State.Alive;
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent <AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        //todo somewhere: stop rocket sound when dead
        if (state==State.Alive)  
		{RespondToThrustInput();
		RespondToRotateInput();}
	}
    void OnCollisionEnter(Collision collision)
    {
        if (state !=State.Alive) {return;} // ignore collisions when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;

            case "Finish":
                state = State.Transcend;
                Invoke("LoadNextLevel", 1f); //parameterise this time
                break;
            default:
                print("Dead");
                state= State.Dead;
                Invoke ("LoadFirstLevel", 1f);
                break;
        }

    }
    private void LoadFirstLevel()
    {SceneManager.LoadScene(0);
    } 
    private void LoadNextLevel()
    {SceneManager.LoadScene(1);// todo: allow for more than 2 levels
    } 
private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        { audioSource.Stop(); }
	}

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainT);
        if (!audioSource.isPlaying) //so it doesn't layer
        { audioSource.PlayOneShot(mainEngine); }
    }

    private void RespondToRotateInput()   //can T while rotating
  
    {
        rigidBody.freezeRotation = true;//take manual control of the rotation
        float rotationThisFrame = rcsT * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) {transform.Rotate(Vector3.forward * rotationThisFrame);}

        else if (Input.GetKey(KeyCode.D)) { transform.Rotate(-Vector3.forward* rotationThisFrame);}
        
       rigidBody.freezeRotation = false; // resume physics control of rotation  
    }
    
}
