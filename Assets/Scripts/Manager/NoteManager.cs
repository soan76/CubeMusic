using UnityEngine;

public class NoteManager : MonoBehaviour
{

    public int bpm = 0; 
    double currentTime = 0f;  

    // Note가 생성될 위치
    [SerializeField] Transform tfNoteAppear = null; 
    // Note Prefab을 담을 변수 
    [SerializeField] GameObject goNote = null;

    TimingManager theTimingManager;
    EffectManager theEffectManager;

    void Start()
    {
        theEffectManager = FindAnyObjectByType<EffectManager>();
        theTimingManager = FindAnyObjectByType<TimingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        // 60s / bpm = 1비트 시간 
        // 60 / 120 = 1비트당 0.5초
        if(currentTime >= 60d / bpm)
        {
            // 0.5초마다 Note 생성
            GameObject t_note = Instantiate(goNote, tfNoteAppear.position, Quaternion.identity, tfNoteAppear);
            theTimingManager.boxNoteList.Add(t_note);
            //t_note.transform.SetParent(this.transform);
            // currentTime에 deltatime을 더하면 0.5100..의 시간오차가 생김
            // 0으로 초기화하면 안됨
            // 오차까지 감안해서 빼줘야 함
            // 0.5100xx - 0.5
            currentTime -= 60d / bpm;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            if(collision.GetComponent<Note>().GetNoteFlag())
                theEffectManager.JudgementEffect(4);
            
            theTimingManager.boxNoteList.Remove(collision.gameObject);
            Destroy(collision.gameObject);
            
        }
    }
}
