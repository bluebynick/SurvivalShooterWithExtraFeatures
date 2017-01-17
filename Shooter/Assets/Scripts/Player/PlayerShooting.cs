using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    public Slider[] sliders;
    public Slider bulletsSlider;
    public PlayerHealth playerHealth;


    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
    int bullets = 20;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
        sliders = Component.FindObjectsOfType<Slider>();
        foreach (Slider s in sliders)
        {
            if (s.name == "BulletsSlider")
            {
                bulletsSlider = s;
            }
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            if(bullets > 0)
            {
                Shoot();
                bullets--;
                bulletsSlider.value = bullets;
            }
            else
            {
            }
            if (Input.GetButton("Reload")) //needs to be here too incase mouse is beiing held down
            {
                bullets = 20;
                bulletsSlider.value = bullets;
            }
        }

        if (Input.GetButton("Reload"))
        {
            bullets = 20;
            bulletsSlider.value = bullets;
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true; //turn on the line renderer of the gun
        gunLine.SetPosition (0, transform.position); //this is creating the first end of the point (the 0 being the begining of the line)

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward; //setting the direction directly in front of the player

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> (); //this gets the health script
            if(enemyHealth != null) //if it the hit thing has a script
            {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }
            gunLine.SetPosition (1, shootHit.point); //this is making the second point of the line (designated by 1) that has a position at the shootHit.point
        }
        else  //this gives us the ability to shoot even if we're not hitting anything
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range); //this is drawing a line b/w the origin and the point at the range, 1 cause it's the second point, the second part of the line, the end
        }
    }
}
