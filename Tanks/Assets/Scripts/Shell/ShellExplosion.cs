using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask tankMask;
    public ParticleSystem explosionParticles;       
    public AudioSource explosionAudio;    
    public float maxDamage = 100f;                  
    public float explosionForce = 1000f;            
    public float maxLifeTime = 2f;                  
    public float explosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }
		

	// Find all the tanks in an area around the shell and damage them.
    private void OnTriggerEnter(Collider other)
    {
		// Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
		Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius, tankMask);

		// Go through all the colliders...
		for (int i = 0; i < colliders.Length; i++)
		{
			// ... and find their rigidbody.
			Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody> ();

			// If they don't have a rigidbody, go on to the next collider.
			if (!targetRigidbody)
				continue;

			// Add an explosion force.
			targetRigidbody.AddExplosionForce (explosionForce, transform.position, explosionRadius);

			// Find the TankHealth script associated with the rigidbody.
			TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth> ();

			// If there is no TankHealth script attached to the gameobject, go on to the next collider.
			if (!targetHealth)
				continue;

			// Calculate the amount of damage the target should take based on it's distance from the shell.
			float damage = CalculateDamage (targetRigidbody.position);

			// Deal this damage to the tank.
			targetHealth.TakeDamage (damage);
		}

		// Unparent the particles from the shell.
		explosionParticles.transform.parent = null;

		// Play the particle system.
		explosionParticles.Play();

		// Play the explosion sound effect.
		explosionAudio.Play();

		// Once the particles have finished, destroy the gameobject they are on.
		Destroy (explosionParticles.gameObject, explosionParticles.duration);

		// Destroy the shell.
		Destroy (gameObject);
    }
		
	// Calculate the amount of damage a target should take based on it's position.
    private float CalculateDamage(Vector3 targetPosition)
    {
		// Create a vector from the shell to the target.
		Vector3 explosionToTarget = targetPosition - transform.position;

		// Calculate the distance from the shell to the target.
		float explosionDistance = explosionToTarget.magnitude;

		// Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
		float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

		// Calculate damage as this proportion of the maximum possible damage.
		float damage = relativeDistance * maxDamage;

		// Make sure that the minimum damage is always 0.
		damage = Mathf.Max (0f, damage);


		Debug.Log ("Damage Done: " + damage);
		Debug.Log ("Damage Done without boost: " + (relativeDistance * 100f)); 


		return damage;
    }
}