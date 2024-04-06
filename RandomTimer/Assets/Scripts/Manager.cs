using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public bool gameModeSelected;
    [SerializeField] public int selectedGameMode;
    [SerializeField] GameObject gamePanel;
    CanvasGroup gameCanvas;
    [SerializeField] GameObject modePanel;
    CanvasGroup modeCanvas;
    [SerializeField] GameObject buttonSoundObj;
    AudioSource buttonAudio;
    Collider2D[] colliders;

    [SerializeField] GameObject linesPanel;

    [SerializeField] GameObject okButtonObj;
    Button okButton;


    // Start is called before the first frame update
    void Start()
    {
        colliders = gamePanel.GetComponentsInChildren<Collider2D>();

        if (okButtonObj != null)
        {
            okButton = okButtonObj.GetComponent<Button>();
            okButton.interactable = false;
        }

        if (gamePanel != null)
        {
            gameCanvas = gamePanel.GetComponent<CanvasGroup>();
            gameCanvas.alpha = 0f;
            gameCanvas.blocksRaycasts = false;
            foreach (Collider2D collider in colliders)
                collider.enabled = false;
        }
        if (modePanel != null)
        {
            modeCanvas = modePanel.GetComponent<CanvasGroup>();
            modeCanvas.alpha = 1f;
            modeCanvas.blocksRaycasts = true;
        }
        gameModeSelected = false;
        buttonAudio = buttonSoundObj.GetComponent<AudioSource>();
    }

    public void SetGameMode(int gameModeID)
    {
        Lines lines = linesPanel.GetComponent<Lines>();
        lines.gameModeId = gameModeID;

        if (gameModeID == 4)
        {
            int randomMode = Random.Range(1, 4);
            selectedGameMode = randomMode;
        }
        else
            selectedGameMode = gameModeID;

        if (!okButton.interactable)
            okButton.interactable = true;

        Debug.Log($"Selected game mode:{selectedGameMode}");
    }


    public void GameModeSelected()
    {        
        gameModeSelected = true;

        gameCanvas.alpha = 1f;
        gameCanvas.blocksRaycasts = true;
        foreach (Collider2D collider in colliders)
            collider.enabled = true;

        modeCanvas.alpha = 0f;
        modeCanvas.blocksRaycasts = false;

        Debug.Log("Start game");
    }

    public void MainMenu()
    {
        Lines lines = linesPanel.GetComponent<Lines>();
        lines.gameModeId = 0;

        selectedGameMode = 0;
        gameModeSelected = false;

        if (okButton.interactable)
            okButton.interactable = false;

        gameCanvas.alpha = 0f;
        gameCanvas.blocksRaycasts = false;
        foreach (Collider2D collider in colliders)
            collider.enabled = false;
        modeCanvas.alpha = 1f;
        modeCanvas.blocksRaycasts = true;

        Debug.Log("Back to main menu");
    }

    public void ButtonClicked()
    {
        buttonAudio.Play();
        Debug.Log("Button Clicked Sound");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
