using UnityEngine;

public class TapToPlaceParent : MonoBehaviour
{
    bool placing = false;

    // Called by GazeGestureManager when the user performs a Select gesture
    void OnSelect()
    {
        // On each Select gesture, toggle whether the user is in placing mode.
        placing = !placing;

        // If the user is in placing mode, display the spatial mapping mesh.
      /*  if (placing)
        {
            SpatialMapping.Instance.DrawVisualMeshes = true;
        }
        // If the user is not in placing mode, hide the spatial mapping mesh.
        else
        {
            SpatialMapping.Instance.DrawVisualMeshes = false;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        // If the user is in placing mode,
        // update the placement to match the user's gaze.

        if (placing)
        {

           /* if (this.transform.parent.GetComponent<Rigidbody>())
            {
                Rigidbody rb = this.transform.parent.GetComponent<Rigidbody>();
                Destroy(rb);
            }*/

            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, SpatialMapping.PhysicsRaycastMask))
            {
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.


                

                if (this.transform.parent.gameObject.name == "Sphere1")
                   this.transform.parent.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 0.2f, hitInfo.point.z); //code to drop it 20cm higher than the ground

                else
                    this.transform.parent.position = hitInfo.point;   //original code


                // Rotate this object's parent object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.transform.parent.rotation = toQuat;
            }
        }
       /* else //if (this.transform.parent.gameObject.name == "Sphere1")
        {
            if (!this.transform.parent.GetComponent<Rigidbody>())
            {
                if (this.transform.parent.gameObject.name == "Sphere1")
                {
                    var rigidbody = this.transform.parent.gameObject.AddComponent<Rigidbody>();
                    rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                }
            }
        }*/
    }
}