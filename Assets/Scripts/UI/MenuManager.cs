using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject optionsMenuUI;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private Button exitButton;

    private void Start() {
        InitializeUI();
        InitializeButtons();
    }

    #region Initialize methods
    private void InitializeUI() {
        mainMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    private void InitializeButtons() {
        playButton.onClick.AddListener(PlayButton);
        optionsButton.onClick.AddListener(OptionsButton);
        backToMenuButton.onClick.AddListener(BackToMenuButton);
        exitButton.onClick.AddListener(ExitButton);
    }
    #endregion

    #region Button methods
    public void PlayButton() {
        SceneManager.LoadScene("GameLevelScene");
    }

    public void OptionsButton() {
        mainMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void BackToMenuButton() {
        mainMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    public void ExitButton() {
        Debug.Log("Quit");
        Application.Quit();
    }
    #endregion
}
