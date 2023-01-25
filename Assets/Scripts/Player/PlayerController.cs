using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   private Rigidbody2D _rb;
   [SerializeField] private Animator playerAnim;

   //[Header("MoveParameters")]
   [SerializeField] private float moveSpeed = 8f, jumpForce = 20f;
   private float _xAxis;

   [SerializeField] private LayerMask whatIsGround;
   [SerializeField] private Transform groundCheckPosition;
   [SerializeField] private float distanceToGroundCheck = 0.1f;
   private bool _isGrounded;
   private bool _canDoubleJump;
   
   private Vector3 _tempScale;
   
   
   [SerializeField] private BulletController bulletToFire;
   [SerializeField] private Transform shootPoint;
   [SerializeField] private float timeBetweenShoots = 0.25f;
   private float _timerBetweenShoots;

   [SerializeField] private float dashSpeed=25f, dashTime=0.2f;
   private float _dashCounter;
   [SerializeField] private float timeToRepeatDash = 3f;
   private float _repeatDash;

   [SerializeField] private SpriteRenderer mySr, afterDashImage;
   [SerializeField] private float afterImageLifeTime = 0.5f;
   [SerializeField] private Color afterImageColor;
   private SpriteRenderer _instantiatedImage;
   [SerializeField] private int countOfCopyDashImages = 5;
   
   [SerializeField] private GameObject standingPlayer,ballPlayer;
   [SerializeField] private float waitToBall=0.5f;
   private float _ballCounter;
   [SerializeField] private Animator ballAnim;

   [SerializeField] private GameObject changePlayerFx;
   [SerializeField] private Transform pointToSpawnChangePlayerFx;

   [SerializeField] private GameObject bombToPlace;
   [SerializeField] private Transform positionToPlaceBomb;

   [SerializeField] private float distanceToWallCheck = 2f;

   private PlayerAbilityTracker _abilities;

   public bool canMove;
   
   private void Awake()
   {
      _rb = GetComponent<Rigidbody2D>();
   }

   private void Start()
   {
      _abilities = GetComponent<PlayerAbilityTracker>();
      canMove = true;
   }

   private void Update()
   {
      MovePlayer();

      CheckIsGrounded();
      
      AnimatePlayer();
   }

   private void CheckIsGrounded()
   {
       _isGrounded = Physics2D.Raycast
         (groundCheckPosition.position, Vector2.down, distanceToGroundCheck, whatIsGround);
   }

   private void MovePlayer()
   {
      if (canMove)
      {
         ControllMovement();
      }
      else
      {
         _rb.velocity = Vector2.zero;
      }
   }
   
   private void ControllMovement()
   {
      if (Input.GetMouseButtonDown(1) && standingPlayer.activeSelf && _abilities.canDash)
      {
         if (Time.time>_repeatDash)
         {
            _repeatDash = Time.time + timeToRepeatDash;
            _dashCounter = dashTime;
            
            AudioManager.Instance.PlaySfxAdjusted(7);
            
            ShowAfterImage();
         }
      }

      if (_dashCounter>0)
      {
         _dashCounter -= Time.deltaTime;
         _rb.velocity = new Vector2(dashSpeed*transform.localScale.x, _rb.velocity.y);
      }
      else
      {
         _xAxis = Input.GetAxisRaw(TagManager.HORIZONTAL_AXIS);
         _rb.velocity = new Vector2(_xAxis * moveSpeed, _rb.velocity.y);
      
         FlipPlayer();
      }
      
      if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
      {
         Jump();
         _canDoubleJump = true;
         
         AudioManager.Instance.PlaySfxAdjusted(12);
         
      }else if (Input.GetKeyDown(KeyCode.Space) && _canDoubleJump && _abilities.canDoubleJump)
      {
         Jump();
         _canDoubleJump = false;
         
         playerAnim.SetTrigger(TagManager.PLAYER_DOUBLE_JUMP_TRIGGER);
         
         AudioManager.Instance.PlaySfxAdjusted(9);
      }

      if (Input.GetMouseButtonDown(0))
      {
         if (Time.time>_timerBetweenShoots)
         {
            Shoot();
         }
      }
      
      //ballMode
      if (!ballPlayer.activeSelf)
      {
         if (Input.GetAxisRaw(TagManager.VERTICAL_AXIS)<-0.9f && _abilities.canBecomeBall)
         {
            _ballCounter -= Time.deltaTime;
            if (_ballCounter<=0)
            {
               ballPlayer.SetActive(true);
               standingPlayer.SetActive(false);
               
               AudioManager.Instance.PlaySFX(6);
               
               ShowChangePlayerModelFX();
            }
         }
         else
         {
            _ballCounter = waitToBall;
         }
      }
      else
      {
         if (Input.GetAxisRaw(TagManager.VERTICAL_AXIS)>0.9f)
         {
            _ballCounter -= Time.deltaTime;
            if (_ballCounter<=0)
            {
               if (!Physics2D.Raycast(groundCheckPosition.position,
                      Vector2.up,distanceToWallCheck,whatIsGround))
               {
                  ballPlayer.SetActive(false);
                  standingPlayer.SetActive(true);
                  
                  AudioManager.Instance.PlaySFX(10);

                  ShowChangePlayerModelFX();
               }
            }
         }
         else
         {
            _ballCounter = waitToBall;
         }
      }
   }

   private void ShowChangePlayerModelFX()
   {
      Instantiate(changePlayerFx, pointToSpawnChangePlayerFx.position, Quaternion.identity);
   }

   private void ShowAfterImage()
   {
      StartCoroutine(nameof(_ShowAfterImageCo));
   }

   private IEnumerator _ShowAfterImageCo()
   {
      for (int i = 0; i < countOfCopyDashImages; i++)
      {
         _instantiatedImage = Instantiate(afterDashImage, transform.position, transform.rotation);
         _instantiatedImage.sprite = mySr.sprite;
         _instantiatedImage.transform.localScale = transform.localScale;
         _instantiatedImage.color = afterImageColor;
         
         Destroy(_instantiatedImage.gameObject,afterImageLifeTime);

         yield return new WaitForSeconds(0.05f);
      }
   }

   private void Shoot()
   {
      if (standingPlayer.activeSelf && _abilities.canShoot)
      {
         Instantiate(bulletToFire, shootPoint.position, Quaternion.identity).moveDirection=
            new Vector2(transform.localScale.x,0f);
    
         playerAnim.SetTrigger(TagManager.PLAYER_SHOOT_TRIGGER);
         
         AudioManager.Instance.PlaySfxAdjusted(14);
      }
      else if (ballPlayer.activeSelf && _abilities.canDropBomb)
      {
         Instantiate(bombToPlace, positionToPlaceBomb.position, Quaternion.identity);
         
         AudioManager.Instance.PlaySfxAdjusted(13);
      }
      _timerBetweenShoots = Time.time + timeBetweenShoots;
   }

   private void Jump()
   {
      _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
   }

   private void FlipPlayer()
   {
      _tempScale = transform.localScale;
      
      if (_xAxis>0)
      {
         _tempScale.x = 1f;
      }else if (_xAxis<0)
      {
         _tempScale.x = -1f;
      }

      transform.localScale = _tempScale;
   }

   private void AnimatePlayer()
   {
      if (standingPlayer.activeSelf)
      {
         playerAnim.SetFloat(TagManager.PLAYER_MOVE_ANIMATION_PARAMETR,Math.Abs(_rb.velocity.x));
         playerAnim.SetBool(TagManager.PLAYER_JUMP_ANIMATION_PARAMETR,_isGrounded);
      }
      
      if (ballPlayer.activeSelf)
      {
         ballAnim.SetFloat(TagManager.PLAYER_MOVE_ANIMATION_PARAMETR,Math.Abs(_rb.velocity.x));
      }
   }

   public void StopPlayerAnimator(bool needToStop)
   {
      playerAnim.enabled = !needToStop;
   }
}
