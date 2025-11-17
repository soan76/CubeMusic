using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class StageManager : MonoBehaviour
{
    [SerializeField] GameObject stage = null;
    GameObject currentStage;
    // 스테이지의 바닥들
    Transform[] stagePlates;

    // 바닥 등장 위치 오프셋
    [SerializeField] float offsetY = 300f;
    [SerializeField] float plateSpeed = 10f;

    int stepCount = 0;
    // 총 바닥 개수
    int totalPlateCount = 0;
    public void RemoveStage()
    {
        if(currentStage!=null)
        {
            Destroy(currentStage);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SettingStage()
    {   
        stepCount = 0;

        currentStage = Instantiate(stage,Vector3.zero,Quaternion.identity);
        stagePlates = currentStage.GetComponent<Stage>().plates;
        totalPlateCount = stagePlates.Length;

        for(int i = 0; i < totalPlateCount; i++)
        {
            // 바닥 비활성화
            stagePlates[i].position = new Vector3(
            stagePlates[i].position.x,
            stagePlates[i].position.y - offsetY,
            stagePlates[i].position.z
             );

        }
    }

    // 다음 스테이지 바닥 보여주기
    // 플레이어 발걸음 수 기준
    public void ShowNextPalte()
    {
        // stepCount가 totalPlateCount를 넘지 않을 때만 활성화
        if (stepCount < totalPlateCount)
        {
            StartCoroutine(MovePlateCo(stepCount));
            stepCount++;
        }
    }
    
    IEnumerator MovePlateCo(int p_num)
    {
        stagePlates[p_num].gameObject.SetActive(true);
 
         Vector3 t_destPos = new Vector3(
            stagePlates[p_num].position.x,
             stagePlates[p_num].position.y + offsetY,
             stagePlates[p_num].position.z
        );

         while (Vector3.SqrMagnitude(stagePlates[p_num].position - t_destPos) >= 0.001f)
         {
             stagePlates[p_num].position = Vector3.Lerp(stagePlates[p_num].position, t_destPos, plateSpeed * Time.deltaTime);
             yield return null;
         }

         stagePlates[p_num].position = t_destPos;
;

    }
}
