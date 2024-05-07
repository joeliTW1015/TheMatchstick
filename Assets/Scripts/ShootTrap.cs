using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTrap : MonoBehaviour
{
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject arrow;
    public void ShootArrow()
    {
        Vector3 shootDir = new Vector3(0, 0, 180 + (transform.eulerAngles.z % 360));
        //Quaternion shootDir = Quaternion.Euler(-transform.eulerAngles);
        Instantiate(arrow, shootPoint.position, Quaternion.Euler(shootDir));
    }
}
