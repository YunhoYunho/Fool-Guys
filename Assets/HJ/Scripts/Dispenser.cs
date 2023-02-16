using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    public class Dispenser : ControlableObstacle
    {
        private enum State { Idle, Create, Preparing, Ready, Shoot, Rest }


        //================ Prefab =================
        [Header("Setting")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform projectPosition;

        //================ Value =================
        [SerializeField] private float power = 30f;
     
        [SerializeField] private float shootingDelay;
        [SerializeField] private float endDelay;
      
        //================ Debug ==================
        [Header("Debug")]
        [SerializeField] private State state;
        [SerializeField] private GameObject curProjectile;
        [SerializeField] private float projectileSize;
        [SerializeField] private float shootingTimer;
        [SerializeField] private float endTimer;

        //================ Photon =================
        private PhotonView pv;


        public void ReadyToCreateDetect()
        {
            pv.RPC("ReadyToCreateBall", RpcTarget.All);
        }


        [PunRPC]
        public void ReadyToCreateBall()
        {
            state = State.Create;

        }


        private void Start()
        {
            pv = GetComponent<PhotonView>();
        }


        public void FixedUpdate()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ChangeState();
        }

      

        public void ChangeState()
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Create:
                    pv.RPC("CreateProjectile", RpcTarget.All);
                    //CreateProjectile();
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

        [PunRPC]
        public void CreateProjectile()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, projectPosition.position, projectPosition.rotation);

            projectile.gameObject.GetComponent<PhotonView>().RPC("Initiate", RpcTarget.All);

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

        protected override IEnumerator ControlCoroutine(float duration, float coolTime)
        {
            Debug.Log("발사 속도 빠르게");
            float originalEndDelay = endDelay;
            float originalShootingDelay = shootingDelay;
            endDelay *= 0.5f;
            shootingDelay *= 0.5f;

            yield return new WaitForSeconds(duration);
            endDelay = originalEndDelay;
            shootingDelay = originalShootingDelay;

            yield return new WaitForSeconds(coolTime);
            controlling = null;
        }
    }
}

