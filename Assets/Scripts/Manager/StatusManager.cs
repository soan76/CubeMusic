using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    // 깜빡임 속도
    [SerializeField] float blinkSpeed = 0.1f;
    [SerializeField] int blinkCount = 10;
    int currentBlinkCount = 0;
    bool isBlink = false; 

    bool isDead = false;

    int maxHp = 3;
    int currentHp = 3;

    int maxSheild = 3;
    int currentSheild = 0;

    [SerializeField] GameObject[] hpImage = null;
    [SerializeField] GameObject[] sheildImage = null;

    [SerializeField] int shieldIncreaseCombo = 5;
    int currentSheildCombo = 0;
    [SerializeField] Image shieldGauge = null;

    Result theResult;
    NoteManager theNote;
    [SerializeField] MeshRenderer playerMesh = null;

    void Start()
    {
        theResult = FindAnyObjectByType<Result>();
        theNote = FindAnyObjectByType<NoteManager>();
    }

    public void CheckShield()
    {
        currentSheildCombo++;

        // 콤보가 쉴드 증가 콤보 수치에 도달했을때 쉴드 증가
        if (currentSheildCombo >= shieldIncreaseCombo)
        {
            IncreaseShield();
            currentSheildCombo = 0;
        }

        shieldGauge.fillAmount = (float)currentSheildCombo / shieldIncreaseCombo;
    }

    public void ResetShieldCombo()
    {
        currentSheildCombo = 0;
         shieldGauge.fillAmount = (float)currentSheildCombo / shieldIncreaseCombo;
    }

    public void IncreaseShield()
    {
        currentSheild++;

        if (currentSheild >= maxSheild)
            currentSheild = maxSheild;

        SettingShieldImage();
    }

    public void IncreaseHP(int p_num)
    {
        currentHp += p_num;
        if (currentHp >= maxHp)
            currentHp = maxHp;
        
        SettingHPImage();
    }

    public void DecreaseHp(int p_num)
    {
        // 깜빡임 효과가 진행중이 아닐때만 데미지를 입음
        if (!isBlink)
        {
            // 쉴드가 남아있다면 쉴드를 먼저 감소시킴
            if (currentSheild > 0)
                DecreaseShield(p_num);
            else
            {
                // 현재 체력에서 넘어온 파라미터값 빼기
                currentHp -= p_num;

                if (currentHp <= 0)
                {
                    // 플레이어가 죽으면 결과창이 뜸
                    theResult.ShowResult();
                    // 결과창이 뜨면 노트 매니저가 노트를 제거함
                    theNote.RemoveNote();
                    isDead = true;
                }
                else
                {
                    StartCoroutine(BlinkCo());
                }

                SettingHPImage();
            }

            
        }
    }

    public void DecreaseShield(int p_num)
    {
        currentSheild -= p_num;

        if (currentSheild <= 0)
            currentSheild = 0;

        SettingShieldImage();
    }

    void SettingHPImage()
    {
        for (int i = 0; i < hpImage.Length; i++)
        {
            if (i < currentHp)
                hpImage[i].SetActive(true);
            else
                hpImage[i].SetActive(false);
        }
    }

    void SettingShieldImage()
    {
        for (int i = 0; i < sheildImage.Length; i++)
        {
            if (i < currentSheild)
                sheildImage[i].SetActive(true);
            else
                sheildImage[i].SetActive(false);
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
    
    IEnumerator BlinkCo()
    {
        isBlink = true;
        while (currentBlinkCount <= blinkCount)
        {
            playerMesh.enabled = !playerMesh.enabled;
            yield return new WaitForSeconds(blinkSpeed);
            currentBlinkCount++;
        }

        playerMesh.enabled = true;
        currentBlinkCount = 0;
        isBlink = false;
    }
}
