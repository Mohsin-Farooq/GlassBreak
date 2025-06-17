using UnityEngine;
public class BlockPhysics : MonoBehaviour
{
    [SerializeField] private Rigidbody _Block;
    [SerializeField] public float explosionForce = 1f;
    [SerializeField] public float explosionRadius = 1f;
    public void PrimaryBlockExplosion(Vector3 BallPosition)
    {
        _Block.AddExplosionForce(explosionForce, BallPosition, explosionRadius, 200f);
    }
    public void SecondaryBlockExplosion(Vector3 BallPosition)
    {
        _Block.AddExplosionForce(1, BallPosition, 0.5f, 1f, ForceMode.Force);
    }
}