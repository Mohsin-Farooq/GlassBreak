using UnityEngine;

namespace GlassBreakGame
{
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
            GameObject ball = PoolManager.instance.GetBall(transform.position, 1);
            ball.transform.localScale = Vector3.one * 2f;
           
        }
    }
}