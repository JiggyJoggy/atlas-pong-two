using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;

namespace ZPong
{


    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        public int scorePlayer1 = 0; // Score for Player 1
        public int scorePlayer2 = 0; // Score for Player 2
        public int winningScore = 11; // Score required to win the game

        public Animator leftGoalExplosion;
        public Animator rightGoalExplosion;

        public GameObject FlashUI;

        public TMP_Text leftScoreText;
        public TMP_Text rightScoreText;
        public GameObject victoryUI;
        public TMP_Text victoryText;

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            winningScore = PlayerPrefs.GetInt("ScoreToWin");
        }

        // Call this function when player 1 scores
        public void ScorePointPlayer1()
        {
            scorePlayer1++;
            leftScoreText.text = "" + scorePlayer1;
            StartCoroutine(FlashScreen());
            rightGoalExplosion.SetTrigger("ScoredRight");
            Debug.Log("Player 1 scored! Current score: " + scorePlayer1);

            PostScore();
        }

        // Call this function when player 2 scores
        public void ScorePointPlayer2()
        {
            scorePlayer2++;
            rightScoreText.text = "" + scorePlayer2;
            StartCoroutine(FlashScreen());
            leftGoalExplosion.SetTrigger("ScoredLeft");
            Debug.Log("Player 2 scored! Current score: " + scorePlayer2);

            PostScore();
        }

        private void PostScore()
        {
            if (CheckWinCondition())
            {
                GameManager.Instance.ResetBall();
            }
        }

        // Check if either player has reached the winning score
        //Returns true if the game can continue
        //Returns false if a player won
        private bool CheckWinCondition()
        {
            bool output = true; 
            
            if (scorePlayer1 >= winningScore)
            {
                Debug.Log("Player 1 wins!");
                victoryUI.gameObject.SetActive(true);
                VictoryMusicManager.Instance.PlayVictoryMusic();

                GameManager.Instance.activeBall.DisableBall();

                victoryText.text = "PLAYER 1\nWINS";
                
                output = false;
            }
            else if (scorePlayer2 >= winningScore)
            {
                Debug.Log("Player 2 wins!");
                victoryUI.gameObject.SetActive(true);
                VictoryMusicManager.Instance.PlayVictoryMusic();

                GameManager.Instance.activeBall.DisableBall();

                victoryText.text = "PLAYER 2\nWINS";
                output = false;
            }

            return output;
        }

        public void ResetGame()
        {
            scorePlayer1 = 0;
            scorePlayer2 = 0;

            leftScoreText.text = "0";
            rightScoreText.text = "0";

            victoryUI.gameObject.SetActive(false);
        }
        public IEnumerator FlashScreen()
        {
            FlashUI.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            FlashUI.SetActive(false);
        }
    }
}