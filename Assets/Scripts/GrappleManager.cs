using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleManager : MonoBehaviour
{
    bool isGrappling = false;
    List<GrapplePoint> validGrapplingPoints = new List<GrapplePoint>();
    GrapplingPoint activeGrapplePoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {}

    public void ToggleGrapple() {
        if (this.isGrappling) {
            UnGrapple();
        } else {
            Grapple();
        }
    }

    void Grapple() {
        var activeGrapplePoint = GetActiveGrapplePoint();
        if (activeGrapplePoint != null) {
            this.isGrappling = true;
            this.activeGrapplePoint = activeGrapplePoint;
        }
    }

    void UnGrapple() {
        this.isGrappling = false;
    }

    void ComputePlayerPosition() {
    }

    GrapplePoint GetActiveGrapplePoint() {
        if (validGrapplingPoints.Count == 0) {
            return null;
        }

        var playerPosition = gameObject.transform.position;
        var closestDistance = float.MaxValue;
        var closetGrapplePoint = null;
        foreach(GrapplePoint grapplePoint in validGrapplingPoints) {
            var distance = (playerPosition - grapplePoint).magnitude;
            if (distance < closestDistance) {
                closestDistance = distance;
                closestGrapplingPoint = grapplePoint;
            }
        }
        return closetGrapplePoint;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var grapplePoint = col.gameObject.GetComponent<GrapplePoint>();
        if (grapplePoint != null) {
            validGrapplingPoints.Add(grapplePoint);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var grapplePoint = col.gameObject.GetComponent<GrapplePoint>();
        if (grapplePoint != null) {
            validGrapplingPoints.Remove(grapplePoint);
        }
    }
}
