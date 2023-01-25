using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossBatle : MonoBehaviour
{
    private CameraController _theCam;
    [SerializeField] private Transform camPosition;
    [SerializeField] private float camSpeed;

    [SerializeField] private GameObject phantomUI;

    [SerializeField] private int treshold1, treshold2;
    [SerializeField] private float activeTime, fadeOutTime, inActiveTime;
    private float _activeCounter, _fadeCounter, _inActiveCounter;

    [SerializeField] private Transform[] spawnPoints;
    private Transform targetPoint;
    [SerializeField] private float moveSpeed;

    [SerializeField] private Animator anim;
    [SerializeField] private Transform theboss;

    [SerializeField] private float timeBetweenShoots1, timeBetweenShoots2;
    private float _shootCounter;
    [SerializeField] private GameObject bossBullet;
    [SerializeField] private Transform shootPoint;

    [SerializeField] private GameObject winObject;

    [SerializeField] private GameObject bossDeadFX;

    private bool _isEndBattle;

    private void Start()
    {
        SetValues();
    }

    private void SetValues()
    {
        _theCam = FindObjectOfType<CameraController>();
        _theCam.enabled = false;
        
        ViewPhantomUI();

        _activeCounter = activeTime;
        _shootCounter = timeBetweenShoots1;
        
        AudioManager.Instance.PlayBossMusic();
    }

    private void Update()
    {
       MoveCameraToBattle();
       
       Fight();
    }

    private void MoveCameraToBattle()
    {
        _theCam.transform.position =
            Vector3.MoveTowards(_theCam.transform.position, camPosition.position, camSpeed * Time.deltaTime);
    }

    private void Fight()
    {
        if (!_isEndBattle)
        {
            if (BossHealthController.Instance.CurrentHealth>treshold1)
        {
            if (_activeCounter>0)
            {
                _activeCounter -= Time.deltaTime;
                if (_activeCounter<=0)
                {
                    _fadeCounter = fadeOutTime;
                    anim.SetTrigger(TagManager.BOSS_VANISH_TRIGGER);
                }

                _shootCounter -= Time.deltaTime;
                if (_shootCounter<=0)
                {
                    _shootCounter = timeBetweenShoots1;
                    
                    Instantiate(bossBullet, shootPoint.position, Quaternion.identity);
                }

            }else if (_fadeCounter>0)
            {
                _fadeCounter -= Time.deltaTime;
                if (_fadeCounter<=0)
                {
                    theboss.gameObject.SetActive(false);
                    _inActiveCounter = inActiveTime;
                }
            }else if (_inActiveCounter>0)
            {
                _inActiveCounter -= Time.deltaTime;
                if (_inActiveCounter<=0)
                {
                    theboss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                    theboss.gameObject.SetActive(true);

                    _activeCounter = activeTime;

                    _shootCounter = timeBetweenShoots1;
                }
            }
        }
        else
        {
            if (targetPoint==null)
            {
                targetPoint = theboss;
                _fadeCounter = fadeOutTime;
                anim.SetTrigger(TagManager.BOSS_VANISH_TRIGGER);
            }
            else
            {
                if (Vector3.Distance(theboss.position,targetPoint.position)>0.02f)
                {
                    theboss.position = Vector3.MoveTowards(theboss.position, targetPoint.position,
                        moveSpeed * Time.deltaTime);
                    
                    if (Vector3.Distance(theboss.position,targetPoint.position)<=0.02f)
                    {
                        _fadeCounter = fadeOutTime;
                        anim.SetTrigger(TagManager.BOSS_VANISH_TRIGGER);
                    }
                    
                    _shootCounter -= Time.deltaTime;
                    if (_shootCounter<=0)
                    {
                        if (PlayerHealth.Instance.CurrentHealth>treshold2)
                        {
                            _shootCounter = timeBetweenShoots1;
                        }
                        else
                        {
                            _shootCounter = timeBetweenShoots2;
                        }

                        Instantiate(bossBullet, shootPoint.position, Quaternion.identity);
                    }
                    
                }else if (_fadeCounter>0)
                {
                    _fadeCounter -= Time.deltaTime;
                    if (_fadeCounter<=0)
                    {
                        theboss.gameObject.SetActive(false);
                        _inActiveCounter = inActiveTime;
                    }
                }else if (_inActiveCounter>0)
                {
                    _inActiveCounter -= Time.deltaTime;
                    if (_inActiveCounter<=0)
                    {
                        theboss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

                        targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                        int whileBreaker = 0;
                        while (targetPoint.position==theboss.position && whileBreaker<10)
                        {
                            targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                            whileBreaker++;
                        }
                        
                        theboss.gameObject.SetActive(true);
                        
                        if (PlayerHealth.Instance.CurrentHealth>treshold2)
                        {
                            _shootCounter = timeBetweenShoots1;
                        }
                        else
                        {
                            _shootCounter = timeBetweenShoots2;
                        }
                    }
                }
            }
        }
        }
        else
        {
            _fadeCounter -= Time.deltaTime;
            if (_fadeCounter<0)
            {
                if (winObject!=null)
                {
                    winObject.SetActive(true);
                    winObject.transform.SetParent(null);
                }
                
                AudioManager.Instance.PlayLevelMusic();
                _theCam.enabled = true;
                gameObject.SetActive(false);
                
                
            }
        }
    }

    private void ViewPhantomUI()
    {
        StartCoroutine(nameof(_ViewPhantomUICo));
    }

    private IEnumerator _ViewPhantomUICo()
    {
        float alphaUI = 0;

        while (alphaUI<1)
        {
            phantomUI.GetComponent<CanvasGroup>().alpha = alphaUI;
            yield return new WaitForSeconds(Time.deltaTime);
            alphaUI += Time.deltaTime;
        }
    }

    public void EndBattle()
    {
        
        Instantiate(bossDeadFX, shootPoint.position, Quaternion.identity);
        
        _isEndBattle = true;
        _fadeCounter = fadeOutTime;
        
        anim.SetTrigger(TagManager.BOSS_VANISH_TRIGGER);
        
        theboss.GetComponent<Collider2D>().enabled = false;
        
        BossBullet[] bullets = FindObjectsOfType<BossBullet>();
        if (bullets.Length>0)
        {
            foreach (var bb in bullets)
            {
                Destroy(bb.gameObject);
            }
        }
    }
}
