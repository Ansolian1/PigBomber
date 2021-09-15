using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private int bombRow;
    private int bombColumn;

    public int bombRadius = 3;
    public GameObject explosionPrefab;

    void Start()
    {
        StartCoroutine(Timer());
    }

    public void SetPosition(int row, int column)
    {
        bombRow = row;
        bombColumn = column;
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(3f);
        Explosion();
    }

    private void Explosion()
    {
        GameField.fieldArray[bombRow, bombColumn].name = "empty";
        ExploseLine(Vector2.left);
        ExploseLine(Vector2.right);
        ExploseLine(Vector2.up);
        ExploseLine(Vector2.down);
        Destroy(gameObject);
    }

    private void ExploseLine(Vector2 dir)
    {
        for (int i = 1; i <= bombRadius; i++)
        {
            try
            {
                if (GameField.fieldArray[bombRow + (int)dir.y * i, bombColumn + (int)dir.x * i].name == "stone")
                    break;
                Instantiate(explosionPrefab, GameField.fieldArray[bombRow + (int)dir.y * i, bombColumn + (int)dir.x * i].transform.position, Quaternion.identity);
            }
            catch (System.Exception)
            {
                break;
            }
        }
    }
}
