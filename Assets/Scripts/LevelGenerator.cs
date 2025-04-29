using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int baseCars = 8;
    [SerializeField] private GameObject car;

    private Vector2Int size = new(8, 8);
    private float cellSize = 2f;
    private float minimumDistance = 1.5f;


    void Start()
    {
        int lvl = 1;
        if (SaveManager.Instance != null)
            lvl = SaveManager.Instance.CurrentLevel;

        Generate(lvl);
    }

    public void Generate(int level)
    {
        var rnd = new System.Random(level * 137);

        int carsToSpawn = baseCars;

        Debug.Log("Total cars to spawn: " + carsToSpawn);

        List<Vector2Int> used = new();
        List<Vector3> directions = new();

        List<Vector2Int> availableCells = new List<Vector2Int>();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                availableCells.Add(new Vector2Int(x, y));
            }
        }

        for (int i = 0; i < carsToSpawn; i++)
        {
            Vector2Int cell = Vector2Int.zero;
            bool isPositionValid = false;

            while (!isPositionValid && availableCells.Count > 0)
            {
                int index = rnd.Next(availableCells.Count);
                cell = availableCells[index];
                availableCells.RemoveAt(index);

                isPositionValid = true;

                foreach (var usedCell in used)
                {
                    if (Vector2Int.Distance(cell, usedCell) < minimumDistance)
                    {
                        isPositionValid = false;
                        break;
                    }
                }
            }

            if (!isPositionValid) continue;

            used.Add(cell);

            Vector3 pos = new Vector3(
                (cell.x - size.x / 2f) * cellSize,
                0,
                (cell.y - size.y / 2f) * cellSize);

            Direction dirEnum = (Direction)rnd.Next(4);  
            Vector3 dir = DirectionToVector(dirEnum);  

            directions.Add(dir);

            var carObject = Instantiate(car, pos, Quaternion.identity, transform);
            var carController = carObject.GetComponent<CarController>();

            carController.Init(dir);

            carObject.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        }

        GameManager.Instance.SetupLevel(level, carsToSpawn);
    }

    

    private Vector3 DirectionToVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward: return Vector3.forward;
            case Direction.Backward: return Vector3.back;
            case Direction.Left: return Vector3.left;
            case Direction.Right: return Vector3.right;
            default: return Vector3.zero;
        }
    }
}

public enum Direction
{
    Forward = 0,
    Backward = 1,
    Left = 2,
    Right = 3
}
