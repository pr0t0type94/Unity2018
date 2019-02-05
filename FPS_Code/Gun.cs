using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour {

    public Camera mainCam;

    public float damage;
    public float range;
    public float bullets;

    public float FireRate;

    public ParticleSystem Gun1Particles;
    public ParticleSystem Gun2Particles;

    private float nextTimeToFire1 = 0f;
    private float nextTimeToFire2 = 0f;

    public int maxAmmo;
    public int currentAmmo1;
    public int currentAmmo2;
    public float reloadTime;
    public Text currAmmo1;
    public Text currAmmo2;

    public Text currCargadoresText;
    public int currCargadores;

    private bool isReloading;


    public Animator gun1Animator;
    public Animator gun2Animator;

    public AudioSource source1;
    public AudioSource source2;
    public AudioSource source3;

    public GameObject Ammo_Icon;
    public GameObject AmmoHolder;

    public GameObject Decal;
    public ParticleSystem ShootHitParticles;
    public GameObject GameController;

    private Enemy target;
    private Diana target_Diana;

    public GameObject bulletsIcon_parentHolder;

    public bool canAttack = true;

    public GameObject objectPoolObject;
    private ObjectPool pool;
    // Use this for initialization
    void Start () {
        currCargadores = 5;
        currCargadoresText.text = currCargadores.ToString();

        currentAmmo1 = maxAmmo;
        currentAmmo2 = maxAmmo;

        currAmmo1.text = currentAmmo1.ToString();
        currAmmo2.text = currentAmmo2.ToString();
        
        pool = new ObjectPool(25, Decal, objectPoolObject.transform);

        

        //SpawnBulletIcons();
    }

    // Update is called once per frame
    void Update () {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R) /*|| currentAmmo1 == 0 && currentAmmo2 == 0*/)
        {
            StartCoroutine(ReloadGuns());
            return;
        }
        

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire1 && currentAmmo1 > 0 && canAttack)
        {
            nextTimeToFire1 = Time.time + 1f / FireRate;
            gun1Animator.SetBool("shooting", true);
            Shoot1();

        }
        else
            gun1Animator.SetBool("shooting", false);


        if (Input.GetButton("Fire2") && Time.time >= nextTimeToFire2 && currentAmmo2 > 0 && canAttack)
        {
            nextTimeToFire2 = Time.time + 1f / FireRate;
            gun2Animator.SetBool("shooting", true);
            Shoot2();

        }
        else
            gun2Animator.SetBool("shooting", false);



        
    }

    void Shoot1()
    {

        currentAmmo1--;

        source1.Play();

        Gun1Particles.Play();

        UpdateAmmoText();


        RaycastHit HitInfo;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out HitInfo, range))
        {
            target = HitInfo.transform.GetComponentInParent<Enemy>();

        }
            if (HitInfo.transform.tag=="Diana")
            target_Diana = HitInfo.transform.GetComponent<Diana>();
        Debug.Log(HitInfo.transform.name);

        if(target != null)
        {
            if(HitInfo.transform.name == "Critical")
            target.TakeDamage(damage,2.5f);
            else
                target.TakeDamage(damage,1.0f);
        }
        if(target_Diana !=null)
        {
            target_Diana.dianaHited();
        }

        CreateShootHitParticles(HitInfo.point, HitInfo.normal);

    }
    void Shoot2()
    {
        currentAmmo2--;

        source2.Play();

        Gun2Particles.Play();

        UpdateAmmoText();

        RaycastHit HitInfo;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out HitInfo, range))
        {
            //target = HitInfo.transform.GetComponent<Enemy>();
            target = HitInfo.transform.GetComponentInParent<Enemy>();

        }
            if (HitInfo.transform.tag == "Diana")
                target_Diana = HitInfo.transform.GetComponent<Diana>();
        //Debug.Log(HitInfo.transform.name);
        //Debug.Log(HitInfo.collider.name);

        if (target != null)
        {
            if (target.tag == "Critical")
            {
                Debug.Log("Critical");
                target.TakeDamage(damage, 2.5f);
            }
            else
                target.TakeDamage(damage, 1.0f);
            //
        }
        if (target_Diana != null)
        {
            target_Diana.dianaHited();
        }

        CreateShootHitParticles(HitInfo.point, HitInfo.normal);


    }
    IEnumerator ReloadGuns()
    {
        isReloading = true;

        gun1Animator.SetBool("Reloading", true);
        gun2Animator.SetBool("Reloading", true);

        source3.Play();

        yield return new WaitForSeconds(reloadTime);

        gun1Animator.SetBool("Reloading", false);
        gun2Animator.SetBool("Reloading", false);

        currCargadores -= 2;

        currentAmmo1 = maxAmmo;
        currentAmmo2 = maxAmmo;

        isReloading = false;

        UpdateAmmoText();

    }

    void UpdateAmmoText()
    {
        currAmmo1.text = currentAmmo1.ToString();
        currAmmo2.text = currentAmmo2.ToString();
        currCargadoresText.text = currCargadores.ToString();
    }


    void CreateShootHitParticles(Vector3 Position, Vector3 Normal)
    {
        
        GameObject.Instantiate(ShootHitParticles, Position, Quaternion.identity);


        //GameObject.Instantiate(Decal, Position, Quaternion.LookRotation(Normal)/*, GameController.m_DestroyObjects*/);
        GameObject obj = pool.GetNextElement();
        obj.transform.position = Position;
        Quaternion.LookRotation(Normal);
        obj.SetActive(true);
    }

    public void AmmoItemCollected()
    {

        currCargadores += 2;
        UpdateAmmoText();

    }

    public Enemy GetTarget()
    {
        return target;
    }
    //void SpawnBulletIcons()
    //{
    //    Vector3 currentPos = AmmoHolder.transform.position;

    //    for (int i = 0; i <= currentAmmo1;i++)
    //    {
    //        GameObject BulletIcon = Instantiate(Ammo_Icon,new Vector3(currentPos.x + i, currentPos.y, currentPos.z), Ammo_Icon.transform.rotation, bulletsIcon_parentHolder.transform);

    //    }
    //}
}
