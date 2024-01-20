using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitbox : MonoBehaviour
{
    // Gate direction

    public Turnstile mainTS;
    public Square squareObject;
    public string dir = "undef";
    // Start is called before the first frame update
    //private bool ini = false;
    //Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

    //private Animator tsAnimator;

    void Start()
    {
        //// Cherche le composant Animator dans les enfants de "portique"
        //tsAnimator = transform.parent.GetComponentInChildren<Animator>();

        //if (tsAnimator == null)
        //{
        //    Debug.LogError("Le composant Animator n'a pas été trouvé sur l'objet enfant 'TS'. Assurez-vous qu'il est présent et actif.");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        dir = mainTS.dir;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Déclenchez l'animation sur l'objet "square"
            if (squareObject != null)
            {

                MyCharacterController chara = other.gameObject.GetComponent<MyCharacterController>(); ;
                if(chara.dir != dir)
                {
                    Animator squareAnimator = squareObject.GetComponent<Animator>();
                    if (squareAnimator != null)
                    {
                        // Déclenchez le paramètre de déclencheur dans l'animation
                        squareAnimator.SetTrigger("Playerhit");
                    }
                }
                
            }
        }
    }


}
