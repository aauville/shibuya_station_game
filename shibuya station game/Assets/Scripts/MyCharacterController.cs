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

    private SpriteRenderer spriteRenderer;
    private LineRenderer lineRenderer;
    [SerializeField]
    public List<Sprite> characterSprites;
    Rigidbody2D rb;

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
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);

            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                isDragging = true;
                ClearPath();
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector2 currentTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathPoints.Add(currentTouchPosition);
            lineRenderer.positionCount = pathPoints.Count;
            lineRenderer.SetPositions(ConvertVector2ToVector3(pathPoints.ToArray()));
            UpdateLineTransparency();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (pathPoints.Count > 0)
        {
            Vector2 targetPosition = pathPoints[0];
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                pathPoints.RemoveAt(0);
            }

            lineRenderer.positionCount = pathPoints.Count;
            lineRenderer.SetPositions(ConvertVector2ToVector3(pathPoints.ToArray()));
            UpdateLineTransparency();

            // Vérifie si le personnage est arrivé à destination
            ArrivedAtDestination();
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

    public void SetDestination(GameObject destination)
    {
        destinationStation = destination;
    }


    private void ArrivedAtDestination()
    {
        if (IsAtCorrectStation(destinationStation.GetComponent<BoxCollider2D>()))
        {
            GameManager.Instance.IncrementScore();
            Destroy(gameObject);
        }
    }

    private bool IsAtCorrectStation(BoxCollider2D stationCollider)
    {
        return stationCollider.OverlapPoint(transform.position);
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

    public float bumpForce = 5f;
    public float bumpDuration = 0.5f;
    private bool isBumping = false;


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isBumping)
            {
                StartCoroutine(BumpCoroutine());
            }
            Debug.Log("Collision entre deux personnages !");
        }
    }

    IEnumerator BumpCoroutine()
    {
        isBumping = true;
        float elapsedTime = 0f;
        while (elapsedTime < bumpDuration)
        {
            float bumpForcePercentage = 1f - (elapsedTime / bumpDuration);
            rb.velocity += (Vector2.up * bumpForce) * bumpForcePercentage;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isBumping = false;
    }
}
