using UnityEditor.Analytics;
using UnityEngine;

public class NoteManager : MonoBehaviour
{

    public int bpm = 0; 
    double currentTime = 0f;

    bool noteActive = true;

    // Note가 생성될 위치
    [SerializeField] Transform tfNoteAppear = null; 

    TimingManager theTimingManager;
    EffectManager theEffectManager;
    ComboManager theComboManager;

    void Start()
    {
        theEffectManager = FindAnyObjectByType<EffectManager>();
        theTimingManager = FindAnyObjectByType<TimingManager>();
        theComboManager = FindAnyObjectByType<ComboManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(noteActive)
        {
            currentTime += Time.deltaTime;

            // 60s / bpm = 1비트 시간 
            // 60 / 120 = 1비트당 0.5초
            if(currentTime >= 60d / bpm)
            {
                GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
                t_note.transform.position = tfNoteAppear.position;
                t_note.SetActive(true);
                theTimingManager.boxNoteList.Add(t_note);
                //t_note.transform.SetParent(this.transform);
                // currentTime에 deltatime을 더하면 0.5100..의 시간오차가 생김
                // 0으로 초기화하면 안됨
                // 오차까지 감안해서 빼줘야 함
                // 0.5100xx - 0.5
                currentTime -= 60d / bpm;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            if (collision.GetComponent<Note>().GetNoteFlag())
            {
                theTimingManager.MissRecord();
                theEffectManager.JudgementEffect(4);
                theComboManager.ResetCombo();
            }


            theTimingManager.boxNoteList.Remove(collision.gameObject);
            ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);
            collision.gameObject.SetActive(false);

        }
    }
    
    public void RemoveNote()
    {
        noteActive = false;
        for(int i = 0; i < theTimingManager.boxNoteList.Count; i++)
        {
            theTimingManager.boxNoteList[i].SetActive(false);
            ObjectPool.instance.noteQueue.Enqueue(theTimingManager.boxNoteList[i]);
        }
    }
}
