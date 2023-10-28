using UnityEngine;
using System.Collections;
namespace Mirror.MyGame
{
    public class AutoDestory : NetworkBehaviour
    {
        public float delay=3f;
        public override void OnStartServer()
        {
            StartCoroutine(DelayedDestroy());
        }
        IEnumerator DelayedDestroy()
        {
            yield return new WaitForSeconds(delay);
            Debug.Log("Destroy particle");
            NetworkServer.Destroy(this.gameObject);

        }
    }
}