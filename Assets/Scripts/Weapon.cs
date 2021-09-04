using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera fpCamera;
    [SerializeField] float range = 100.0f;
    [SerializeField] float sedativeStrength = 1.0f;
    [SerializeField] ParticleSystem muzzleBurst;
    [SerializeField] GameObject hitEffect;

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        PlayMuzzleFlash();

        RaycastHit hit;

        if( Physics.Raycast(fpCamera.transform.position, fpCamera.transform.forward, out hit, range))
        {
            CreateHitImpact(hit);

            EnemyAI target = hit.transform.GetComponent<EnemyAI>();

            if(target) { target.Sedate(sedativeStrength); }
        } 
    }

    void PlayMuzzleFlash()
    {
        muzzleBurst.Play();
    }

    void CreateHitImpact(RaycastHit hit)
    {
        GameObject gameobject = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(gameobject, 1.0f);
    }
}
