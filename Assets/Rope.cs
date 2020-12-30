using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
       // rb.isKinematic = true;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform t = this.transform.GetChild(i);
            t.gameObject.AddComponent<CharacterJoint>();
            CharacterJoint cj = t.GetComponent<CharacterJoint>();
            cj.enableProjection = true;
            cj.connectedBody =t.parent.GetComponent<Rigidbody>();
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
