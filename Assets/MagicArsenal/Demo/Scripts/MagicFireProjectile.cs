using UnityEngine;
using UnityEngine.EventSystems;

namespace MagicArsenal
{
    public class MagicFireProjectile : MonoBehaviour
    {
        RaycastHit hit;
        public GameObject[] projectiles;
        public Transform spawnPosition;
        [HideInInspector] public int currentProjectile;
        public float speed = 1000;

        //    MyGUI _GUI;
        MagicButtonScript selectedProjectileButton;

        void Start()
        {
            selectedProjectileButton = GameObject.Find("Button").GetComponent<MagicButtonScript>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextEffect();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                NextEffect();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                PreviousEffect();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PreviousEffect();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
                    {
                        GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position,
                            Quaternion.identity) as GameObject;
                        projectile.transform.LookAt(hit.point);
                        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed);
                        projectile.GetComponent<MagicProjectileScript>().impactNormal = hit.normal;
                    }
                }

            }

            Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin,
                Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100, Color.yellow);
        }

        public void NextEffect()
        {
            if (currentProjectile < projectiles.Length - 1)
                currentProjectile++;
            else
                currentProjectile = 0;
            selectedProjectileButton.GetProjectileNames();
        }

        public void PreviousEffect()
        {
            if (currentProjectile > 0)
                currentProjectile--;
            else
                currentProjectile = projectiles.Length - 1;
            selectedProjectileButton.GetProjectileNames();
        }

        public void AdjustSpeed(float newSpeed)
        {
            speed = newSpeed;
        }
    }
}