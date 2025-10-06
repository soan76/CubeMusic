using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    TimingManager theTimingManager;

    void Start()
    {
        theTimingManager = FindAnyObjectByType<TimingManager>();
    }
    
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //판정 체크
            theTimingManager.CheckTiming();
        }
    }
}
