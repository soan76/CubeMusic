using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static bool s_canPresskey = true;

    [Header("이동 속도")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] CubeRotator cubeRotator; // 회전 담당 스크립트 연결

    // 낭떠러지로 추락했을때 플레이어를 원위치로 돌아오게 함
    Vector3 originPos = new Vector3();

    private Vector3 moveDir;
    public Vector3 destPos;
    private bool canMove = true;
    bool isFalling = false;

    private TimingManager theTimingManager;
    private CameraController theCam;
    // 추락에 필요한 중력
    Rigidbody myRigid;
    StatusManager theStatus;

    void Start()
    {
        theTimingManager = FindAnyObjectByType<TimingManager>();
        theStatus = FindAnyObjectByType<StatusManager>();
        theCam = FindAnyObjectByType<CameraController>();
        // GetComponentInChildren = 자식 객체중에 특정 컴포넌트가 있다면 가져옴
        myRigid = GetComponentInChildren<Rigidbody>();
        originPos = transform.position;
    }
    public void Initialized()
    {
        transform.position = Vector3.zero;
        destPos = Vector3.zero;
        cubeRotator.ResetRealCube();
        canMove = true;
        s_canPresskey = true;
        isFalling = false;
        myRigid.useGravity = false;
        myRigid.isKinematic = true;
    }
    void Update()
    {
        if(GameManager.instance.isStartGame)
        {
            CheckFalling();

            if (!canMove) return;

            if (Keyboard.current.aKey.wasPressedThisFrame ||
                Keyboard.current.sKey.wasPressedThisFrame ||
                Keyboard.current.dKey.wasPressedThisFrame ||
                Keyboard.current.wKey.wasPressedThisFrame)
            {
                if (s_canPresskey && canMove && !isFalling)
                {
                    Calc();

                    if (theTimingManager.CheckTiming())
                    {
                        StartAction();
                    }
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
        cubeRotator.StartRolling(moveDir); // 회전 요청
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

    // 레이저를 쏴서 바닥 오브젝트가 있는지 없는지 확인
    // 자기자신의 위치에서 밑으로 레이저를 쏨
    void CheckFalling()
    {
        if(!isFalling && canMove)
        {
            if (!Physics.Raycast(transform.position, Vector3.down, 1.1f))
            {
                Falling();
            }
        }
    }

    void Falling()
    {
        isFalling = true;
        myRigid.useGravity = true;
        myRigid.isKinematic = false;
    }
    
    public void ResetFalling()
    {
        // 체력이 1씩 감소
        theStatus.DecreaseHp(1);
        AudioManager.instance.PlaySFX("Falling");

        // 플레이어가 죽으면 추락으로 인한 위치 조정 막기
        if(!theStatus.IsDead())
        {
            isFalling = false;  
            myRigid.useGravity = false;
            myRigid.isKinematic = true;

            transform.position = originPos;
            cubeRotator.ResetRealCube();
        }
        
    }
}
