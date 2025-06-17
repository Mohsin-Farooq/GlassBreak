using UnityEngine;

[RequireComponent(typeof(BlockCollisionAction))]
[RequireComponent(typeof(BlockCollision))]
[RequireComponent(typeof(BlockPhysics))]
public class BlockManager : MonoBehaviour
{
    [SerializeField] private BlockCollisionAction _BlockCollisionAction;
    [SerializeField] private BlockCollision _BlockCollision;
    [SerializeField] private BlockPhysics _BlockPhysics;
    private void OnEnable()
    {
        _BlockCollisionAction.PrimaryExplosionl += _BlockPhysics.PrimaryBlockExplosion;
        _BlockCollisionAction.SecondaryExplosionl += _BlockPhysics.SecondaryBlockExplosion;
        _BlockCollision.OnCollsionWithWall += _BlockCollisionAction.BlockCollideWithWall;
        _BlockCollision.OnCollsionWithBall += _BlockCollisionAction.BlockCollideWithBall;
    }
}