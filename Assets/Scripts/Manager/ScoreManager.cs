using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text txtScore = null;
    // 점수 증가량
    [SerializeField] int increaseScore = 10;
    int currentScore = 0;

    // 판정에 따른 가중치
    [SerializeField] float[] weight = null;

    [SerializeField] int comboBonusScore = 10;

    Animator myAnim;
    string animScoreUp = "ScoreUp"; 
    ComboManager theCombo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        theCombo = FindAnyObjectByType<ComboManager>();
        myAnim = GetComponent<Animator>(); 
        currentScore = 0;
        txtScore.text = "0";
    }

    public void IncreaseScore(int p_JudgementState)
    {
        // 콤보 증가
        theCombo.IncreaseCombo();

        // 콤보 보너스 점수 계산
        int t_currentCombo = theCombo.GetcurrentCombo();
        int t_bonusComboScore = (t_currentCombo / 10) * comboBonusScore;

        // 판정 가중치 계산
        int t_increaseScore = increaseScore + t_bonusComboScore;
        t_increaseScore = (int)(t_increaseScore * weight[p_JudgementState]);

        // 점수 반영
        currentScore += t_increaseScore;
        txtScore.text = string.Format("{0:#,##0}", currentScore);

        // 애니메이션 재생
        if (myAnim != null)
        {
            myAnim.ResetTrigger(animScoreUp);
            myAnim.SetTrigger(animScoreUp);
        }
        else
        {
            Debug.LogWarning("⚠️ ScoreManager: Animator가 연결되지 않았습니다. Text (TMP)에 Animator가 붙어 있는지 확인하세요.");
        }
    }
}
