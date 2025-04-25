using System.Collections;
using System.Linq;
using UnityEditor.Callbacks;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public float maxShipSpeed, maxShipRotatingSpeed, changingSpeedMod, changingRorationSpeedMod;
    Transform shipTR;
    [SerializeField] Transform hook;
    Rigidbody2D shipRB;

    void Start()
    {
        shipTR = transform;
        shipRB = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        ShipMoving();
        ShipRotating();
    }

    void ShipMoving(){
        float yAx = Input.GetAxisRaw("Vertical");
        shipRB.AddForce(shipTR.up * yAx * changingSpeedMod);
        shipRB.linearVelocityY = Mathf.Clamp(shipRB.linearVelocityY, -maxShipSpeed, maxShipSpeed);
    }

    void ShipRotating(){
        float xAx = Input.GetAxisRaw("Horizontal");
        //shipRB.AddTorque(xAx);
        shipTR.Rotate(0, 0, -xAx * changingRorationSpeedMod);
        //shipTR.rotation *= Quaternion.Euler(0f, 0f, -xAx * changingRorationSpeedMod * Time.fixedDeltaTime);
        // float curZ = shipTR.localEulerAngles.z;
        // curZ = (curZ > 180f) ? curZ - 360f : curZ;
        // shipTR.Rotate(0, 0, curZ);
        shipRB.angularVelocity = Mathf.Clamp(shipRB.angularVelocity, -maxShipRotatingSpeed, maxShipRotatingSpeed);
    }

    void HookPush(){
        
    }

    IEnumerable HookRevealing(){
        foreach(Transform hookPart in hook) 
        {
            yield return new WaitForSeconds(1f);
            hookPart.gameObject.SetActive(true);
            if(hookPart.TryGetComponent<Rigidbody2D>(out Rigidbody2D curRB))
                curRB.AddForce(shipTR.up * changingSpeedMod, ForceMode2D.Impulse);
        }
    }

    IEnumerable HookReturning(){
        for(int i = hook.childCount - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(1f);
            hook.GetChild(i).gameObject.SetActive(false);
        }
    }
}
