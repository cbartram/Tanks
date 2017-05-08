using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float startingHealth = 100f;          
    public Slider slider;                        
    public Image fillImage;                      
    public Color fullHealthColor = Color.green;  
    public Color zeroHealthColor = Color.red;    
    public GameObject explosionPrefab;   
	public GameManager tankManager;

	public bool collectedPowerup = false;
    
    private AudioSource explosionAudio;          
    private ParticleSystem explosionParticles;   
    private float currentHealth;  
    private bool dead; 


    private void Awake()
    {
        explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
        explosionAudio = explosionParticles.GetComponent<AudioSource>();
	
        explosionParticles.gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        currentHealth = startingHealth;
        dead = false;

        SetHealthUI();
    }

    public void TakeDamage(float amount)
    {
		// Reduce current health by the amount of damage done.
		currentHealth -= amount;

		// Change the UI elements appropriately.
		SetHealthUI();

		// If the current health is at or below zero and it has not yet been registered, call OnDeath.
		if (currentHealth <= 0f && !dead)
		{
			OnDeath();
		}
    }

	void OnTriggerEnter(Collider collision)
	{

		if (collision.gameObject.name == "Heart(Clone)") {
			Destroy (collision.gameObject);

			if (currentHealth > 70f) {
				//Back to full health
				currentHealth = 100f;
				SetHealthUI ();

			} else {

				//Add 30% health
				currentHealth += 30f;
				SetHealthUI ();
			}
		}


		//Collecting a powerup
		if (collision.gameObject.name == "RocketPowerup(Clone)") {

			collectedPowerup = true;
			Destroy (collision.gameObject);
		}


	}


    private void SetHealthUI()
    {
		// Set the slider's value appropriately.
		slider.value = currentHealth;

		// Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
		fillImage.color = Color.Lerp (zeroHealthColor, fullHealthColor, currentHealth / startingHealth);
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
		// Set the flag so that this function is only called once.
		dead = true;

		// Move the instantiated explosion prefab to the tank's position and turn it on.
		explosionParticles.transform.position = transform.position;
		explosionParticles.gameObject.SetActive (true);

		// Play the particle system of the tank exploding.
		explosionParticles.Play ();

		// Play the tank explosion sound effect.
		explosionAudio.Play();

		// Turn the tank off.
		gameObject.SetActive (false);
    }
}