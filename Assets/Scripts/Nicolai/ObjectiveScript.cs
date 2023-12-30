using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveScript : MonoBehaviour
{
    public float upSpeed = 1f;
    public float spinSpeed = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);

        transform.Translate(Vector3.up * Mathf.Cos(Time.timeSinceLevelLoad) * upSpeed * Time.deltaTime);
    }
}
