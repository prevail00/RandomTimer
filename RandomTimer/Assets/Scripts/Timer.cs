using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] GameObject managerObj;

    [SerializeField] GameObject minuteTextObj;
    [SerializeField] GameObject secTextObj;
    [SerializeField] GameObject beepTextObj;
    TMP_Text beepText;
    [SerializeField] GameObject[] setInactiveGameObjects;


    //Probability of Beep
    [SerializeField, Range(0f, 1f)] float minProb = 0.1f;
    [SerializeField, Range(0f, 1f)] float maxProb;
    [SerializeField, Range(0f, 1f)] float constProb = 0.2f;
    public float radius;
    public float actProb;    
    public int maxTicCount;
    public float amplitude;
    
    [SerializeField] float tickTime = 1;  // Every seconds
    public bool paused = true;
    public bool reset = true;
    float timePassed = 0;
    public int tickCount = 0;
    [SerializeField] int wakeUpTime = 3;
    public int startingMin = 0;
    public int startingSec = 0;
    [SerializeField] int maxMinutes = 5;

    public int beepCount;

    [SerializeField] GameObject startPauseButtonObj;
    Button startPauseButton;
    [SerializeField] GameObject minutePanel;
    [SerializeField] GameObject secondsPanel;
    
    int currentTime;
    int startTime;
    public int gameMode;

    [SerializeField] GameObject timerSoundObj;
    AudioSource timerAudio;
    
    [SerializeField] GameObject setupSoundObj;
    AudioSource setupAudio;



    [SerializeField] GameObject boomSoundObj;
    AudioSource beepAudio;
    void Start()
    {
        paused = true;
        reset = true;
        beepCount = 0;

        beepText = beepTextObj.GetComponent<TMP_Text>();              
        beepText.text = $"Beep Count:\n{beepCount:00}";

        startPauseButton = startPauseButtonObj.GetComponent<Button>();
        startPauseButton.interactable = false;

        timerAudio = timerSoundObj.GetComponent<AudioSource>();
        setupAudio = setupSoundObj.GetComponent<AudioSource>();
        beepAudio = boomSoundObj.GetComponent<AudioSource>();
    }
    void Update()
    {
        //Counting real time
        if (!paused)
        {
            timePassed += Time.deltaTime;
            if (timePassed > tickTime)
            {
                timePassed -= tickTime;
                tickCount++;
                OnTick(tickCount);
            }
        }


        if (reset)
        {
            startTime = startingMin * 60 + startingSec;
            if (startTime > 0)
            {
                if (startPauseButton.interactable == false)
                    startPauseButton.interactable = true;
            }
            else
            {
                if (startPauseButton.interactable == true)
                    startPauseButton.interactable = false;
            }
            currentTime = startTime;
        }            
        DisplayTime();
    }

    void OnTick(int count)
    {
        //Functions of probability based on game modes
        amplitude = maxProb - minProb;        
        if (gameMode == 2) //Gauss
        {
            if (count > wakeUpTime)
            {
                actProb = minProb + amplitude * Mathf.Exp(-(Mathf.Pow(((count - (wakeUpTime + 1) - maxTicCount)/ 2), 2) / (2 * Mathf.Pow(radius, 2))));
                Debug.Log($"Actual Gauss probability:{actProb}");
            }                
        }
        else if (gameMode == 3) //F&R
        {
            if (count > wakeUpTime)
            {
                if (count < ((maxTicCount - wakeUpTime) / 2))
                {
                    actProb = minProb + amplitude * Mathf.Exp(-(Mathf.Pow((count - (wakeUpTime + 1)), 2) / (2 * Mathf.Pow(radius, 2))));
                    Debug.Log($"Actual F%R probability:{actProb}");
                }
                else
                {
                    actProb = minProb + amplitude * Mathf.Exp(-(Mathf.Pow(count - (maxTicCount + wakeUpTime - 1), 2) / (2 * Mathf.Pow(radius, 2))));
                    Debug.Log($"Actual F%R probability:{actProb}");
                }
            }
        }
        else //Constant probability
        {
                actProb = constProb;
        }
        

        //Timer
        if (currentTime > 0)
        {
            if (currentTime == 1)
            {
                Beep();
                if (startPauseButton.interactable == true)
                    startPauseButton.interactable = false;
            }
            else
            {
                if (tickCount > wakeUpTime)
                    Probability();
            }
            currentTime --;
            timerAudio.Play();
            Debug.Log($"Seconds left: {currentTime:00}");
            Debug.Log("Tick sound");
        }
        else
        {
            paused = true;                       
        }
        DisplayTime();    
    }

    public void StartOrPauseTimer()
    {
        if (paused)
        {
            if (currentTime == startTime)
            {
                Manager manager = managerObj.GetComponent<Manager>();
                gameMode = manager.selectedGameMode;
                maxTicCount = startTime;
                radius = (maxTicCount - wakeUpTime) / 10;
            }
            if (currentTime > 0)
            {                
                paused = false;
                reset = false;
                ChangeButton();
            }            
        }
        else
        {
            paused = true;
            ChangeButton();
        }
    }
    public void ResetTimer()
    {
        currentTime = startingMin * 60 + startingSec;
        timePassed = 0;
        tickCount = 0;

        beepCount = 0;
        beepText.text = $"Beep Count:\n{beepCount:00}";

        actProb = minProb;

        DisplayTime();
        
        if (paused)
        {
            StopAllCoroutines();
            reset = true;                        
        } 
        ChangeButton();
        Debug.Log("Reseted");
    }
    void DisplayTime()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int actSec = Mathf.FloorToInt(currentTime % 60);
        if (minuteTextObj != null)
        {
            TMP_Text minuteText = minuteTextObj.GetComponent<TMP_Text>();
            minuteText.text = minutes.ToString("00");
        }
        if (secTextObj != null)
        {
            TMP_Text secText = secTextObj.GetComponentInChildren<TMP_Text>();
            secText.text = actSec.ToString("00");
        }
    }
    void ChangeButton()
    {
        TMP_Text buttonText = startPauseButtonObj.GetComponentInChildren<TMP_Text>();
        Image buttonImage = startPauseButtonObj.GetComponentInChildren<Image>();
        if (!paused)
        {
            buttonText.text = "Pause";
            buttonImage.color = Color.red;
        }
        else if (currentTime > 0 && currentTime != startTime)
        {
            buttonText.text = "Resume";
            buttonImage.color = Color.yellow;
        }
        else
        {
            buttonText.text = "Start";
            buttonImage.color = Color.green;
        }
    }

    void Beep()
    {
        beepAudio.Play();
        Debug.Log("Beep sound");
        beepCount++;
        beepText.text = $"Beep Count:\n{beepCount:00}";
    }

    void Probability()
    {
        float randomValue = Random.value;
        Debug.Log(randomValue);
        if (randomValue < actProb)
            Beep();
    }

    public void Zero()
    {
        paused = true;
        Debug.Log("Timer set to 00:00");
        startingMin = 0;
        startingSec = 0;
        startTime = 0;
        ResetTimer();
    }

    public void PlusSec()
    {
        if (reset)
        {
            if (startingSec < 59)
            {
                startingSec += 1;
            }
            else
            {
                startingSec = 0;
            }
            Debug.Log("+1 sec");
            setupAudio.Play();
            Debug.Log("Setup button sound");
        }
    }

    public void MinusSec()
    {
        if (reset)
        {
            if (startingSec > 0)
            {
                startingSec -= 1;
            }
            else
            {
                if (startingMin > 0)
                {
                    startingMin -= 1;
                    startingSec = 59;
                }
                else
                    startingSec = 59;
            }
            setupAudio.Play();
            Debug.Log("Setup button sound");
            Debug.Log("-1 sec");
        }
    }

    public void PlusMin()
    {
        if (reset)
        {
            if (startingMin < maxMinutes)
            {
                startingMin += 1;
                setupAudio.Play();
                Debug.Log("Setup button sound");
                Debug.Log("+1 min");
            }
        }
    }
    
    
    public void MinusMin()
    {
        if (reset)
        {
            if (startingMin > 0)
            {
                startingMin -= 1;
                setupAudio.Play();
                Debug.Log("Setup button sound");
                Debug.Log("-1 min");
            }
        }
    }

    
}
