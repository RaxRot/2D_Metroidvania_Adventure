using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private float distanceToOpen =8 ;

    private PlayerController thePlayer;

    private bool _isPlayerExiting;

    [SerializeField] private Transform exitPoint;
    [SerializeField] private float movePlayerSpeed=10;

    [SerializeField] private string levelToLoad;

    [SerializeField] private GameObject winPanel;
    
    private void Start()
    {
        thePlayer = PlayerHealth.Instance.GetComponent<PlayerController>();
    }

    private void Update()
    {
        ControllDoor();

        if (_isPlayerExiting)
        {
            thePlayer.transform.position = Vector3.MoveTowards(thePlayer.transform.position, exitPoint.position,
                movePlayerSpeed * Time.deltaTime);
        }
    }

    private void ControllDoor()
    {
        if (Vector3.Distance(transform.position,thePlayer.transform.position)<=distanceToOpen)
        {
            anim.SetBool(TagManager.DOOR_IS_OPEN_PARAMETR,true);
        }
        else
        {
            anim.SetBool(TagManager.DOOR_IS_OPEN_PARAMETR,false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(TagManager.PLAYER_TAG))
        {
            if (!_isPlayerExiting)
            {
                thePlayer.canMove = false;
                
                UseDoor();
            }
        }
    }

    private void UseDoor()
    {
        StartCoroutine(nameof(_UseDoorCo));
    }

    private IEnumerator _UseDoorCo()
    {
        UIManager.Instance.StartFadeToBlack();
        yield return new WaitForSeconds(1.5f);
        
        _isPlayerExiting = true;
        
        thePlayer.StopPlayerAnimator(true);

        yield return new WaitForSeconds(1.5f);
        
        RespawnManager.Instance.SetSpawnPoint(exitPoint.position);
        thePlayer.canMove = true;
        thePlayer.StopPlayerAnimator(false);
        
        UIManager.Instance.StartFadeFromBlack();

        if (levelToLoad==TagManager.WIN_LEVEL_NAME)
        {
            ShowWinPanel();
        }
        else
        {
            PlayerPrefs.SetString("ContinueLevel",levelToLoad);
            PlayerPrefs.SetFloat("PosX",transform.position.x+2f);
            PlayerPrefs.SetFloat("PosY",transform.position.y+2f);
            PlayerPrefs.SetFloat("PosZ",transform.position.z);
        
            SceneManager.LoadScene(levelToLoad);
        }
    }

    private void ShowWinPanel()
    {
        StartCoroutine(nameof(_ShowWinPanelCo));
    }

    private IEnumerator _ShowWinPanelCo()
    {
        float alphaUI = 0;

        while (alphaUI<1)
        {
            winPanel.GetComponent<CanvasGroup>().alpha = alphaUI;
            yield return new WaitForSeconds(Time.deltaTime);
            alphaUI += Time.deltaTime;
        }

        if (alphaUI>=1)
        {
            Time.timeScale = 0f;
        }
    }
}
