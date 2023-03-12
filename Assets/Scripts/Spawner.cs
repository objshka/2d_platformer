using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Coin _prefab;
    [SerializeField] private Transform[] _spawnPoints;

    private void Start()
    {
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            Instantiate(_prefab, _spawnPoints[i].transform.position, Quaternion.identity);
        }
    }
}