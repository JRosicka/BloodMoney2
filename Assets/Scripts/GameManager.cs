using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public PlayerManager PlayerManager;
    public SoundManager SoundManager;
    
    [Header("Game Over")]
    public TextMeshProUGUI GameOverText;
    public SoundCall GameOverSound;
    
    public static GameManager Instance;

    [HideInInspector]
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
        SoundManager.thisSoundManager.PlaySound(GameOverSound, gameObject);
        
        Time.timeScale = 0;

        StartCoroutine(ResetGame());
    }

    private IEnumerator ResetGame() {
        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}