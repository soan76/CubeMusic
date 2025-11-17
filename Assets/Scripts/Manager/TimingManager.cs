using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    // 생성된 노트를 담는 리스트 -> 판정범위에 있는지 확인해야하기 때문
    public List<GameObject> boxNoteList = new List<GameObject>();

    int[] judgementRecord = new int[5];

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null;
    // TimingBox => 판정 범위의 최소값(x) 최대값(y)
    Vector2[] timingBoxs = null;

    EffectManager theEffect;
    ScoreManager theScoreManager;
    ComboManager theComboManager;
    StageManager theStageManager;
    PlayerController thePlayer;
    StatusManager theStatusManager;
    AudioManager theAudioManager;

    void Start()
    {
        theAudioManager = AudioManager.instance;
        theEffect = FindAnyObjectByType<EffectManager>();
        theScoreManager = FindAnyObjectByType<ScoreManager>();
        theComboManager = FindAnyObjectByType<ComboManager>();
        theStageManager = FindAnyObjectByType<StageManager>();
        thePlayer = FindAnyObjectByType<PlayerController>();
        theStatusManager = FindAnyObjectByType<StatusManager>();

        // 타이밍 박스 설정
        // 0번째 타이밍 박스 = Perfect (가장 좁은 판정)
        // 3번째 타이밍 박스 = Bad (가장 넓은 판정)
        timingBoxs = new Vector2[timingRect.Length];

        for (int i = 0; i < timingRect.Length; i++)
        {
            Vector3[] corners = new Vector3[4];
            timingRect[i].GetWorldCorners(corners);

            // 각각의 판정범위
            // Center의 위치를 기준으로 timingRect의 절반 너비만큼 빼고 더해서 판정범위 설정
            timingBoxs[i].Set(corners[0].x, corners[2].x);
        }
    }

    public bool CheckTiming()
    {
        for (int i = 0; i < boxNoteList.Count; i++)
        {

            float t_notePosX = boxNoteList[i].transform.position.x;

            for (int x = 0; x < timingBoxs.Length; x++)
            {
                if (timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
                {
                    // 노트 제거
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);

                    // 이펙트 연출
                    // 0: Perfect, 1: Great, 2: Good, 3: Bad
                    // Hit 이펙트는 Perfect, Great, Good 일 때만 재생
                    if (x < timingBoxs.Length - 1)
                        theEffect.NoteHitEffect();


                    //다음 스테이지 바닥 호출
                    if (CheckCanNextPlate())
                    {
                        //점수증가
                        theScoreManager.IncreaseScore(x);
                        // 판때기 등장
                        theStageManager.ShowNextPalte();
                        theEffect.JudgementEffect(x); // 판정 연출
                        judgementRecord[x]++; // 판정 기록
                        theStatusManager.CheckShield(); // 쉴드 체크
                    }
                    else
                    {
                        theEffect.JudgementEffect(5);
                    }

                    StartCoroutine(WaitAndCheckNextPlate());
                    theAudioManager.PlaySFX("Clap");

                    return true;
                }
            }
        }
        theComboManager.ResetCombo();
        theEffect.JudgementEffect(timingBoxs.Length - 1);
        MissRecord();

        return false;
    }
    
    private IEnumerator WaitAndCheckNextPlate()
    {
        // 이동 코루틴(MoveGo/SpinCo)이 끝날 시간을 살짝 기다림
        yield return new WaitForSeconds(0.15f);

        // Raycast를 Collider 밖에서 쏘도록 살짝 위로 올림
        if (Physics.Raycast(thePlayer.destPos + Vector3.up * 0.5f, Vector3.down, out RaycastHit t_hitInfo, 1.1f))
        {
            if (t_hitInfo.transform.CompareTag("BasicPlate"))
            {
                BasicPlate t_plate = t_hitInfo.transform.GetComponent<BasicPlate>();
                if (t_plate.flag)
                {
                    t_plate.flag = false;
                    theStageManager.ShowNextPalte();
                }
            }
        }
    }

    bool CheckCanNextPlate()
    {
        // Physics.Raycast() = 가상의 광선을 쏴서 맞은 대상의 정보를 가져오는 함수
        // 광선위치, 방향, 충돌정보, 길이
        // 플레이어의 위치에서 아래방향으로 레이저를 쏘고 레이저에 부딪힌 객체의 정보를 획득, 길이
        if (Physics.Raycast(thePlayer.destPos, Vector3.down, out RaycastHit t_hitInfo, 1.1f))
        {
            if (t_hitInfo.transform.CompareTag("BasicPlate"))
            {
                BasicPlate t_plate = t_hitInfo.transform.GetComponent<BasicPlate>();
                if (t_plate.flag)
                {
                    t_plate.flag = false;
                    return true;
                }
            }
        }

        return false;
    }

    public int[] GetJudgementRecord()
    {
        return judgementRecord;
    }

    public void MissRecord()
    {
        judgementRecord[4]++;
        theStatusManager.ResetShieldCombo();
    }
    public void Initialized()
    {
        judgementRecord[0] = 0;
        judgementRecord[1] = 0;
        judgementRecord[2] = 0;
        judgementRecord[3] = 0;
        judgementRecord[4] = 0;
    }
}
