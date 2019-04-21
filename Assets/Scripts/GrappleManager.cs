using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleManager : MonoBehaviour
{
    public Color colorActive;
    public Color colorDefault;
    public float ropeLength;
    public bool isGrappling = false;
    List<GrapplePoint> validGrapplingPoints = new List<GrapplePoint>();
    public GameObject ropeObject;
    public GrapplePoint activeGrapplePoint;

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

            // highlight the grapple point that will be selected
            var newActiveGrapplePoint = GetActiveGrapplePoint();
            if (newActiveGrapplePoint != null) {
                this.activeGrapplePoint = newActiveGrapplePoint;
                this.activeGrapplePoint.GetComponent<SpriteRenderer>().color = colorActive;
            }
        }
        else
        {

        }
    }

    public void UpdateRopeTransform(Vector2 newPlayerPosition)
    {
        Vector2 grapplePointPosition = GetGrapplePointPosition();
        Vector2 ropeDirection = (newPlayerPosition - grapplePointPosition).normalized;
        Vector2 newPos = grapplePointPosition + ropeDirection * this.ropeLength * 0.5f;
        this.ropeObject.transform.position = new Vector3(newPos.x, newPos.y, -10.0f);

        var angle = Vector2.Angle(ropeDirection, -Vector3.up);
        if (Mathf.Sign(Vector2.Dot(ropeDirection, Vector2.right)) < 0)
        {
            angle *= -1f;
        } else {
            angle += 180;
        }
        this.ropeObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    public Vector2 GetGrapplePointPosition() {
        var pos3 = this.activeGrapplePoint.gameObject.transform.position;
        return new Vector2(pos3.x, pos3.y);
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
            
            var playerPos2 = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            
            this.ropeObject.SetActive(true);
            this.ropeLength = (playerPos2 - GetGrapplePointPosition()).magnitude;
            this.ropeObject.transform.localScale = Vector3.one * this.ropeLength;
        }
    }

    void UnGrapple() {
        this.isGrappling = false;
        this.ropeObject.SetActive(false);
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
