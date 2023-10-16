using UnityEngine;
using UnityEngine.Events;
public class BoundLimit : MonoBehaviour
{
      public  UnityEvent<Transform> OnOutBound;
      private void OnTriggerExit2D(Collider2D other)
    {

        OnOutBound?.Invoke(other.transform);

    }
}
