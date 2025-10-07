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

    Animator myAnim;
    string animScoreUp = "ScoreUp"; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAnim = GetComponent<Animator>(); 
        currentScore = 0;
        txtScore.text = "0";
    }

    public void IncreaseScore(int p_JudgementState)
    {
        int t_increaseScore = increaseScore;

        // 가중치 계산
        t_increaseScore = (int)(t_increaseScore * weight[p_JudgementState]);

        currentScore += t_increaseScore;
        txtScore.text = string.Format("{0:#,##0}", currentScore);

        if (myAnim != null)
        {
            myAnim.ResetTrigger(animScoreUp); // 혹시 이전 프레임 잔여 방지
            myAnim.SetTrigger(animScoreUp);
        }
        else
        {
            Debug.LogWarning("⚠️ ScoreManager: Animator가 연결되지 않았습니다. Text (TMP)에 Animator가 붙어 있는지 확인하세요.");
        }
    }
}
