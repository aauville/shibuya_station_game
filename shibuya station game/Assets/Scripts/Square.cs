using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject TS;
    //public string dir = "none";

    public Animator porteAnim;

    void Start()
    {
        porteAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
        //if (TS.CompareTag("L2R_Gate"))
        //{
        //    dir = "in";
        //    transform.gameObject.tag = "L2R_Gate";
        //}
        //else if (TS.CompareTag("R2L_Gate"))
        //{
        //    dir = "out";
        //    transform.gameObject.tag = "R2L_Gate";
        //}
    }
}
