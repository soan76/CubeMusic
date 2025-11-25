using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject goUI = null;
    [SerializeField] TextMeshProUGUI[] txtCount = null;
    [SerializeField] TextMeshProUGUI txtCoin = null;
    [SerializeField] TextMeshProUGUI txtScore = null;
    [SerializeField] TextMeshProUGUI txtMaxCombo = null;

    int currentSong = 0;
    public void SetCurrentSong(int p_songNum)
    {
        currentSong = p_songNum;
    }

    ScoreManager theScore;
    ComboManager theCombo;
    TimingManager theTiming;
    DataManager theDatabase;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        theScore = FindAnyObjectByType<ScoreManager>();
        theCombo = FindAnyObjectByType<ComboManager>();
        theTiming = FindAnyObjectByType<TimingManager>();
        theDatabase = FindAnyObjectByType<DataManager>();
    }

    public void ShowResult()
    {
        FindAnyObjectByType<CenterFlame>().ResetMusic();

        AudioManager.instance.StopBGM();

        goUI.SetActive(true);

        for (int i = 0; i < txtCount.Length; i++)
            txtCount[i].text = "0";

        txtCoin.text = "0";
        txtScore.text = "0";
        txtMaxCombo.text = "0";

        int[] t_judgement = theTiming.GetJudgementRecord();
        int t_currentScore = theScore.GetCurrentScore();
        int t_maxCombo = theCombo.GetMaxCombo();
        int t_coin = t_currentScore / 50;

        for (int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = string.Format("{0:#,##0}", t_judgement[i]);
        }

        txtScore.text = string.Format("{0:#,##0}", t_currentScore);
        txtMaxCombo.text = string.Format("{0:#,##0}", t_maxCombo);
        txtCoin.text = string.Format("{0:#,##0}", t_coin);

        if(t_currentScore > theDatabase.score[currentSong])
        {
            theDatabase.score[currentSong] = t_currentScore;
            theDatabase.SaveScore();
        }
    }

    public void BtnRetry()
    {
        goUI.SetActive(false);
        GameManager.instance.RetryGame();
    }

    public void BtnMainMenu()
    {
        goUI.SetActive(false);
        // 게임 매니저의 메인 메뉴 함수 호출
        GameManager.instance.MainMenu();
    }
}
