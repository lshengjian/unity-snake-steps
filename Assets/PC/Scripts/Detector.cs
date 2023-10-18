using UnityEngine;
using System;
using TMPro;

public class Detector : MonoBehaviour
{
    public event Action<Transform,Transform> OnTouched= (_,_) => {};
    
    public string oppositeTag="";
    // Start is called before the first frame update
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(oppositeTag))
        {
           // Debug.Log(this.name+"-->"+other.name);
            OnTouched.Invoke(this.transform,other.transform);
        } 
    }
}
