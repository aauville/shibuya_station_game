using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TS : MonoBehaviour
{
    public bool L2R = false;
    // Start is called before the first frame update
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsTag("L2R_Tag"))
        {
            transform.gameObject.tag = "L2R_Gate";      
        }
   
        else
        {
            transform.gameObject.tag = "R2L_Gate";
        }
    }
}
