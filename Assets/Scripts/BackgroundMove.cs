using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.Tracing;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    private int counter = 0;
    bool Change = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Change)
        {
            if (counter <= 10)
            {
                gameObject.transform.position += new Vector3((float)0.5, 0);
                StartCoroutine(wait());
                counter++;
            }
            else
            {
                gameObject.transform.position -= new Vector3((float)5.5, 0);
                StartCoroutine(wait());
                counter = 0;
            }
        }
        
    }
    private IEnumerator wait()
    {
        Change = false;
        yield return new WaitForSeconds(0.1f);
        Change = true;
    }
}
