using UnityEngine;
public class BallSpawning : MonoBehaviour
{
    public static BallSpawning instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        BallSpawm();
    }
    public void Spawn()
    {
        BallSpawm();
    }
    private void BallSpawm()
    {
        GameObject ball = PoolManager.instance.GetBall(this.transform.position, 1);
        ball.transform.localScale = Vector3.one;
      //  BallVisualEffects visual = ball.GetComponent<BallVisualEffects>();
       //  StartCoroutine(visual.ScaleUp(0.2f, Vector3.one * 1.5f));
    }
}