using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class MyCharacterController : MonoBehaviour
{
    public float moveSpeed = 1f;
    [SerializeField]
    public float lineSize = 1f;
    private bool isDragging = false;
    private List<Vector2> pathPoints = new List<Vector2>();
    private GameObject destinationStation;
    public GameObject stationSign;
    private SpriteRenderer spriteRenderer;
    private LineRenderer lineRenderer;
    [SerializeField]
    public List<Sprite> characterSprites;
    Rigidbody2D rb;
    public bool canMove = true;

    private bool allowDrag = true; // [Unused] enable/disable dragging in can of problem with collisions (can still pass through if LMB is held)
    private bool hitGate = false; // On when wrong gate hit

    public bool LRdir = false; // Character direction (LR false => Right to left): should be randomized
    private string gate = "R2L_Gate"; // Default matching gate tag (Right to Left). Is set in start() according to LRdir
                                      // can be improved but still works

    public string dir = "none"; // exit to station -> in otherwise out or tsf

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;
        lineRenderer.numCapVertices = 30;
        lineRenderer.numCornerVertices = 30;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.textureMode = LineTextureMode.Tile;
        ChooseRandomSprite();
        rb = gameObject.GetComponent<Rigidbody2D>();
        /*
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        */
        // Set the correct gate tag 
        if (LRdir == false)
        {
            gate = "L2R_Gate";
        }
        spriteRenderer.sortingOrder = 1;
        lineRenderer.sortingOrder = 1;
    }

    void Update()
    {
        // if the user touches or clicks on the character, start drawing the path
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);

            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                isDragging = true;
                // Clear the existing path when a new drag starts
                ClearPath();
            }
        }

        // if the user is dragging the character, add the current touch position to the path
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector2 currentTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // add the current touch position to the path
            pathPoints.Add(currentTouchPosition);

            // Update LineRenderer with the new path points
            lineRenderer.positionCount = pathPoints.Count;
            lineRenderer.SetPositions(ConvertVector2ToVector3(pathPoints.ToArray()));

            // Set transparency based on distance to the character
            UpdateLineTransparency();
        }

        // if the user stops touching the screen, stop drawing the path
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // if the character has an active path, move towards the last point in the path
        if (pathPoints.Count > 0 && canMove)
        {
            Vector2 targetPosition = pathPoints[0];
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // if the character has reached the last point in the path, remove that point from the path
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                pathPoints.RemoveAt(0);
            }
            if (hitGate == false || hitGate == true) // no trace when gate kickback 
            {
                // Update LineRenderer with the remaining path points
                lineRenderer.positionCount = pathPoints.Count;
                lineRenderer.SetPositions(ConvertVector2ToVector3(pathPoints.ToArray()));

                // Set transparency based on distance to the character
                UpdateLineTransparency();
            }
        }

        if (!canMove)
        {
            ClearPath();
            canMove = true;
        }

        else if (pathPoints.Count == 0)
        {
            hitGate = false;
            allowDrag = true;
        }

    }

    private void ClearPath()
    {
        pathPoints.Clear();
        lineRenderer.positionCount = 0;
    }

    private Vector3[] ConvertVector2ToVector3(Vector2[] vector2Array)
    {
        Vector3[] vector3Array = new Vector3[vector2Array.Length];
        for (int i = 0; i < vector2Array.Length; i++)
        {
            vector3Array[i] = new Vector3(vector2Array[i].x, vector2Array[i].y, 0f);
        }
        return vector3Array;
    }

    // Function to set new path (kickback) when wrong gate
    // nm is the normal vector to the surface hit
    private void wrongGate(Vector2 nm)
    {
        ClearPath();
        Vector2 newPos = transform.position;
        //pathPoints.Add(newPos+100*nm);
        // Kickback effect (new trace)

        for (float i = 1; i < 200; i++)
        {
            newPos += nm * (20f / i);
            pathPoints.Add(newPos);
        }
        hitGate = false;
        allowDrag = false;
    }

    // Function to update line transparency based on distance to the character
    private void UpdateLineTransparency()
    {
        for (int i = 0; i < pathPoints.Count; i++)
        {
            float distance = Vector2.Distance(transform.position, pathPoints[i]);
            float transparency = Mathf.Clamp01(1f - distance / 5f);
            Color color = lineRenderer.startColor;
            color.a = transparency;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }

    public void SetDestination(GameObject destination, GameObject associatedObject, string direction)
    {
        destinationStation = destination;
        stationSign = associatedObject;
        dir = direction;

        if (stationSign != null)
        {

            var originalScale = stationSign.transform.localScale;
            stationSign.transform.parent = transform;
            stationSign.transform.localScale = originalScale;

            float signOffset = 3.0f; // distance between cat and sign
            Vector3 newPosition = stationSign.transform.localPosition + new Vector3(0f, signOffset, 0f);
            stationSign.transform.localPosition = newPosition;

        }
    }


    private void ChooseRandomSprite()
    {
        if (characterSprites != null && characterSprites.Count > 0)
        {
            int randomIndex = Random.Range(0, characterSprites.Count);
            Sprite randomSprite = characterSprites[randomIndex];
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = randomSprite;
            }
        }
        else
        {
            Debug.LogWarning("No sprite defined in the list characterSprites");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           // Debug.LogError("Collision");
            ClearPath();
            //GameManager.Instance.IncrementAnger();
            canMove = false;
        }

        hitbox hb = other.gameObject.GetComponent<hitbox>();
        string dirTS = "undefined";
        if (hb != null)
        {
            // La classe est "TS"
            dirTS = hb.dir;
            //Debug.Log("La valeur de L2R de la classe TS est : " + valeurL2R);
            if (dirTS != dir)
            {
                if(hitGate == false)
                {
                    // Generates the normal vector to the surface hit 
                    // Current collider is rectangular (we can try circular to have better dynamics)
                    Vector2 hit = other.transform.position - transform.position;
                    hit = -hit.normalized;
                    allowDrag = false;
                    wrongGate(hit);
                    // ClearPath()


                    // Animation events
                    Animator porteAnimator = other.gameObject.GetComponent<Animator>();
                    // Animator signAnimator = sign.GetComponent<Animator>();
                    if (porteAnimator != null)
                    {
                        porteAnimator.SetTrigger("Playerhit");
                        // signAnimator.SetTrigger("Playerhit");
                    }
                }
               
            }
            else if (dirTS == dir)
            {
                StartCoroutine(DisableCollisionTemporarily(other.collider));
                return;
            }
        }


    }
    IEnumerator DisableCollisionTemporarily(Collider2D otherCollider)
    {
        Physics2D.IgnoreCollision(otherCollider, GetComponent<Collider2D>(), true);
        yield return new WaitForSeconds(20f); // Temps d'ignorance de la collision
        Physics2D.IgnoreCollision(otherCollider, GetComponent<Collider2D>(), false);
    }

    // Function to manage collision events
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == destinationStation)
        {
            GameManager.Instance.IncrementScore();
            Destroy(gameObject);
        }

        //Square tsComponent = other.gameObject.GetComponent<Square>();
        //string dirTS = "undefined";
        //if (tsComponent != null)
        //{
        //    // La classe est "TS"
        //    dirTS = tsComponent.dir;
        //    //Debug.Log("La valeur de L2R de la classe TS est : " + valeurL2R);
        //    if (dirTS != dir)
        //    {
        //        // Generates the normal vector to the surface hit 
        //        // Current collider is rectangular (we can try circular to have better dynamics)
        //        Vector2 hit = other.transform.position - transform.position;
        //        hit = hit.normalized;
        //        allowDrag = false;
        //        wrongGate(hit);
        //        // ClearPath()
                

        //        // Animation events
        //        Animator porteAnimator = other.gameObject.GetComponent<Animator>();
        //        // Animator signAnimator = sign.GetComponent<Animator>();
        //        if (porteAnimator != null)
        //        {
        //            porteAnimator.SetTrigger("Playerhit");
        //            // signAnimator.SetTrigger("Playerhit");
        //        }
        //    }
        //}

        

        //just in case 
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.LogError("Collision");
            ClearPath();
            //GameManager.Instance.IncrementAnger();
            canMove = false;
        }

        if (other.CompareTag("Gatebody"))
        {
            // Generates the normal vector to the surface hit 
            // Current collider is rectangular (we can try circular to have better dynamics)
            Vector2 hit = other.transform.position - transform.position;
            hit = hit.normalized;
            wrongGate(hit);
            // ClearPath()
            allowDrag = false;
            ClearPath();
        }
    }


    // [Unused] Function to disable dragging when player inside of a colloder object (oui ça peut arriver et le perso continue sa route)
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Square")
        {
            ClearPath();
            allowDrag = false;
        }
    }

    private void OnDestroy()
    {
        if (stationSign != null)
        {
            Destroy(stationSign);
        }
    }
}

