using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public UnityEngine.Vector2 limits;// sets the limits for bar movement 
    public Text scoreDisplay; // holds the text varable for score display
    AudioSource audioSource; // holds the audio Source
    public AudioClip coinPickUp, hit; // holds the audio Clips
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); //finds and stores the audio source
    }

    public Text finalScore; // holds the UI element for the final score display 
    public GameObject[] reveal; // game over screen stored here

    void Update()
    {
        BarMovement(); // calls on the bar movement script 

        if(!pause && Input.GetKeyDown(KeyCode.Space)) { // if the game is not paused and the player presses space 
            StartCoroutine(PlayerInput()); // start coroutine player input

        }
        if(lives <= 0) { // if lives are 0 or less 
            pause = true; // pause the game 
            
            if(Input.GetKeyDown(KeyCode.R)) { // if player presses r
                SceneManager.LoadScene(0); // load scene at index 0.
            }
            
            
        }


    }

    public GameObject bar, scoreBar; // stores the bar and target bar game objects 
    public float speed= 5f;  // sets the bars speed
    void BarMovement() {
        // if the bar hit one of the limits invert the speed. 
        if(bar.transform.position.y > limits.x || bar.transform.position.y < limits.y) {
            speed *= -1; 
        }

        if(!pause) {// if the game is not paused 
            bar.transform.Translate(UnityEngine.Vector2.up * speed * Time.deltaTime); // move the bar up using speed as a refrence 
        }

    }

    bool pause = false; // pause status of the game 
    IEnumerator PlayerInput() {

        pause = true; // pause the game 
        ScoreCalualte(); // work out the score
        yield return new WaitForSeconds(0.5f); // wait .5 of a second 
        MoveScoreBar();// calls move socre bar 
        speed *= -1; // invert speed. 
        pause = false; // unpause the game 


    }

    int score = 0; // holds the score 
    int lives = 6; // holds the number of lives 
    void ScoreCalualte() { // checks the score 
        // check the distance to the bar and store as a positive value. 
        float distance = Mathf.Abs(bar.transform.position.y - scoreBar.transform.position.y);
        int addToScore =0; 
        
        if(distance > 0.51f) { // if outlside outer limitations 
            LostAlive(); // lose a life 
            audioSource.PlayOneShot(hit, 1f); // pay negative audio
        }
        else if(distance < 0.5f) { // just inside bounds 
            addToScore = 25; // add 25 
            audioSource.PlayOneShot(coinPickUp, 1f); // play good audio Clip
            DifficultyScale();// call difficulty Scale. 
        }
        else if(distance < 0.3f) {
            addToScore = 50; // add 50 
            audioSource.PlayOneShot(coinPickUp, 1f); // play good audio Clip
            DifficultyScale();// call difficulty Scale. 
        }
        else if(distance < 0.1f) {
            addToScore = 100;// add100
            audioSource.PlayOneShot(coinPickUp, 1f); // play good audio Clip
            DifficultyScale();// call difficulty Scale. 
        }
        score += addToScore; // apply the score to add 
        scoreDisplay.text = score.ToString(); // update the displayed score. 
    }

    void DifficultyScale() {

        if(speed < 10f && speed > 0) { // if speed is less then 10 and greater then 0 
            speed += 0.5f;// add 0.5f to speed.
        }
        else if(speed > -10f && speed < 0) { // if speed is less then 0 but greater then -10 
            speed -= 0.5f; // remove 0.5f from the speed. 
        }
    }

    public  Image[] heart = new Image[6];// store heart images. 
    void LostAlive() {
        lives --;// remove one from the live value
        for(int i =0; i < heart.Length; i++) { // loop where n is less then heart lenght, iterating at +1

            if(i < lives) { // if i is less then lives 
                heart[i].enabled = true;// heart UI at point i is visable 
            }else { // if i is not less then live values
                heart[i].enabled = false;// heart UI at point i is not visable 
            }

        }

        if(lives <= 0) { // if i have no lives lft

            foreach(GameObject g in reveal) { // for evey object in reveal 
                g.SetActive(true); // make then active
            }
            finalScore.text = score.ToString(); // update the final score text 
        }
            Debug.Log("LostALife");// debug lost of life
    }

    void MoveScoreBar() {
        // move the score bar wiithin the limitiations set but limits. 
        UnityEngine.Vector2 pos = new UnityEngine.Vector2(transform.position.x, Random.Range(limits.y, limits.x)); 
        scoreBar.transform.position = pos; // update the posision of the score bar

    }
}
