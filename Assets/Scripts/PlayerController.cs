using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int startPlayerPositionRow = 4;
    public int startPlayerPositionColumn = 0;

    public Sprite leftPig;
    public Sprite rightPig;
    public Sprite upPig;
    public Sprite downPig;

    public GameObject bombPrefab;
    public GameManager gameManager;

    private SpriteRenderer spriteRenderer;
    private Vector2 targetPosition;
    private Vector2 lastDirection;
    private int currentPlayerPositionRow;
    private int currentPlayerPositionColumn;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        targetPosition = GameField.fieldArray[startPlayerPositionRow, startPlayerPositionColumn].transform.position;
        currentPlayerPositionRow = startPlayerPositionRow;
        currentPlayerPositionColumn = startPlayerPositionColumn;
        lastDirection = Vector2.right;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveLeft();
        if (Input.GetKeyDown(KeyCode.RightArrow)) MoveRight();
        if (Input.GetKeyDown(KeyCode.UpArrow)) MoveUp();
        if (Input.GetKeyDown(KeyCode.DownArrow)) MoveDown();
        if (Input.GetKeyDown(KeyCode.Space)) PlaceBomb();

        MovePlayer();
    }

    public void MoveLeft()
    {
        TryToMovePlayer(Vector2.left);
    }

    public void MoveRight()
    {
        TryToMovePlayer(Vector2.right);
    }

    public void MoveUp()
    {
        TryToMovePlayer(Vector2.up);
    }

    public void MoveDown()
    {
        TryToMovePlayer(Vector2.down);
    }

    private void TryToMovePlayer(Vector2 direction)
    {
        GameObject targetObject;
        try
        {
            targetObject = GameField.fieldArray[currentPlayerPositionRow - (int)direction.y, currentPlayerPositionColumn + (int)direction.x];
        }
        catch (System.Exception)
        {
            return;
        }

        if (targetObject.name == "stone")
        {
            return;
        }

        RenderPlayer(direction);
        lastDirection = direction;
        targetPosition = targetObject.transform.position;

        currentPlayerPositionRow -= (int)direction.y;
        currentPlayerPositionColumn += (int)direction.x;
    }

    private void RenderPlayer(Vector2 direction)
    {
        if (direction == Vector2.left)
        {
            spriteRenderer.sprite = leftPig;
        }
        if (direction == Vector2.right)
        {
            spriteRenderer.sprite = rightPig;
        }
        if (direction == Vector2.up)
        {
            spriteRenderer.sprite = upPig;
        }
        if (direction == Vector2.down)
        {
            spriteRenderer.sprite = downPig;
        }
    }

    private void MovePlayer()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 3);
    }

    public void PlaceBomb()
    {
        GameObject targetPlace;
        try
        {
            targetPlace = GameField.fieldArray[currentPlayerPositionRow - (int)lastDirection.y, currentPlayerPositionColumn + (int)lastDirection.x];
        }
        catch (System.Exception)
        {
            return;
        }

        if (targetPlace.name == "stone" || targetPlace.name == "bomb")
        {
            return;
        }
        
        GameObject bomb = Instantiate(bombPrefab, targetPlace.transform.position, Quaternion.identity);
        GameField.fieldArray[currentPlayerPositionRow - (int)lastDirection.y, currentPlayerPositionColumn + (int)lastDirection.x].name = "bomb";
        bomb.GetComponent<Bomb>().SetPosition(currentPlayerPositionRow - (int)lastDirection.y, currentPlayerPositionColumn + (int)lastDirection.x);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Explosion"))
        {
            gameManager.DecreaseLives(1);
            currentPlayerPositionRow = startPlayerPositionRow;
            currentPlayerPositionColumn = startPlayerPositionColumn;
            transform.position = GameField.fieldArray[currentPlayerPositionRow, currentPlayerPositionColumn].transform.position;
            targetPosition = transform.position;
        }
    }

}
