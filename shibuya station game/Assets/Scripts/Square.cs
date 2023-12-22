using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject TS;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (TS.CompareTag("L2R_Gate"))
        {

            transform.gameObject.tag = "L2R_Gate";
        }
        else if (TS.CompareTag("R2L_Gate"))
        {

            transform.gameObject.tag = "R2L_Gate";
        }
    }
}
