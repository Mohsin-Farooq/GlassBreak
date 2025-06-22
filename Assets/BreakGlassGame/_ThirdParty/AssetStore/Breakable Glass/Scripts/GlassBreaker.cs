using UnityEngine;
using System.Collections;
using MoreMountains.NiceVibrations;

public class GlassBreaker : MonoBehaviour {
	Vector3 vel;
	BreakGlass g;
    private Rigidbody rb;

    private void Start()
    {
		rb = GetComponent<Rigidbody>();
    } 
    void FixedUpdate () {

		vel = rb.velocity;
	}
	
	void OnCollisionEnter (Collision col) {
		MMVibrationManager.Haptic(HapticTypes.LightImpact);

		if (col.gameObject.GetComponent<BreakGlass>()!=null){
			g = col.gameObject.GetComponent<BreakGlass>();
			GetComponent<Rigidbody>().velocity = vel * g.SlowdownCoefficient;
			
			if(g.BreakByVelocity){
				if(col.relativeVelocity.magnitude >= g.BreakVelocity){
					col.gameObject.GetComponent<BreakGlass>().BreakIt();
					return;	
				}
			}
			
			if(g.BreakByImpulse){
				if(col.relativeVelocity.magnitude * GetComponent<Rigidbody>().mass >= g.BreakImpulse){
					col.gameObject.GetComponent<BreakGlass>().BreakIt();
					return;	
				}
			}
			
		}
	}
}
