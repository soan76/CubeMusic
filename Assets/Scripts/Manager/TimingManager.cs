using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    // 생성된 노트를 담는 리스트 -> 판정범위에 있는지 확인해야하기 때문
    public List<GameObject> boxNoteList = new List<GameObject>();
    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null;
    // TimingBox => 판정 범위의 최소값(x) 최대값(y)
    Vector2[] timingBoxs = null;

    EffectManager theEffect;
    ScoreManager theScoreManager;

    void Start()
    {
        theEffect = FindAnyObjectByType<EffectManager>();
        theScoreManager = FindAnyObjectByType<ScoreManager>();

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

    public void CheckTiming()
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

                    theEffect.JudgementEffect(x);

                    //점수증가
                    theScoreManager.IncreaseScore(x);
                    return;
                }
            }
        }
        theEffect.JudgementEffect(timingBoxs.Length - 1);
    }
}
