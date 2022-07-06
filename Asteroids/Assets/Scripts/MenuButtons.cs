using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private string firstControl, secondControl;
    [SerializeField] private CoreLogic coreLogic;
    [SerializeField] private TextMeshProUGUI controlText;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private Button continueButton;
    private CanvasGroup menuGroup;

    private bool menuOpened = true;

    private void Awake()
    {
        Time.timeScale = 0;
        menuGroup = GetComponent<CanvasGroup>();
        LoadControl();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !menuOpened)
            OpenMenu();
    }

    public void NewGame()
    {
        if(coreLogic.gameStarted)
            SceneManager.LoadScene(0);

        gameUI.SetActive(true);
        continueButton.interactable = true;
        coreLogic.OnStartGame();
        CloseMenu();
    }
    public void OpenMenu()
    {
        Time.timeScale = 0;
        menuOpened = true;
        ChangeUiCondition(menuGroup);
    }
    public void CloseMenu()
    {
        Time.timeScale = 1;
        menuOpened = false;
        ChangeUiCondition(menuGroup);
    }

    private void ChangeUiCondition(CanvasGroup group)
    {
        if(group.alpha == 0)
        {
            menuGroup.alpha = 1;
            menuGroup.blocksRaycasts = true;
            menuGroup.interactable = true;
        } else
        {
            menuGroup.alpha = 0;
            menuGroup.blocksRaycasts = false;
            menuGroup.interactable = false;
        }
    }

    private void LoadControl()
    {
        if (!PlayerPrefs.HasKey("mouseControl"))
        {
            OnChangeControl();
            return;
        }

        if(PlayerPrefs.GetInt("mouseControl") == 0)
            controlText.SetText(firstControl);
        else
            controlText.SetText(secondControl);

        coreLogic.ChangeControl();
    }
    public void OnChangeControl()
    {
        if (controlText.text == firstControl)
        {
            controlText.SetText(secondControl);
            PlayerPrefs.SetInt("mouseControl", 1);
        } else
        {
            controlText.SetText(firstControl);
            PlayerPrefs.SetInt("mouseControl", 0);
        }

        coreLogic.ChangeControl();
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
