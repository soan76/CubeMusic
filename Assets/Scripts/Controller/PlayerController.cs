using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static bool s_canPresskey = true;

    // 이동
    [SerializeField] float moveSpeed = 3;
    private Vector3 dir;
    public Vector3 destPos;
    private bool isMoving = false;

    // 회전
    [SerializeField] float spinSpeed = 270;
    Vector3 rotDir = new Vector3();
    Quaternion destRot = new Quaternion();

    // 반동
    [SerializeField] float recoilPosY = 0.25f;
    [SerializeField] float recoilSpeed = 1.5f;

    bool canMove = true;

    // 기타
    [SerializeField] Transform fakeCube = null;
    [SerializeField] Transform realCube = null;

    TimingManager theTimingManager;
    CameraController theCam;

    void Start()
    {
        theTimingManager = FindAnyObjectByType<TimingManager>();
        theCam = FindAnyObjectByType<CameraController>();
    }

    void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame ||
            Keyboard.current.sKey.wasPressedThisFrame ||
            Keyboard.current.dKey.wasPressedThisFrame ||
            Keyboard.current.wKey.wasPressedThisFrame)
        {
            if (canMove && s_canPresskey)
            {
                //타이밍을 체크하기 전에 미리
                Calc();

                //판정 체크
                if (theTimingManager.CheckTiming())
                {
                    StartAction();
                }
            }
        }
    }

    void Calc()
    {
        // 방향계산
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.wKey.wasPressedThisFrame) vertical = 1f;
        if (Keyboard.current.sKey.wasPressedThisFrame) vertical = -1f;
        if (Keyboard.current.aKey.wasPressedThisFrame) horizontal = -1f;
        if (Keyboard.current.dKey.wasPressedThisFrame) horizontal = 1f;

        // 방향계산
        dir = new Vector3(horizontal, 0f, vertical);

        // ✅ 중복 키 입력 방지
        if (dir.x != 0f && dir.z != 0f)
        {
            // 최근 입력된 키 기준으로 하나만 유지
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
                dir.z = 0f;
            else
                dir.x = 0f;
        }

        if (dir == Vector3.zero) return;

        // 이동 목표값 계산
        destPos = transform.position + new Vector3(-dir.x, 0, dir.z);
        // 회전 목표값 계산
        if (dir.z > 0)          // W (앞)
            rotDir = Vector3.left;    // X축 음수
        else if (dir.z < 0)     // S (뒤)
            rotDir = Vector3.right;   // X축 양수
        else if (dir.x > 0)     // D (오른쪽)
            rotDir = Vector3.back;    // Z축 음수
        else if (dir.x < 0)     // A (왼쪽)
            rotDir = Vector3.forward; // Z축 양수


        // ✅ 회전 중심 수정 (Pivot 하단)
        Vector3 pivot = transform.position + Vector3.down * (realCube.localScale.y / 2f);

        // RotateAround = 공전 대상, 회전 축, 회전값)을 이용한 편법 회전 구현
        fakeCube.rotation = realCube.rotation; // 회전 초기화 (누적 방지)
        fakeCube.RotateAround(pivot, rotDir, spinSpeed);
        destRot = fakeCube.rotation;
    }

    void StartAction()
    {
        StartCoroutine(MoveGo());
        StartCoroutine(SpinCo());
        StartCoroutine(RecoilCo());
        StartCoroutine(theCam.ZoomCam());
    }
    IEnumerator MoveGo()
    {
        canMove = false;
        isMoving = true;

        // Vector3.SqrMagnitude = 제곱근을 리턴 ex: SqrMagnitude(4) = 2
        while (Vector3.SqrMagnitude(transform.position - destPos) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = destPos;
        isMoving = false;
        canMove = true;
    }

    IEnumerator SpinCo()
    {
        while (Quaternion.Angle(realCube.rotation, destRot) > 0.5f)
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation, destRot, spinSpeed * Time.deltaTime);
            yield return null;
        }

        realCube.rotation = destRot;
    }

    IEnumerator RecoilCo()
    {
        while (realCube.position.y < recoilPosY)
        {
            realCube.position += new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        while (realCube.position.y > 0)
        {
            realCube.position -= new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        realCube.localPosition = new Vector3(0, 0, 0);
    }
}
