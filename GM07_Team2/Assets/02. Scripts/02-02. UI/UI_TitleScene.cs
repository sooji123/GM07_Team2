using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_TitleScene : MonoBehaviour
{
    [SerializeField]
    private Button _newGameButton;
    [SerializeField]
    private Button _continueButton;
    [SerializeField]
    private Button _exitButton;

    private void Awake()
    {
        if (_newGameButton != null)
        {
            _newGameButton.onClick.AddListener(OnClickNewGame);
        }
        if (_continueButton != null)
        {
            _continueButton.onClick.AddListener(OnClickContinue);
        }
        if (_exitButton != null)
        {
            _exitButton.onClick.AddListener(OnClickExit);
        }
    }

    private void OnClickNewGame()
    {
        SceneManager.LoadScene(ESceneName.Maptest.ToString());
    }
    private void OnClickContinue()
    {
        SceneManager.LoadScene(ESceneName.Maptest.ToString());
    }

    private void OnClickExit()
    {
        Application.Quit();
    }
}
