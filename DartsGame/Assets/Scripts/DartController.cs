using Assets.Scripts.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DartController : MonoBehaviour
{
    public float upForce = 0f;
    public float rightForce = 0f;
    public float forwardForce = 2_000f;

    public GameObject wall;
    public Transform notes;

    public bool randomizeThrow = true;

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private bool atStartPosition;

    // TODO: when resuming tomorrow -- mess more with textmeshpro, see if I can get used to its quirks, potentially rewatch:
    // https://www.youtube.com/watch?v=UUjEkLwgueY

    private DartParameterSet dartParams;

    private new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        atStartPosition = true;

        DisableMovement();

        dartParams = DartParameterSet.Defaults;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (atStartPosition)
            {
                ThrowDart();
            }
            else
            {
                DisableMovement();
                ResetPosition();
            }
            atStartPosition = !atStartPosition;
        }
    }

    private bool IsOriginalPosition()
    {
        return startingPosition == transform.position && startingRotation == transform.rotation;
    }// walking out of opening in torg makes player eating stop eating

    private void ThrowDart()
    {
        ResetPosition();
        EnableMovement();
    }

    /*
          Vector3 calcBallisticVelocityVector(Vector3 source, Vector3 target, float angle){
         Vector3 direction = target - source;                            
         float h = direction.y;                                           
         direction.y = 0;                                               
         float distance = direction.magnitude;                           
         float a = angle * Mathf.Deg2Rad;                                
         direction.y = distance * Mathf.Tan(a);                            
         distance += h/Mathf.Tan(a);                                      
 
         // calculate velocity
         float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2*a));
         return velocity * direction.normalized;    
     }
     * */

    private void OnCollisionEnter(Collision collision)
    {
        DisableMovement();

        var renderer = collision.gameObject.GetComponent<Renderer>();
        var test = collision.gameObject.transform.parent == notes;
        if (collision.gameObject.transform.parent == notes)
        {
            StartCoroutine("FlashNoteGreen", renderer);
        }
        if (wall == collision.gameObject)
        {
            StartCoroutine("FlashWallRed", renderer);
        }
    }

    IEnumerator FlashNoteGreen(Renderer renderer)
    {
        var flashCount = 3;
        var delay = 0.25f;
        var startColor = renderer.material.color;
        for (int i = 0; i < flashCount; i++)
        {
            renderer.material.color = Color.green;
            yield return new WaitForSeconds(delay);

            renderer.material.color = startColor;
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator FlashWallRed(Renderer renderer)
    {
        var flashCount = 3;
        var delay = 0.25f;
        for (int i = 0; i < flashCount; i++)
        {
            renderer.material.color = Color.red;
            yield return new WaitForSeconds(delay);

            renderer.material.color = Color.white;
            yield return new WaitForSeconds(delay);
        }
    }

    private void ResetPosition()
    {
        transform.position = startingPosition;
        // transform.rotation = startingRotation;
    }

    private void EnableMovement()
    {
        // TODO: Early on the dart could sometimes still be flying while EnableMovement() was called (shouldn't happen anymore)...
        // But that would cause weird physics behaviors because we need to add relative force to the dart,
        // and these modify that non-zero force value. Deciding our numbers assuming no existing
        // momentum seems much easier when doing it by hand. Might want to revisit this approach, calculating the correct amount
        // of force to add to the dart to get it to our final destination automatically has the widest range of applications.
        StopMovement();

        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;

        ApplyForce();
    }

    private void ApplyForce()
    {
        if (randomizeThrow)
        {
            rigidbody.AddRelativeForce(Vector3.up * dartParams.ForwardForce);
            rigidbody.AddRelativeForce(Vector3.right * UnityEngine.Random.Range(dartParams.UpForceMin, dartParams.UpForceMax));
            rigidbody.AddRelativeForce(Vector3.forward * UnityEngine.Random.Range(dartParams.RightForceMin, dartParams.RightForceMax));
        }
        else
        {
            rigidbody.AddRelativeForce(Vector3.up * forwardForce);
            rigidbody.AddRelativeForce(Vector3.right * upForce);
            rigidbody.AddRelativeForce(Vector3.forward * rightForce);
        }

        // TODO: Need to pick a point along the wall to aim for and then try calculating via CalculateTrajectory() and something like this to aim:
        /*
         * 
         *         public override Quaternion AimRotation(Vector3 start, Vector3 end, Vector3 velocity){
 
            float low;
            //float high;
            Ballistics.CalculateTrajectory (start, end, velocity, out low);//, out high); //get the angle
 
 
            Vector3 wantedRotationVector = Quaternion.LookRotation (end - start).eulerAngles; //get the direction
            wantedRotationVector.x = low; //combine the two
            return Quaternion.Euler (wantedRotationVector); //into a quaternion
        } // from https://forum.unity.com/threads/projectile-trajectory-prediction.664909/
         */
    }

    private void DisableMovement()
    {
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;

        StopMovement();
    }

    private void StopMovement()
    {
        rigidbody.velocity = Vector3.zero;
    }
}