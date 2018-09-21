using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
