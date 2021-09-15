using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    public GameObject stone;
    public int fieldWidth = 17;
    public int fieldHeight = 9;

    public GameObject startPosition;
    public float cellSpacingX = 80f;
    public float cellSpacingY = 80f;
    public float rowSpacingX = -10f;


    public static GameObject[,] fieldArray = new GameObject[9, 17];

    void Awake()
    {
        CreateField();
    }

    private void CreateField()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < fieldHeight; i++)
        {
            for (int j = 0; j < fieldWidth; j++)
            {
                if (i % 2 != 0 && j % 2 != 0)
                {
                    fieldArray[i, j] = Instantiate(stone, startPosition.transform.position + new Vector3(cellSpacingX * j - rowSpacingX * i, -cellSpacingY * i), Quaternion.identity, transform);
                    fieldArray[i, j].name = "stone";
                }
                else
                {
                    fieldArray[i, j] = new GameObject("empty");
                    fieldArray[i, j].transform.position = startPosition.transform.position + new Vector3(cellSpacingX * j - rowSpacingX * i, -cellSpacingY * i);
                    fieldArray[i, j].transform.SetParent(transform);
                }
            }
        }
    }
}
