using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] Animator noteHitAnimator = null;
    string hit = "Hit";

    [SerializeField] Animator judgementAnimator = null;
    [SerializeField] UnityEngine.UI.Image judgementImage = null;
    [SerializeField] Sprite[] judgementSprite = null;

    public void JudgementEffect(int p_num)
    {
        // 파라미터 값에 맞는 판정 이미지 스프라이트로 교체
        Debug.Log($"JudgementEffect 실행됨: {p_num}");
        if (p_num < judgementSprite.Length)
            judgementImage.sprite = judgementSprite[p_num];
        judgementAnimator.SetTrigger(hit);
    }

    public void NoteHitEffect()
    {
        // hit 파라미터를 넘겨줌 -> 애니메이션 재생
        noteHitAnimator.SetTrigger(hit);
    }
}
