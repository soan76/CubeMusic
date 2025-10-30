using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static bool s_canPresskey = true;

    [Header("이동 속도")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] CubeRotator cubeRotator; // 🔹 회전 담당 스크립트 연결

    private Vector3 moveDir;
    public Vector3 destPos;
    private bool canMove = true;

    private TimingManager theTimingManager;
    private CameraController theCam;

    void Start()
    {
        theTimingManager = FindAnyObjectByType<TimingManager>();
        theCam = FindAnyObjectByType<CameraController>();
    }

    void Update()
    {
        if (!canMove) return;

        if (Keyboard.current.aKey.wasPressedThisFrame ||
            Keyboard.current.sKey.wasPressedThisFrame ||
            Keyboard.current.dKey.wasPressedThisFrame ||
            Keyboard.current.wKey.wasPressedThisFrame)
        {
            if (s_canPresskey && canMove)
            {
                Calc();

                if (theTimingManager.CheckTiming())
                {
                    StartAction();
                }
            }
        }
    }

    void Calc()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.wKey.wasPressedThisFrame) vertical = 1f;
        if (Keyboard.current.sKey.wasPressedThisFrame) vertical = -1f;
        if (Keyboard.current.aKey.wasPressedThisFrame) horizontal = -1f;
        if (Keyboard.current.dKey.wasPressedThisFrame) horizontal = 1f;

        moveDir = new Vector3(horizontal, 0f, vertical);

        // 대각 입력 방지
        if (moveDir.x != 0f && moveDir.z != 0f)
        {
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
                moveDir.z = 0f;
            else
                moveDir.x = 0f;
        }

        if (moveDir == Vector3.zero) return;

        // 이동 목표 설정
        destPos = transform.position + new Vector3(-moveDir.x, 0, moveDir.z);
    }

    void StartAction()
    {
        StartCoroutine(MoveCo());
        cubeRotator.StartRolling(moveDir); // 🎯 회전 요청
        StartCoroutine(theCam.ZoomCam());
    }

    IEnumerator MoveCo()
    {
        canMove = false;

        while (Vector3.SqrMagnitude(transform.position - destPos) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = destPos;
        canMove = true;
    }
}
