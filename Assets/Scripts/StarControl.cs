using UnityEngine;

public class StarControl : MonoBehaviour
{
    [SerializeField] float rayDistance, kickForce, maxPowerDistance, curPowerCoef;
    [SerializeField] Transform starTR, arrowTR;
    Rigidbody starRB;
    void Start()
    {
        starRB = starTR.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Ray ry = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ry, out RaycastHit hit, rayDistance))
        {
            Vector3 direction = starTR.position - hit.point;
            direction.Normalize();
            if(Input.GetMouseButton(0)) {
                arrowTR.gameObject.SetActive(true);
                curPowerCoef = direction.magnitude / maxPowerDistance;
                curPowerCoef = curPowerCoef > maxPowerDistance ? 1 : curPowerCoef;
                arrowTR.localScale = new(curPowerCoef, arrowTR.localScale.y, arrowTR.localScale.z);
                arrowTR.position = starTR.position + direction.normalized;
                Vector3 arrowRotation = Vector3.RotateTowards(arrowTR.forward, direction, Time.fixedDeltaTime, 0f);
                arrowTR.rotation = Quaternion.LookRotation(arrowRotation);
            }
            else
                arrowTR.gameObject.SetActive(false);
            if(Input.GetMouseButtonUp(0)) {
                starRB.AddForce(direction * kickForce, ForceMode.Impulse);
            }
        }
    }
}
