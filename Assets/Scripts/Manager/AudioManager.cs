using UnityEngine;

[System.Serializable]
// 사운드 정보
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] Sound[] sfx = null; // 효과음 배열
    [SerializeField] Sound[] bgm = null; // 효과음 재생용 오디오 소스

    [SerializeField] AudioSource bgmPlayer = null; 
    [SerializeField] AudioSource[] sfxPlayer = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(string p_bgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (p_bgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip; // BGM 오디오 소스에 클립 할당 
                bgmPlayer.Play();
            }
        }
    }

    public void StopBGM()
    {
        if(bgmPlayer != null)
            bgmPlayer.Stop();
    }
    
    public void PlaySFX(string p_sfxName)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == sfx[i].name)
            {
                for (int x = 0; x < sfx.Length; x++)
                {
                    if (!sfxPlayer[x].isPlaying)
                    {
                        sfxPlayer[x].clip = sfx[i].clip;
                        sfxPlayer[x].Play();
                        return;
                    }
                }
                Debug.Log("모든 SFX 오디오 소스가 사용 중입니다.");
                return;
            }
        }

        Debug.Log(p_sfxName + "이름의 SFX를 찾을 수 없습니다.");
    }
}
