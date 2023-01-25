using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Slider playerHealthSlider;

    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed = 2f;
    private bool _fadingToBlack, _fadingFromBlack;

    [SerializeField] private GameObject pausePanel;
    private bool _pausePanelIsActive;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Fade();
        
        PauseUnpause();
    }

    private void PauseUnpause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowHidePausePanel();
        }
    }

    private void ShowHidePausePanel()
    {
        _pausePanelIsActive = !_pausePanelIsActive;
        pausePanel.SetActive(_pausePanelIsActive);
        
        if (_pausePanelIsActive)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    
    public void GoToMenu()
    {
        Time.timeScale = 1f;

        PrepareElements();
        
        SceneManager.LoadScene(TagManager.MAIN_MENU_SCENE_NAME);
    }

    private void PrepareElements()
    {
        Destroy(PlayerHealth.Instance.gameObject);
        PlayerHealth.Instance = null;
        
        Destroy(RespawnManager.Instance.gameObject);
        RespawnManager.Instance = null;

        Instance = null;
        Destroy(gameObject);
    }

    private void Fade()
    {
        if (_fadingToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a==1f)
            {
                _fadingToBlack = false;
            }
        }
        else if (_fadingFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            
            if (fadeScreen.color.a==0f)
            {
                _fadingFromBlack = false;
            }
        }
        
    }

    public void UpdatePlayerHealth(int currentHealth,int maxHealth)
    {
        playerHealthSlider.maxValue = maxHealth;
        playerHealthSlider.value = currentHealth;
    }

    public void StartFadeToBlack()
    {
        _fadingToBlack = true;
        _fadingFromBlack = false;
    }

    public void StartFadeFromBlack()
    {
        _fadingToBlack = false;
        _fadingFromBlack = true;
    }
}
