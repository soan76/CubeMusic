using UnityEngine;

public class DataManager : MonoBehaviour
{
    public int[] score;

    void Start()
    {
        LoadScore();
    }

    public void SaveScore()
    {
        for(int i = 0; i < score.Length; i++)
        {
            PlayerPrefs.SetInt("Score" + (i + 1), score[i]);
        }
    }

    public void LoadScore()
    {
        for(int i = 0; i < score.Length; i++)
        {
            string key = "Score" + (i + 1);

            if(PlayerPrefs.HasKey(key))
            {
                score[i] = PlayerPrefs.GetInt(key);
            }
        }
    }    
}
