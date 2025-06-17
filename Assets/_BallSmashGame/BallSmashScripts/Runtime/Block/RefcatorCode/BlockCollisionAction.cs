using MoreMountains.NiceVibrations;
using UnityEngine;
using System;
public class BlockCollisionAction : MonoBehaviour
{
    [SerializeField] private BlockPhysics _BlockPhysics;
    public event Action<Vector3> PrimaryExplosionl;
    public event Action<Vector3> SecondaryExplosionl;
    public void BlockCollideWithWall()
    {
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
        Sound_Vibrate("WoodHit_0");
    }
    public void BlockCollideWithBall(Vector3 ballPosition, bool isHit)
    {
        Sound_Vibrate("WoodHit_0");
        if (!isHit)
        {
            PrimaryExplosionl?.Invoke(ballPosition);
        }
        else
        {
            SecondaryExplosionl?.Invoke(ballPosition);
        }
    }
    private void Sound_Vibrate(string soundName)
    {
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
        AudioManager._instance.PlaySound(soundName);
    }
    
}