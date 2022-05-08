using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject menuObject;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI praiseText;
    [SerializeField] private TextMeshProUGUI characterSizeText;
    [SerializeField] private TextMeshProUGUI tapTimingText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    [Header("Praise Settings")]
    [SerializeField] private string[] praiseWords;
    [Header("Sliders")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private GameObject tapSkorBar;

    [Header("Events")]
    [SerializeField] private UnityEvent onWinUI;
    [SerializeField] private UnityEvent onLoseUI;
    [SerializeField] private UnityEvent onGameStartUI;
    [SerializeField] private UnityEvent forceToCloseOnNewLevel;

    [Header("Images")]
    [SerializeField] private Image tapTimingBar;
    [SerializeField] private Image tapTimingTextArea;


    private float deltaTime;
    private int inputCounter = 0;
    private bool isUpdating = false;

    private bool isPowerUp = true;
    private bool isDirection= true;

    private float power = 0;
    private float powerSpeed = 100f;

    private float mamaValue;



    private void Awake()
    {
        onGameStartUI?.Invoke();
        UpdateLevelText();

        if (GameManager.Instance.IsDebug && !menuObject.activeInHierarchy)
        {
            GameManager.Instance.currentState = GameManager.GameState.Normal;
        }
    }
    private void OnEnable()
    {

        GameManager.onWinEvent += ExecuteOnWin;
        GameManager.onWinEvent += SetTapTimingBarPassive;
        GameManager.onBossScene += SetTapTimingBarActive;
        LevelManager.onNewLevelLoaded += UpdateLevelText;
        GameManager.onLoseEvent += ExecuteOnLose;
        LevelManager.onNewLevelLoaded += ForceToClose;
        InputManager.Instance.onTouchStart += CloseTheMenu;
        GameManager.Instance.onCharacterTake += OnNewCharacterTake;

    }

    private void OnDisable()
    {
        GameManager.onWinEvent -= ExecuteOnWin;
        GameManager.onLoseEvent -= ExecuteOnLose;
        LevelManager.onNewLevelLoaded -= UpdateLevelText;
        LevelManager.onNewLevelLoaded -= ForceToClose;
        InputManager.Instance.onTouchStart -= CloseTheMenu;
        GameManager.Instance.onCharacterTake -= OnNewCharacterTake;

    }

    public void SetProgresBarMaxValue(float maxValue)
    {
        progressBar.maxValue = maxValue;
    }
    public void UpdateProgressBar(float newTargetValue, float duration = 1f)
    {
        if (isUpdating)
        {
            return;
        }
        StartCoroutine(ProgressBar(newTargetValue, duration));
    }
    private IEnumerator ProgressBar(float newTargetValue, float duration = 1f)
    {
        float startingValue = progressBar.value;
        float targetValue = newTargetValue;
        float elapsedTime = 0f;
        isUpdating = true;

        while (elapsedTime < duration)
        {
            progressBar.value = Mathf.Lerp(startingValue, targetValue, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        progressBar.value = targetValue;
        isUpdating = false;
    }
    void Update()
    {
        if (GameManager.Instance.IsDebug)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ScreenshotHandler.TakeScreenshotOnGame();
            }

            ShowFPS();
        }

        if (GameManager.Instance.currentState == GameManager.GameState.Boss)
        {
            ShowTapTimingBar();

        }

    }

    /// <summary>
    /// Karýþýk gelmesini istiyorsanýz parametreyi boþ "" yada null býrakýn
    /// </summary>
    /// <param name="newText"></param>
    public void ShowPraiseText(string newText)
    {
        Animator praiseAnim = praiseText.GetComponent<Animator>();

        if (!praiseAnim.IsInTransition(0) && praiseAnim.GetCurrentAnimatorStateInfo(0).IsName("PraiseTextAnimation"))
        {
            return;
        }

        if (String.IsNullOrEmpty(newText))
        {
            praiseText.text = GetRandomWord();
        }
        else
        {
            praiseText.text = newText;
        }

        praiseAnim.SetTrigger("Text");
    }

    [ContextMenu("Show Random Praise Text Debug")]
    private void ShowRandomPraiseTextDebug()
    {
        Animator praiseAnim = praiseText.GetComponent<Animator>();

        if (!praiseAnim.IsInTransition(0) && praiseAnim.GetCurrentAnimatorStateInfo(0).IsName("PraiseTextAnimation"))
        {
            return;
        }
        praiseText.text = GetRandomWord();
        praiseAnim.SetTrigger("Text");
    }

    public string GetRandomWord()
    {
        int index = UnityEngine.Random.Range(0, praiseWords.Length);
        return praiseWords[index];
    }
    private void ForceToClose()
    {
        inputCounter = 0;
        GameManager.Instance.GiveInputToUser = false;
        SetMenuOnNewLevelLoad();
        forceToCloseOnNewLevel?.Invoke();

    }
    private void SetMenuOnNewLevelLoad()
    {
        if (GameManager.Instance.ShowMenuOnNewSceneLoaded)
        {
            menuObject.gameObject.SetActive(true);
        }
        else
        {
            inputCounter = 1;
        }
    }
    private void CloseTheMenu()
    {
        if (GameManager.Instance.TakeInputCount || inputCounter < 3)
        {
            inputCounter++;
        }

        if (inputCounter >= 1 && menuObject.activeInHierarchy)
        {
            menuObject.gameObject.SetActive(false);
        }

        if (inputCounter >= 1 && inputCounter < 3)
        {
            GameManager.Instance.currentState = GameManager.GameState.Normal;

            if (GameManager.Instance.GiveInputOnFirstClick)
            {
                GameManager.Instance.GiveInputToUser = true;
            }
        }
        if (inputCounter >= 2 && inputCounter < 3 && !GameManager.Instance.GiveInputOnFirstClick)
        {
            GameManager.Instance.GiveInputToUser = true;
        }
    }
    private void ShowFPS()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
    private void UpdateLevelText()
    {
        levelText.text = "Level" + " " + LevelManager.Instance.GetPlayedLevelCount().ToString();
    }
    private void ExecuteOnWin()
    {
        GameManager.Instance.currentState = GameManager.GameState.Victory;
        onWinUI?.Invoke();
    }
    private void ExecuteOnLose()
    {
        GameManager.Instance.currentState = GameManager.GameState.Failed;
        onLoseUI?.Invoke();
    }

    private void OnNewCharacterTake(int amount)
    {
        characterSizeText.text = amount.ToString();

        mamaValue = amount;

        Debug.Log(mamaValue);

    }


    private void PoweActive()
    {

        if(isDirection == true)
        {
            power += Time.deltaTime * powerSpeed;

            if(power > 100)
            {
                isDirection = false;
                power = 100;
            }
            
        }
        else
        {
            power -= Time.deltaTime * powerSpeed;
            if (power < 0)
            {
                isDirection = true;
                power = 0;
            }
        }

        tapTimingBar.fillAmount = power / 100;
    }

    private void EndPower()
    {
        isPowerUp = false;
        tapTimingText.text = "x" + (power/3.3f).ToString("f0");
        tapTimingTextArea.color = Color.Lerp(Color.red, Color.green, power/100);
    }

    private void ShowTapTimingBar()
    {

        if (isPowerUp == true)
        {
            PoweActive();
        }

        if (Input.GetMouseButton(0))
        {
            EndPower();
        }
    }

    private void SetTapTimingBarActive()
    {
        tapSkorBar.SetActive(true);
    }

    private void SetTapTimingBarPassive()
    {
        tapSkorBar.SetActive(false);
        Debug.Log(mamaValue);
        totalScoreText.text = "Score: "  + (mamaValue * (power / 3.3f)).ToString("f0");

    }

    public float GetPower()
    {
        return power;
    }


}
