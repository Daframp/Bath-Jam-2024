using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterRotation : MonoBehaviour
{
    private Transform parentTransform;

    void Start()
    {
        parentTransform = transform.parent;
    }

    void LateUpdate()
    {
        // Inverse the parent's rotation to keep this object upright
        //transform.rotation = Quaternion.Inverse(parentTransform.rotation);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, parentTransform.rotation.z);
    }
}
