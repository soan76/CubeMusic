using Unity.VisualScripting;
using UnityEngine;

public class CenterFlame : MonoBehaviour
{
    bool musicStart = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!musicStart)
        {
            if (collision.CompareTag("Note"))
            {
                AudioManager.instance.PlayBGM("BGM0");
                musicStart = true;
            }
        }
        
    }

}
