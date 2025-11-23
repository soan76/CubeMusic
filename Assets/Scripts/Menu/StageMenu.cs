using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable] // inspector 창에서 수정 가능하게
public class Song
{
    public string name;
    public string composer;
    public int bpm; // bpm에 따라 노트 속도 변경
    public Sprite sprite;
}

public class StageMenu : MonoBehaviour
{
    [SerializeField] Song[] songList = null;
    [SerializeField] TextMeshProUGUI txtSongName = null;
    [SerializeField] TextMeshProUGUI txtSongComposer = null;
    [SerializeField] Image imgDisk = null;

    [SerializeField] GameObject TitleMenu = null;

    // 현재 선택된 노래 
    int currentSong = 0;

    void Start()
    {
        SettingSong();
    }

    public void BtnNext()
    {
        // 터치효과 사운드
        AudioManager.instance.PlaySFX("Touch");

        if(++currentSong > songList.Length - 1)
        {
            currentSong = 0; // 마지막 노래에서 다시 처음으로
        }
        SettingSong();
    }

    public void BtnPrior()
    {
        AudioManager.instance.PlaySFX("Touch");

        if(--currentSong < 0)
        {
            currentSong = songList.Length - 1;
        }
        SettingSong();
    }

    // 노래 세팅
    void SettingSong()
    {
        txtSongName.text = songList[currentSong].name;
        txtSongComposer.text = songList[currentSong].composer;
        imgDisk.sprite = songList[currentSong].sprite;

        // 버튼을 누르면 해당 노래가 나오게 함
        AudioManager.instance.PlayBGM("BGM" + currentSong);
    }

    public void BtnBack()
    {
        TitleMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void BtnPlay()
    {
         int t_bpm = songList[currentSong].bpm;

        GameManager.instance.GameStart(currentSong, t_bpm);
        this.gameObject.SetActive(false);
    }
}
