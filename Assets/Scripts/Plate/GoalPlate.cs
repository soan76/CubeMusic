using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    AudioSource theAudio;
    NoteManager theNote;

    void Start()
    {
        theAudio = GetComponent<AudioSource>();
        theNote = FindAnyObjectByType<NoteManager>();
    }

    //플레이어에 콜라이더 감지되면 audio 재생
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            theAudio.Play();
            // 콜라이더 감지되면 이동 제한
            PlayerController.s_canPresskey = false;
            theNote.RemoveNote();
        }
    }
}
