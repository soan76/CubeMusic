using System.Collections;
using UnityEngine;

[ExecuteAlways]
public class CameraController : MonoBehaviour
{
    [SerializeField] Transform thePlayer = null;
    [SerializeField] float followSpeed = 15f;


    //카메라와 플레이어간의 거리차
    Vector3 playerDistance = new Vector3();

    float hitDistance = 0;
    [SerializeField] float zoomDistance = -1.25f;

    // 화면비율 고정 설정
    //[SerializeField] float targetAspect = 9f / 16f;

    //Camera cam;

    void Start()
    {
        //cam = GetComponent<Camera>();
        playerDistance = transform.position - thePlayer.position;

        //UpdateCameraAspect();
    }

    // Update is called once per frame
    void Update()
    {
        // 이동할 좌표값을 구한 뒤 카메라 이동
        Vector3 t_destPos = thePlayer.position + playerDistance + (transform.forward * hitDistance);
        // Vector3.Lerp(A,B,C) : 두 좌표 사이를 보간하여 부드럽게 이동 (A와 B사이의 값에서 C비율의 값을 추출)
        transform.position = Vector3.Lerp(transform.position, t_destPos, followSpeed * Time.deltaTime);

        //UpdateCameraAspect();
    }

    /* void UpdateCameraAspect()
    {
        if (cam == null) return;

        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            // 세로 화면에서 상하 블랙바
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            cam.rect = rect;
        }
        else
        {
            // 가로 화면에서 좌우 블랙바
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            cam.rect = rect;
        }
    } */

    public IEnumerator ZoomCam()
    {
        hitDistance = zoomDistance;

        yield return new WaitForSeconds(0.15f);

        hitDistance = 0;
    }
}
