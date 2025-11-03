using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // GetComponentInParent = 부모객체의 컴포넌트를 가져옴
            other.GetComponentInParent<PlayerController>().ResetFalling();
        }      
    }
}
