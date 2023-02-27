using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObstacle : MonoBehaviour
{
    [SerializeField] private Transform hitPoint;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layerMask;

    /// <summary>
    /// 날려버리는 오브젝트에서 힘을 가할 때
    /// 플레이어의 최대 속도 제한을 해제해야 한다.
    /// 플레이어를 미리 랙돌 상태로 만드는 방법으로 최대 속도 제한을 해제해본다.
    /// (애니메이션 프레임에서 동작하는 함수)
    /// </summary>
    public void Hit()
    {
        Collider[] colliders = Physics.OverlapSphere(hitPoint.position, radius, layerMask);
        //Debug.Log(colliders.Length);
        foreach (Collider collider in colliders)
        {
            PlayerController pc = collider.GetComponent<PlayerController>();
            if (pc != null)
            {
                //Debug.Log("플레이어 히트");
                pc.OnHit();
            }

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hitPoint.position, radius);
    }
}
