using UnityEngine;
using System.Collections.Generic;

public class BreakGlass : MonoBehaviour {
	public List<GameObject> BrokenGlassGO; // The broken glass GameObject
	GameObject BrokenGlassInstance; 
	public bool BreakSound=true;
	public GameObject SoundEmitter; //An object that will emit sound
	public float SoundEmitterLifetime=2.0f;
	public float ShardsLifetime=2.0f; //Lifetime of shards in seconds (0 if you don't want shards to disappear)
	public float ShardMass=0.5f; //Mass of each shard
	public Material ShardMaterial;
	
	public bool BreakByVelocity=true;
	public float BreakVelocity=2.0f;
	
	public bool BreakByImpulse=false;
	public float BreakImpulse=2.0f; // Impulse of rigidbody Impulse = Mass * Velocity.magnitude
	
	public bool BreakByClick=false;
	
	public float SlowdownCoefficient=0.6f; // Percent of speed that hitting object has after the hit 


	[SerializeField] private BottleManager bottleManager;

	void Start()
	{	
		bottleManager.RegisterBottle(this);
	}
	private void Smashed()
    {     
		bottleManager.BottleSmashed(this);		
	}
	/*
	/ If you want to break the glass call this function ( myGlass.SendMessage("BreakIt") )
	*/
	//public void BreakIt(){

	//	Smashed();

	//	BrokenGlassInstance = Instantiate(BrokenGlassGO[Random.Range(0,BrokenGlassGO.Count)],this.transform.position, transform.rotation) as GameObject;

	//	BrokenGlassInstance.transform.localScale = transform.lossyScale;

	//	foreach(Transform t in BrokenGlassInstance.transform){
	//		t.GetComponent<Renderer>().material = ShardMaterial;
	//		t.GetComponent<Rigidbody>().mass=ShardMass;


	//	}

	//	if (BreakSound) Destroy(Instantiate(SoundEmitter, transform.position, transform.rotation) as GameObject, SoundEmitterLifetime);

	//	//if(ShardsLifetime>0) Destroy(BrokenGlassInstance,ShardsLifetime);

	////	Destroy(gameObject);
	//	gameObject.SetActive(false); 
	//}



	public void BreakIt()
	{
		// Trigger smashed logic (animations, effects, etc.)
		Smashed();

		// Instantiate the broken glass prefab
		BrokenGlassInstance = Instantiate(BrokenGlassGO[Random.Range(0, BrokenGlassGO.Count)], this.transform.position, transform.rotation) as GameObject;
		BrokenGlassInstance.transform.localScale = transform.lossyScale;

		// Explosion parameters
		float explosionForce = 0f; // Adjust this to control the spread of the shards
		float explosionRadius = 0f;  // Adjust this to control the radius of the explosion
		float upwardsModifier = 0f; // Adds vertical lift to the shards

		// Apply explosion force to each shard
		foreach (Transform shard in BrokenGlassInstance.transform)
		{
			Rigidbody shardRb = shard.GetComponent<Rigidbody>();

			if (shardRb != null)
			{
				// Apply explosion force from the center of the broken glass
				shardRb.AddExplosionForce(
					explosionForce,            // Force magnitude
					transform.position,        // Explosion origin
					explosionRadius,           // Explosion radius
					upwardsModifier           // Upward force modifier
					//ForceMode.Impulse          // Type of force applied
				);
			}
		}

		
		if (BreakSound)
			Destroy(Instantiate(SoundEmitter, transform.position, transform.rotation) as GameObject, SoundEmitterLifetime);

		
		//if (ShardsLifetime > 0)
		//	Destroy(BrokenGlassInstance, ShardsLifetime);

		// Deactivate the original object
		gameObject.SetActive(false);
	}

	void OnMouseDown () {
		if(BreakByClick) BreakIt();	
	}
}
