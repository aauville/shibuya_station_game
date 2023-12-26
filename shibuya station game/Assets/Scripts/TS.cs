using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TS : MonoBehaviour
{
    // Gate direction
    public bool L2R = false;
    // Start is called before the first frame update
    private Animator animator;
    private bool ini = false;
    void Start()
    {
        animator = GetComponent<Animator>();
  
    }

    // Update is called once per frame
    void Update()
    {

        if (L2R == true && ini == false)
        {
            // Remplacez "YourAnimationTrigger" par le nom de votre déclencheur d'animation
            
            transform.gameObject.tag = "L2R_Gate";
            // Réinitialisez le booléen pour éviter de déclencher l'animation à chaque mise à jour
            ini = true;
        }
        if (L2R == false && ini == false)

        {
            animator.SetTrigger("Trigbase");
            transform.gameObject.tag = "R2L_Gate";
            ini = true;
            
        }

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
