using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleManager : MonoBehaviour
{
    public Color colorActive;
    public Color colorDefault;
    public bool isGrappling = false;
    List<GrapplePoint> validGrapplingPoints = new List<GrapplePoint>();
    GrapplePoint activeGrapplePoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() 
    {
        if (!this.isGrappling) {
            if (this.activeGrapplePoint != null) {
                this.activeGrapplePoint.GetComponent<SpriteRenderer>().color = colorDefault;
            }

            var newActiveGrapplePoint = GetActiveGrapplePoint();
            if (newActiveGrapplePoint != null) {
                this.activeGrapplePoint = newActiveGrapplePoint;
                this.activeGrapplePoint.GetComponent<SpriteRenderer>().color = colorActive;
            }
        }
    }

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
            this.activeGrapplePoint.GetComponent<SpriteRenderer>().color = colorActive;
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
        GrapplePoint closestGrapplePoint = null;
        foreach(GrapplePoint grapplePoint in validGrapplingPoints) {
            var distance = (playerPosition - grapplePoint.gameObject.transform.position).magnitude;
            if (distance < closestDistance) {
                closestDistance = distance;
                closestGrapplePoint = grapplePoint;
            }
        }
        return closestGrapplePoint;
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
