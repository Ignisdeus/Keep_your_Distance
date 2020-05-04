using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public UnityEngine.Vector2 limits;
    public Text scoreDisplay; 
    AudioSource audioSource; 
    public AudioClip coinPickUp, hit; 
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public Text finalScore;
    public GameObject[] reveal; 

    void Update()
    {
        BarMovement();

        if(!pause && Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(PlayerInput());

        }
        if(lives <= 0) {
            pause = true;
            
            if(Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene(0);
            }
            
            
        }


    }

    public GameObject bar, scoreBar; 
    public float speed= 5f;  
    void BarMovement() {

        if(bar.transform.position.y > limits.x || bar.transform.position.y < limits.y) {
            speed *= -1; 
        }

        if(!pause) {
            bar.transform.Translate(UnityEngine.Vector2.up * speed * Time.deltaTime);
        }

    }

    bool pause = false; 
    IEnumerator PlayerInput() {

        pause = true;
        ScoreCalualte();
        yield return new WaitForSeconds(0.5f);
        MoveScoreBar();
        speed *= -1;
        pause = false;


    }

    int score = 0; 
    int lives = 6; 
    void ScoreCalualte() {
        float oldScore = score; 
        float x = 100; 
        float distance = Mathf.Abs(bar.transform.position.y - scoreBar.transform.position.y);
        int addToScore =0; 
        
        if(distance > 0.51f) {
            LostAlive();
            audioSource.PlayOneShot(hit, 1f);
        }
        else if(distance < 0.5f) {
            addToScore = 25;
            audioSource.PlayOneShot(coinPickUp, 1f);
            DifficultyScale();
        }
        else if(distance < 0.3f) {
            addToScore = 50;
            audioSource.PlayOneShot(coinPickUp, 1f);
            DifficultyScale();
        }
        else if(distance < 0.1f) {
            addToScore = 100;
            audioSource.PlayOneShot(coinPickUp, 1f);
            DifficultyScale();
        }
        score += addToScore; 
        scoreDisplay.text = score.ToString(); 
    }

    void DifficultyScale() {

        if(speed < 10f && speed > 0) {
            speed += 0.5f;
        }
        else if(speed > -10f && speed < 0) {
            speed -= 0.5f;
        }
    }

    public  Image[] heart = new Image[6];
    void LostAlive() {
        lives --;
        for(int i =0; i < heart.Length; i++) {

            if(i < lives) {
                heart[i].enabled = true;
            }else {
                heart[i].enabled = false;
            }

        }

        if(lives <= 0) {

            foreach(GameObject g in reveal) {
                g.SetActive(true); 
            }
            finalScore.text = score.ToString(); 
        }
            Debug.Log("LostALife");
    }

    void MoveScoreBar() {

        UnityEngine.Vector2 pos = new UnityEngine.Vector2(transform.position.x, Random.Range(limits.y, limits.x));
        scoreBar.transform.position = pos; 

    }
}
