using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turnstile : MonoBehaviour
{
    // Gate direction
    public string dir = "in";
    public TS body;
    public Square gate;

    public Animator bodyAnim;


    // Direction change parameters - can be randomized
    public float timeOffset = 2f; //offset at start
    public float period = 5f; // time between direction change 

    private bool ini = false;
    void Start()
    {
        bodyAnim = body.GetComponent<Animator>(); ;
        StartCoroutine(TriggerAnimationWithDelay());

        if (dir == "in" && ini == false)
        {
            transform.gameObject.tag = "L2R_Gate";
            body.transform.tag = "L2R_Gate";
            gate.transform.tag = "L2R_Gate";
            ini = true;
        }
        if (dir == "out" && ini == false)

        {
            bodyAnim.SetTrigger("Trigbase");
            transform.gameObject.tag = "R2L_Gate";
            body.transform.tag = "R2L_Gate";
            gate.transform.tag = "R2L_Gate";
            ini = true;

        }

    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = bodyAnim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsTag("L2R_Tag"))
        {
            dir = "in";
            transform.gameObject.tag = "L2R_Gate";
            body.transform.tag = "L2R_Gate";
            gate.transform.tag = "L2R_Gate";
        }
        else
        {
            dir = "out";
            transform.gameObject.tag = "R2L_Gate";
            body.transform.tag = "L2R_Gate";
            gate.transform.tag = "L2R_Gate";
        }
    }


    IEnumerator TriggerAnimationWithDelay()
    {
        // Délai initial avant le premier déclenchement
        yield return new WaitForSeconds(timeOffset);

        while (true)
        {
            // Déclencher l'animation
            bodyAnim.SetTrigger("ChangeDir");

            // Attendre 5 secondes avant le prochain déclenchement
            yield return new WaitForSeconds(5f);
        }
    }

}
