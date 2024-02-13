using UnityEngine;
using System.Collections.Generic;

public class PointsEffectManager : MonoBehaviour
{
    public GameObject pointsEffectPrefab;
    public int poolSize = 10;

    private Queue<PointsEffect> pointsEffectPool = new Queue<PointsEffect>();
    public float verticalOffset = 30.0f;

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject pointsEffectObject = Instantiate(pointsEffectPrefab, transform);
            PointsEffect pointsEffect = pointsEffectObject.GetComponent<PointsEffect>();
            if (pointsEffect != null)
            {
                pointsEffectPool.Enqueue(pointsEffect);
            }
            pointsEffectObject.SetActive(false);

        }
    }
    public void ShowPointsEffect(float pointValue)
    {
        if (pointsEffectPool.Count > 0)
        {
            PointsEffect pointsEffect = pointsEffectPool.Dequeue();
        
            // Activate the object before starting the coroutine
            pointsEffect.gameObject.SetActive(true);

            pointsEffect.Activate(transform.position, pointValue, verticalOffset * pointsEffectPool.Count);
        }
    }

    public void ReturnToPool(PointsEffect pointsEffect)
    {
        pointsEffect.gameObject.SetActive(false);
        pointsEffectPool.Enqueue(pointsEffect);
    }
}