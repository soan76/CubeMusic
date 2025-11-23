using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 게임이 시작되면 UI 활성화
    [SerializeField] GameObject[] goGameUi = null;
    [SerializeField] GameObject goTitleUI = null;
 
    public static GameManager instance;

    public bool isStartGame = false;

    ComboManager theCombo;
    ScoreManager theScore;
    TimingManager thetiming;
    StatusManager theStatus;
    PlayerController thePlayer;
    StageManager theStage;
    NoteManager theNote;
    // CenterFlame은 처음부터 비활성화된 객체는 FindAnyObjectByType로 찾을 수 없음
    [SerializeField] CenterFlame theMusic = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        theNote = FindAnyObjectByType<NoteManager>();
        theStage = FindAnyObjectByType<StageManager>();
        theCombo = FindAnyObjectByType<ComboManager>();
        theScore = FindAnyObjectByType<ScoreManager>();
        thetiming = FindAnyObjectByType<TimingManager>();
        theStatus = FindAnyObjectByType<StatusManager>();
        thePlayer = FindAnyObjectByType<PlayerController>();
    }

    public void GameStart(int p_songNum, int p_bpm)
    {
        for(int i = 0; i < goGameUi.Length; i++)
        {
            goGameUi[i].gameObject.SetActive(true);
        }
        theMusic.bgmName = "BGM" + p_songNum;
        theNote.bpm = p_bpm;
        theStage.RemoveStage();
        theCombo.ResetCombo();
        theStage.SettingStage();
        theScore.Initialized();
        thetiming.Initialized();
        theStatus.Initialized();
        thePlayer.Initialized();

        AudioManager.instance.StopBGM();

        isStartGame = true;
    }

    public void MainMenu()
    {
        // 게임이 종료되면 UI 비활성화
        for(int i = 0; i < goGameUi.Length; i++)
        {
            goGameUi[i].gameObject.SetActive(false);
        }

        goTitleUI.SetActive(true);
    }
}
