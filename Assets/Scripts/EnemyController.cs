using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int startEnemyPositionRow = 4;
    public int startEnemyPositionColumn = 4;

    public GameManager gameManager;

    public Sprite leftEnemy;
    public Sprite rightEnemy;
    public Sprite upEnemy;
    public Sprite downEnemy;

    public Sprite leftEnemyDirt;
    public Sprite rightEnemyDirt;
    public Sprite upEnemyDirt;
    public Sprite downEnemyDirt;

    private bool isDirted = false;
    private bool isStopped = false;

    private SpriteRenderer spriteRenderer;
    private Vector2 targetPosition;
    private Vector2 lastDirection;
    private int currentEnemyPositionRow;
    private int currentEnemyPositionColumn;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        targetPosition = GameField.fieldArray[startEnemyPositionRow, startEnemyPositionColumn].transform.position;
        currentEnemyPositionRow = startEnemyPositionRow;
        currentEnemyPositionColumn = startEnemyPositionColumn;
        StartCoroutine(Move());
        lastDirection = Vector2.right;
    }

    void Update()
    {
        MoveEnemy();
    }

    IEnumerator Move()
    {
        while (true)
        {
            if (!isStopped)
            {
                Vector2 direction = GetRandomDirection();
                TryToMoveEnemy(direction);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private Vector2 GetRandomDirection()
    {
        int i = Random.Range(0, 4);
        switch (i)
        {
            case 0:
                return Vector2.left;
            case 1:
                return Vector2.right;
            case 2:
                return Vector2.up;
            case 3:
                return Vector2.down;
            default:
                return Vector2.zero;
        }
    }

    private void TryToMoveEnemy(Vector2 direction)
    {
        GameObject targetObject;
        try
        {
            targetObject = GameField.fieldArray[currentEnemyPositionRow - (int)direction.y, currentEnemyPositionColumn + (int)direction.x];
        }
        catch (System.Exception)
        {
            return;
        }

        if (targetObject.name == "stone")
        {
            return;
        }

        RenderEnemy(direction);
        targetPosition = targetObject.transform.position;
        lastDirection = direction;
        currentEnemyPositionRow -= (int)direction.y;
        currentEnemyPositionColumn += (int)direction.x;
    }

    private void MoveEnemy()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 3);
    }

    private void RenderEnemy(Vector2 direction)
    {
        if (direction == Vector2.left)
        {
            if (isDirted)
                spriteRenderer.sprite = leftEnemyDirt;
            else
                spriteRenderer.sprite = leftEnemy;
        }
        if (direction == Vector2.right)
        {
            if (isDirted)
                spriteRenderer.sprite = rightEnemyDirt;
            else
                spriteRenderer.sprite = rightEnemy;
        }
        if (direction == Vector2.up)
        {
            if (isDirted)
                spriteRenderer.sprite = upEnemyDirt;
            else
                spriteRenderer.sprite = upEnemy;
        }
        if (direction == Vector2.down)
        {
            if (isDirted)
                spriteRenderer.sprite = downEnemyDirt;
            else
                spriteRenderer.sprite = downEnemy;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion"))
        {
            if (!isDirted)
            {
                StartCoroutine(ToClean());
                gameManager.IncreaseScore(100);
                isDirted = true;
                RenderEnemy(lastDirection);
            }
            else if(!isStopped)
            {
                StopCoroutine(ToClean());
                gameManager.IncreaseScore(500);
                isStopped = true;
                StartCoroutine(ToLetMove());
            }
        }
    }

    IEnumerator ToClean()
    {
        yield return new WaitForSeconds(6f);
        isDirted = false;
        RenderEnemy(lastDirection);
    }

    IEnumerator ToLetMove()
    {
        yield return new WaitForSeconds(3f);
        isDirted = false;
        isStopped = false;
        RenderEnemy(lastDirection);
    }
}
