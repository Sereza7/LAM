using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    Rigidbody m_rigidbody;

    void start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }
    
    void OnCollisionEnter (Collision collision)
    {
        m_rigidbody.velocity = Vector3.zero; 
    }
}
