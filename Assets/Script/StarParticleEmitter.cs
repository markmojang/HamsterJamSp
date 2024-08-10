using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParticleEmitter : MonoBehaviour
{
    public ParticleSystem starParticleSystem;
    public Transform cameraTransform;
    [SerializeField] private float emissionRadius = 20f;
    [SerializeField] private float partipersec = 60f;
    private bool spawning = false;
    private float delay;

    void Start(){
        delay = 1 / partipersec;
        
    }
    void Update()
    {

        if(!spawning){
            StartCoroutine(EmitStarsAroundCamera());
        }
    }

    IEnumerator EmitStarsAroundCamera()
    {
        spawning = true;
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.position = cameraTransform.position + Random.insideUnitSphere * emissionRadius;
        starParticleSystem.Emit(emitParams, 1);
        yield return new WaitForSeconds(delay);
        spawning = false;
    }
}
