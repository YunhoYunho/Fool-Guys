using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    public class Dispenser : MonoBehaviour
    {
        private enum State { Create, Preparing, Ready, Shoot, Rest }


        //================ Prefab =================
        [Header("Setting")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform projectPosition;

        //================ Value =================
        [SerializeField] private float power;
     
        [SerializeField] private float shootingDelay;
        [SerializeField] private float endDelay;
      

        //================ Debug ==================
        [Header("Debug")]
        [SerializeField] private State state;
        [SerializeField] private GameObject curProjectile;
        [SerializeField] private float projectileSize;
        [SerializeField] private float shootingTimer;
        [SerializeField] private float endTimer;


        public void FixedUpdate()
        {
            switch(state) 
            {
                case State.Create:
                    CreateProjectile();
                    break;
                case State.Preparing:
                    PrepareProjectile();
                    break;
                case State.Ready:
                    WaitBeforeShoot();
                    break;
                case State.Shoot:
                    Shoot();
                    break;
                case State.Rest:
                    Rest();
                    break;
                default:
                    break;
            }
        }

        public void CreateProjectile()
        {
            GameObject projectile = Instantiate(projectilePrefab, projectPosition);

            if (projectile.GetComponent<ColorSelector>() != null)
                projectile.GetComponent<ColorSelector>().SetRandomStyle();

            projectile.GetComponent<Rigidbody>().isKinematic = true;
            projectile.transform.localScale = Vector3.zero;

            projectileSize = 0f;
            curProjectile = projectile;
            state = State.Preparing;
        }

        private void PrepareProjectile()
        {
            projectileSize += Time.fixedDeltaTime * 4f;
            curProjectile.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, projectileSize);

            if (projectileSize > 1f)
            {
                shootingTimer = shootingDelay;
                state = State.Ready;
            }
               
        }

        private void WaitBeforeShoot()
        {
            if (shootingTimer > 0f) shootingTimer -= Time.fixedDeltaTime;
            else
            {
                state = State.Shoot;
            }
        }

        private void Shoot()
        {
            curProjectile.transform.parent = null;
            curProjectile.GetComponent<Rigidbody>().isKinematic = false;
            curProjectile.GetComponent<Rigidbody>().AddForce(transform.up * power, ForceMode.VelocityChange);

            endTimer = endDelay;
            state = State.Rest;
        }

        private void Rest()
        {
            if (endTimer > 0f) endTimer -= Time.fixedDeltaTime;
            else
                state = State.Create;
        }
    }
}

