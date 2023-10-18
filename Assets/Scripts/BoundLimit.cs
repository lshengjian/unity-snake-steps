
using UnityEngine;
using System;
public class BoundLimit : MonoBehaviour
{
      public event Action<Transform> OnOutBound=_=>{};
      private void OnTriggerExit2D(Collider2D other)
    {

        OnOutBound.Invoke(other.transform);

    }
}
