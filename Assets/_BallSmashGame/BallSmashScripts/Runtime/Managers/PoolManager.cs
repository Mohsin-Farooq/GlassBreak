using System.Collections.Generic;
using UnityEngine;
public class PoolManager : MonoBehaviour
{
     [SerializeField] private GameObject BallPrefab;
     [SerializeField] private int BallCount, lerpDuration;
     private readonly Queue<GameObject> ballPool = new Queue<GameObject>();
     public static PoolManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        for (int i = 0; i <= BallCount; i++)
        {
            GameObject Ball = Instantiate(BallPrefab, this.gameObject.transform);
            Ball.SetActive(false);
            ballPool.Enqueue(Ball);

        }
    }
    public GameObject GetBall(Vector3 Position,float duration)
    {
        if (ballPool.Count == 0)
        {

            Debug.LogWarning("Not enough ball");
            return null;
        }

        GameObject Ball = ballPool.Dequeue();
        Ball.transform.position = Position;

        Ball.SetActive(true);
        return Ball;
    }

    public void ReturnBall(GameObject Ball)
    {
        Ball.gameObject.SetActive(false);
        ballPool.Enqueue(Ball);
    }
}