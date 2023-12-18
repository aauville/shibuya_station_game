using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{



    public bool LRdir= false;
    public GameObject sign;

    public float moveSpeed = 500f;
    private bool isDragging = false;
    private List<Vector2> pathPoints = new List<Vector2>();

    private SpriteRenderer spriteRenderer;
    private LineRenderer lineRenderer;

    private bool allowDrag = true;
    private bool hitGate = false;

    private string gate = "R2L_Gate";



    private void Start()
    {
        // Retrieve the SpriteRenderer component at the start
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Add LineRenderer to the GameObject
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0; // Initialize the position of LineRenderer to 0
        lineRenderer.startWidth = 3f; // Line width
        lineRenderer.endWidth = 3f;
        lineRenderer.numCapVertices = 30;
        lineRenderer.numCornerVertices = 30;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.textureMode = LineTextureMode.Tile;

        if(LRdir == false)
        {
            gate = "L2R_Gate";
        }

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

        if (hitGate == true)
        {
            if (pathPoints.Count > 0)
            {
                Vector2 targetPosition = pathPoints[0];
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed*2 * Time.deltaTime);
                if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
                {
                    pathPoints.RemoveAt(0);
                }
            }
            else if (pathPoints.Count == 0)
            {
                hitGate = false;
                allowDrag = true;
            }
        }
        // if the character has an active path, move towards the last point in the path
        else if (pathPoints.Count > 0)
        {
            Vector2 targetPosition = pathPoints[0];
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // if the character has reached the last point in the path, remove that point from the path
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                pathPoints.RemoveAt(0);
            }

            // Update LineRenderer with the remaining path points
            lineRenderer.positionCount = pathPoints.Count;
            lineRenderer.SetPositions(ConvertVector2ToVector3(pathPoints.ToArray()));

            // Set transparency based on distance to the character
            UpdateLineTransparency();
        }


    }

    // Function to clear the existing path
    private void ClearPath()
    {
        pathPoints.Clear();
        // Reset the position of LineRenderer
        lineRenderer.positionCount = 0;
    }

    // Function to convert Vector2[] to Vector3[]
    private Vector3[] ConvertVector2ToVector3(Vector2[] vector2Array)
    {
        Vector3[] vector3Array = new Vector3[vector2Array.Length];
        for (int i = 0; i < vector2Array.Length; i++)
        {
            vector3Array[i] = new Vector3(vector2Array[i].x, vector2Array[i].y, 0f);
        }
        return vector3Array;
    }

    private void wrongGate(Vector2 nm)
    {
        ClearPath();
        Vector2 newPos = transform.position;
       // pathPoints.Add(newPos+10*nm);
        for(float i = 1; i<100; i++)
        {
            newPos += nm *(20f/i);
            pathPoints.Add(newPos);
        }
        hitGate = true;
        allowDrag = false;
    }

    // Function to update line transparency based on distance to the character
    private void UpdateLineTransparency()
    {
        for (int i = 0; i < pathPoints.Count; i++)
        {
            float distance = Vector2.Distance(transform.position, pathPoints[i]);
            float transparency = Mathf.Clamp01(1f - distance / 5f); // You can adjust the divisor for the desired fading effect
            Color color = lineRenderer.startColor;
            color.a = transparency;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(gate))
        {
            Vector2 hit = collision.contacts[0].normal;
            wrongGate(hit);
            //ClearPath()
            allowDrag = false;
            Animator porteAnimator = collision.gameObject.GetComponent<Animator>();
            Animator signAnimator = sign.GetComponent<Animator>();
            if (porteAnimator != null)
            {
                porteAnimator.SetTrigger("Playerhit");
                signAnimator.SetTrigger("Playerhit");
            }

        }
        if (collision.gameObject.CompareTag("Player"))
        {
            ClearPath();
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Gate"))
        {
            //ClearPath();
            allowDrag = false;
        }

    }

}
