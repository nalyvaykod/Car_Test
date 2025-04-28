using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private CarController carPrefab;
    [SerializeField] private Vector2Int size = new(10, 6);
    [SerializeField] private float cellSize = 2.5f;
    [SerializeField] private int baseCars = 10;

    public void Generate(int level)
    {
        var rnd = new System.Random(level * 137);
        int carsToSpawn = baseCars + level * 2;
        List<Vector2Int> used = new();

        for (int i = 0; i < carsToSpawn; i++)
        {
            Vector2Int cell;
            do cell = new Vector2Int(rnd.Next(size.x), rnd.Next(size.y));

            while (used.Contains(cell));

            used.Add(cell);

            Vector3 pos = new Vector3(
                (cell.x - size.x / 2f) * cellSize,
                0,
                (cell.y - size.y / 2f) * cellSize);

            Vector3 dir = rnd.Next(4) switch
            {
                0 => Vector3.forward,
                1 => Vector3.back,
                2 => Vector3.left,
                _ => Vector3.right
            };

            var car = Instantiate(carPrefab, pos, Quaternion.identity, transform);
            car.Init(dir);
        }
        GameManager.Instance.SetupLevel(level, carsToSpawn);
    }

    void Start()
    {
        int lvl = SaveManager.Instance.CurrentLevel;
        Generate(lvl);
    }


}
