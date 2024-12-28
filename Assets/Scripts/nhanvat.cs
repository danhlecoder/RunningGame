using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_run : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1; // 0:left 1:middle 2:right
    public float laneDistance = 4; // khoảng cách giữa các làn

    public float jumpForce;
    public float Gravity = -20;

    public Animator animator;

    private bool isGrounded; // kiểm tra trạng thái grounded

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!player_manager.isGameStarted)
            return;

        // Tăng tốc độ tiến lên
        if (forwardSpeed < maxSpeed)
            forwardSpeed += 0.2f * Time.deltaTime;

        animator.SetBool("isGameStarted", true);
        direction.z = forwardSpeed;

        // Kiểm tra trạng thái grounded
        if (controller.isGrounded)
        {
            if (!isGrounded)
            {
                isGrounded = true;
                animator.SetBool("isGrounder", false);
            }

            direction.y = -1;

            if (SwipeManager.swipeUp)
            {
                Jump();
            }

            if (SwipeManager.swipeDown)
            {
                StartCoroutine(Slide());
            }
        }
        else
        {
            isGrounded = false;
            direction.y += Gravity * Time.deltaTime;

            if (SwipeManager.swipeDown)
            {
                StartCoroutine(Slide());
            }
        }

        HandleLaneMovement();
        MoveCharacter();
    }

    private void FixedUpdate()
    {
        if (!player_manager.isGameStarted)
            return;

        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
        isGrounded = false;
        animator.SetBool("isGrounder", true);
    }

    private IEnumerator Slide()
    {
        animator.SetBool("isSliding", true);
        controller.height = 0.5f;
        controller.center = new Vector3(0, 0.25f, 0);

        yield return new WaitForSeconds(1.3f);

        controller.height = 2.0f;
        controller.center = new Vector3(0, 1.0f, 0);
        animator.SetBool("isSliding", false);
    }

    private void HandleLaneMovement()
    {
        if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;

            StartCoroutine(SetIsRight());
        }

        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;

            StartCoroutine(SetIsLeft());
        }
    }

    private void MoveCharacter()
    {
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        if (transform.position == targetPosition)
            return;

        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;

        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
    }

    private IEnumerator SetIsRight()
    {
        animator.SetBool("isRight", true); // Đặt trạng thái vuốt phải thành true
        yield return new WaitForSeconds(0.2f); // Giữ trạng thái trong 0.2 giây
        animator.SetBool("isRight", false); // Đặt lại trạng thái vuốt phải thành false
    }

    private IEnumerator SetIsLeft()
    {
        animator.SetBool("isLeft", true); // Đặt trạng thái vuốt trái thành true
        yield return new WaitForSeconds(0.2f); // Giữ trạng thái trong 0.2 giây
        animator.SetBool("isLeft", false); // Đặt lại trạng thái vuốt trái thành false
    }

private void OnControllerColliderHit(ControllerColliderHit hit)
{
    if (!player_manager.isGameStarted) return;

    if (hit.gameObject.CompareTag("Obstacle"))
    {
        player_manager.gameOver = true;
        FindObjectOfType<audioManager>().PlaySound("GameOver");
    }
}
}
