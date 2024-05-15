using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform MainPlayer;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    
    [SerializeField] private Collider2D standingCollider;
    [SerializeField] private Collider2D crouchingCollider;

    private bool isGrounded = false;
    private bool isJumping = false;
    private bool canJump = false;

    private Animator animator;

    private enum PlayerState
    {
        Standing,
        Crouching
    }

    private PlayerState currentState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        currentState = PlayerState.Standing;
        standingCollider.enabled = true;
        crouchingCollider.enabled = false;
    }

    private void Update()
    {
       // Kiểm tra trạng thái animation và kích hoạt/ vô hiệu hóa collider tương ứng
        switch (currentState)
        {
            case PlayerState.Standing:
                standingCollider.enabled = true;
                crouchingCollider.enabled = false;
                break;
            case PlayerState.Crouching:
                standingCollider.enabled = false;
                crouchingCollider.enabled = true;
                break;
            default:
                break;
        }

        if (GameManager.Instance.isPlaying)
        {
            isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);

            // Kiểm tra xem có cử chỉ chạm nào không
            if (Input.touchCount > 0)
            {
                // Lấy cử chỉ chạm đầu tiên
                Touch touch = Input.GetTouch(0);

                // Kiểm tra nếu cử chỉ chạm này là cử chỉ chạm đầu tiên và nếu đang chạm vào màn hình
                if (touch.phase == TouchPhase.Began)
                {
                    float screenCenterX = Screen.width / 2f;
                    // Kiểm tra nếu chạm vào màn hình ở phía trên màn hình (có thể tùy chỉnh tùy theo trò chơi của bạn)
                    if (touch.position.x > screenCenterX)
                    {
                        // Kiểm tra nếu người chơi đang đứng trên mặt đất và có thể nhảy
                        if (isGrounded && canJump)
                        {
                            canJump = true;
                            isJumping = true;
                            rb.velocity = Vector2.up * jumpForce;   
                           
                        }
                    }
                    else
                    {
                        ChangeState(PlayerState.Crouching);
                       
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    // Nếu cử chỉ chạm kết thúc, kiểm tra xem nó có phải là cử chỉ chạm kết thúc nhảy không
                    if (!isGrounded && isJumping)
                    {
                        isJumping = false;
                    }
                    else
                    {
                        ChangeState(PlayerState.Standing);
                        
                    }
                }
            }

            if (isGrounded && !isJumping)
            {
                canJump = true; 
            }
        }
    }

    // Hàm để thay đổi trạng thái của nhân vật
    private void ChangeState(PlayerState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case PlayerState.Standing:
                animator.SetBool("crouch", false);
                break;
            case PlayerState.Crouching:
                animator.SetBool("crouch", true);
                break;
            default:
                break;
        }
    }



}