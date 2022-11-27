using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public PlayerManager PlayerManager;
    public TextMeshProUGUI GameOverText;
    
    public static GameManager Instance;

    public bool GameOverTriggered;
    private void Awake() {
        if (Instance != null) {
            throw new Exception("Already created GameManager instance");
        }

        Instance = this;
    }

    public void GameOver(PlayerManager.PlayerID winner) {
        if (GameOverTriggered) return;
        GameOverTriggered = true;
        
        Debug.Log("Game over!");

        GameOverText.text = $"GAME OVER\nWinner: Player {(int)winner + 1}";
        GameOverText.gameObject.SetActive(true);
        Time.timeScale = 0;

        StartCoroutine(ResetGame());
    }

    private IEnumerator ResetGame() {
        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}