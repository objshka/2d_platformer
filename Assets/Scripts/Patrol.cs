using UnityEngine;
using DG.Tweening;

public class Patrol : MonoBehaviour
{
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private float duration;

    private void Start()
    {
        transform.DOMove(_targetPosition.position, duration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
