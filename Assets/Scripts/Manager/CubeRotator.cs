using System.Collections;
using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    [Header("회전 설정")]
    [SerializeField] private float spinSpeed = 270f;

    [Header("반동 설정")]
    [SerializeField] private float recoilHeight = 0.25f;
    [SerializeField] private float recoilSpeed = 2f;

    [Header("큐브 참조")]
    [SerializeField] private Transform fakeCube;
    [SerializeField] private Transform realCube;

    private bool isRolling = false;
    private Vector3 rotAxis;
    private Quaternion destRot;

    public void StartRolling(Vector3 moveDir)
    {
        if (isRolling || moveDir == Vector3.zero) return;
        StartCoroutine(RollRoutine(moveDir));
    }

    IEnumerator RollRoutine(Vector3 moveDir)
    {
        isRolling = true;

        float half = transform.localScale.y / 2f;

        // 방향별 회전축
        if (moveDir.z > 0) rotAxis = Vector3.right; // W (앞)
        else if (moveDir.z < 0) rotAxis = Vector3.left;   // S (뒤)
        else if (moveDir.x > 0) rotAxis = Vector3.forward; // D (오른쪽) 
        else if (moveDir.x < 0) rotAxis = Vector3.back;    // A (왼쪽)   

        // 피벗 계산 (정확히 이동 방향 하단 모서리 중심)
        Vector3 pivot = transform.parent.position + ((-Vector3.up + moveDir) * half);

        // 목표 회전값 계산
        fakeCube.rotation = realCube.rotation;
        fakeCube.RotateAround(pivot, rotAxis, 90f);
        destRot = fakeCube.rotation;

        float currentAngle = 0f;
        const float targetAngle = 90f;

        while (currentAngle < targetAngle)
        {
            float step = spinSpeed * Time.deltaTime;
            if (currentAngle + step > targetAngle)
                step = targetAngle - currentAngle;

            // 큐브 회전
            realCube.RotateAround(pivot, rotAxis, step);

            // 부드러운 반동
            float bounce = Mathf.Sin((currentAngle / targetAngle) * Mathf.PI) * recoilHeight;
            realCube.localPosition = new Vector3(0, bounce, 0);

            currentAngle += step;
            yield return null;
        }

        // 보정
        realCube.rotation = destRot;
        realCube.localPosition = Vector3.zero;

        isRolling = false;
    }

    public void ResetRealCube()
    {
        realCube.localPosition = Vector3.zero;
    }
}
