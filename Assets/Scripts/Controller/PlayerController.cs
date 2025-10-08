using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // 이동
    [SerializeField] float moveSpeed = 3;

    private Vector3 dir;
    private Vector3 destPos;
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

    void Start()
    {
        theTimingManager = FindAnyObjectByType<TimingManager>();
    }

    void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame ||
            Keyboard.current.sKey.wasPressedThisFrame ||
            Keyboard.current.dKey.wasPressedThisFrame ||
            Keyboard.current.wKey.wasPressedThisFrame)
        {
            if (canMove)
            {
                //판정 체크
                if (theTimingManager.CheckTiming())
                {
                    StartAction();
                }
            }
        }
    }

    void StartAction()
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
        // 이동 목표값 계산
        destPos = transform.position + new Vector3(-dir.x, 0, dir.z);
        // 회전 목표값 계산
        rotDir = new Vector3(-dir.z, 0f, -dir.x);
        // RotateAround = 공전 대상, 회전 축, 회전값)을 이용한 편법 회전 구현
        fakeCube.RotateAround(transform.position, rotDir, spinSpeed);
        destRot = fakeCube.rotation;

        StartCoroutine(MoveGo());
        StartCoroutine(SpinCo());
        StartCoroutine(RecoilCo());
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
