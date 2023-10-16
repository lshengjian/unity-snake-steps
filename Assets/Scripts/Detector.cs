using UnityEngine;
using UnityEngine.Events;

public class Detector : MonoBehaviour
{
    public  UnityEvent<Transform,Transform> OnTouched;
    
    public string oppositeTag="";
    // Start is called before the first frame update
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(oppositeTag))
        {
            OnTouched?.Invoke(this.transform,other.transform);
        } 
    }
}
