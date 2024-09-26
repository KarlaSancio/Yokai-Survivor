using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        InGame,
        Pause,
        GameOver,
        LevelUp
    }

    public GameState currentGameState;
    public GameState previousGameState;

    [Header("UI")]
    public GameObject pauseMenu;
    public GameObject resultsMenu;
    public GameObject levelUpMenu;

    [Header("Player Stats Display")]
    // Status atuais do jogo (que vao aparecer no menu de pausa)
    public Text currentHealthText;
    public Text currentMightText;
    public Text currentRecoveryItemText;
    public Text currentProjectileSpeedText;
    public Text currentMoveSpeedText;
    public Text currentMagnetText;

    [Header("Run Results Display")]
    public Image chosenCharacterImage;
    public Text chosenCharacterName;
    public Text levelReachedText;
    public Text timeSurvivedText;
    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItensUI = new List<Image>(6);

    [Header("Timer")]
    public float timeLimit; // Tempo limite para o jogo
    float timerTime; // Tempo atual do jogo
    public Text timerText; // Texto que vai mostrar o tempo

    // flag de game over
    public bool isGameOver = false;

    // flag de level up
    public bool choosingUpgrade = false;

    public GameObject playerObject;

    void Awake()
    {
        // Checa se ja existe uma instancia(Singleton) de GameManager
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Ja existe uma instancia de GameManager," + this +"foi destruido");
            Destroy(gameObject);
        }

        disableMenus();
    }

    void Update()
    {

        switch (currentGameState)
        {
            case GameState.InGame:
                // Game is running
                PauseAndResumeChecker();
                UpdateTimer();
                break;
            case GameState.Pause:
                // Game is paused
                PauseAndResumeChecker();
                break;
            case GameState.GameOver:
                // Game is over
                if(!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("Game Over");
                    ShowRunResults();
                }
                break;

            case GameState.LevelUp:
                // Level up
                if(!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f;
                    levelUpMenu.SetActive(true);
                    Debug.Log("Level Up");
                }
                break;
            default:
                Debug.LogWarning("Invalid game state");
                break;
        }
    }

    public void modifyGameState(GameState newGameState) //Funcao para mudar o estado do jogo
    {
        currentGameState = newGameState;
    }

    public void PauseGame()
    {
        if (currentGameState != GameState.Pause)
        {
            previousGameState = currentGameState;
            modifyGameState(GameState.Pause);
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            Debug.Log("Game Paused");
        }

    }

    public void ResumeGame()
    {
        if(currentGameState == GameState.Pause)
        {
            modifyGameState(previousGameState);
            Time.timeScale = 1f; // Para despausar o jogo
            pauseMenu.SetActive(false);
            Debug.Log("Game Resumed");
        }
    }

    void PauseAndResumeChecker()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
           if (currentGameState == GameState.Pause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void GameOver()
    {
        timeSurvivedText.text = timerText.text;
        modifyGameState(GameState.GameOver);
    }

    void disableMenus()
    {
        pauseMenu.SetActive(false);
        resultsMenu.SetActive(false);
        levelUpMenu.SetActive(false);
    }

    void ShowRunResults()
    {
        resultsMenu.SetActive(true);
    }

    public void AssignChoosenCharacterUI(CharacterScriptableObject characterData)
    {
        chosenCharacterImage.sprite = characterData.Icon;
        chosenCharacterName.text = characterData.name;
    }

    public void AssignLevelReachedUI(int level)
    {
        levelReachedText.text = level.ToString();
    }

    public void AssignChoosenWeaponsAndPassiveItensUI(List<Image> weapons, List<Image> passiveItens)
    {
        if (weapons.Count != chosenWeaponsUI.Count || passiveItens.Count != chosenPassiveItensUI.Count)
        {
            Debug.LogWarning("Tamanho das listas de armas e itens passivos diferentes do esperado");
            return;
        }

        for (int i = 0; i <chosenWeaponsUI.Count; i++)
        {
            if(weapons[i].sprite)
            {
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = weapons[i].sprite;
            }
            else
            {
                chosenWeaponsUI[i].enabled = false;
            }
        }

        for (int i = 0; i < chosenPassiveItensUI.Count; i++)
        {
            if(passiveItens[i].sprite)
            {
                chosenPassiveItensUI[i].enabled = true;
                chosenPassiveItensUI[i].sprite = passiveItens[i].sprite;
            }
            else
            {
                chosenPassiveItensUI[i].enabled = false;
            }
        }
    }

    void UpdateTimer()
    {
        timerTime += Time.deltaTime;

        UpdateTimerDisplay();

        if(timerTime >= timeLimit)
        {
            GameOver();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timerTime / 60f);
        int seconds = Mathf.FloorToInt(timerTime % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        modifyGameState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f;
        levelUpMenu.SetActive(false);
        modifyGameState(GameState.InGame);
        Debug.Log("Level Up Ended");
    }
} 

