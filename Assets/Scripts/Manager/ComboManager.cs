using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] GameObject goComboImage = null;
    [SerializeField] TMP_Text txtCombo = null;

    int currentCombo = 0;

    Animator txtAnim;
    Animator imgAnim;
    string animComboUp = "ComboUp";

    void Start()
    {
        txtAnim = GetComponent<Animator>(); 
        imgAnim = GetComponent<Animator>();
        // 처음엔 콤보 UI 비활성화
        txtCombo.gameObject.SetActive(false);
        goComboImage.SetActive(false);
    }

    public void IncreaseCombo(int p_num = 1)
    {
        // 증가할 콤보수
        currentCombo += p_num;
        txtCombo.text = string.Format("{0:#,##0}", currentCombo);

        if (currentCombo > 2)
        {
            txtCombo.gameObject.SetActive(true);
            goComboImage.SetActive(true);

            txtAnim.SetTrigger(animComboUp);
            imgAnim.SetTrigger(animComboUp);
        }
    }

    public int GetcurrentCombo()
    {
        return currentCombo;
    }

    public void ResetCombo()
    {
        // 리셋 콤보량
        currentCombo = 0;
        txtCombo.text = "0";
        txtCombo.gameObject.SetActive(false);
        goComboImage.SetActive(false);
    }
}
