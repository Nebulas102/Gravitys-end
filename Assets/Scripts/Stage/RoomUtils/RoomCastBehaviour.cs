using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCastBehaviour : MonoBehaviour
{
    private const float RAYCASTDISTANCE = 1f;

    private void FixedUpdate()
    {
        RayCastsDirectionsStandardRoom();
        RayCastDirectionCheck();
    }

    public void RayCastDirectionCheck()
    {
        Debug.Log("RayCastDirectionCheck");
        Room _room = gameObject.GetComponent<Room>();
        MeshRenderer roomRender = gameObject.GetComponent<MeshRenderer>();

        // if top raycast hit
        // if (Physics.Raycast(_room.topRay, out RaycastHit topHit, RAYCASTDISTANCE))
        if (Physics.Raycast(_room.topRay.origin, transform.TransformDirection(Vector3.forward * RAYCASTDISTANCE), out RaycastHit topHit))
        {
            if (topHit.collider.tag == "Room")
            {
                Debug.Log(_room.id + " top hit " + topHit.transform.gameObject.GetComponent<Room>().id);
                _room.openSides.RemoveAll(x => x == 0);
                topHit.transform.gameObject.GetComponent<Room>().openSides.RemoveAll(x => x == 2);

                _room.DisplayOpenSides();
            }
        }

        // if right raycast hit
        if (Physics.Raycast(_room.rightRay.origin, transform.TransformDirection(Vector3.right * RAYCASTDISTANCE), out RaycastHit rightHit))
        {
            if (rightHit.collider.tag == "Room")
            {
                _room.openSides.RemoveAll(x => x == 1);
                Debug.Log(_room.id + " right hit " + rightHit.transform.gameObject.GetComponent<Room>().id);
                rightHit.transform.gameObject.GetComponent<Room>().openSides.RemoveAll(x => x == 3);

                _room.DisplayOpenSides();
            }
        }

        // if bottom raycast hit
        if (Physics.Raycast(_room.bottomRay.origin, transform.TransformDirection(Vector3.back * RAYCASTDISTANCE), out RaycastHit bottomHit))
        {
            if (bottomHit.collider.tag == "Room")
            {
                _room.openSides.RemoveAll(x => x == 2);
                Debug.Log(_room.id + " bottom hit " + bottomHit.transform.gameObject.GetComponent<Room>().id);
                bottomHit.transform.gameObject.GetComponent<Room>().openSides.RemoveAll(x => x == 0);

                _room.DisplayOpenSides();
            }
        }

        // if left raycast hit
        if (Physics.Raycast(_room.leftRay.origin, transform.TransformDirection(Vector3.left * RAYCASTDISTANCE), out RaycastHit leftHit))
        {
            if (leftHit.collider.tag == "Room")
            {
                _room.openSides.RemoveAll(x => x == 3);
                Debug.Log(_room.id + " left hit " + leftHit.transform.gameObject.GetComponent<Room>().id);
                leftHit.transform.gameObject.GetComponent<Room>().openSides.RemoveAll(x => x == 1);

                _room.DisplayOpenSides();
            }
        }
    }

    public void RayCastsDirectionsStandardRoom()
    {
        Debug.Log("RayCastsDirectionsStandardRoom");
        MeshRenderer roomRender = gameObject.GetComponent<MeshRenderer>();
        Room _room = gameObject.GetComponent<Room>();

        // cast ray top
        Vector3 rayPosTop = new Vector3(transform.position.x + (roomRender.bounds.size.x / 2), 0, transform.position.z + (roomRender.bounds.size.z / 2));
        _room.topRay = new Ray(rayPosTop, transform.TransformDirection(Vector3.forward * (RAYCASTDISTANCE + roomRender.bounds.size.z / 2)));
        Debug.DrawRay(rayPosTop, transform.TransformDirection(Vector3.forward * (RAYCASTDISTANCE + roomRender.bounds.size.z / 2)), Color.green);

        // cast ray right
        Vector3 rayPosRight = new Vector3(transform.position.x + (roomRender.bounds.size.x / 2), 0, transform.position.z + (roomRender.bounds.size.z / 2));
        _room.rightRay = new Ray(rayPosRight, transform.TransformDirection(Vector3.right * (RAYCASTDISTANCE + roomRender.bounds.size.x / 2)));
        Debug.DrawRay(rayPosRight, transform.TransformDirection(Vector3.right * (RAYCASTDISTANCE + roomRender.bounds.size.x / 2)), Color.blue);

        // cast ray bottom
        Vector3 rayPosBottom = new Vector3(transform.position.x + (roomRender.bounds.size.x / 2), 0, transform.position.z + (roomRender.bounds.size.z / 2));
        _room.bottomRay = new Ray(rayPosBottom, transform.TransformDirection(Vector3.back * (RAYCASTDISTANCE + roomRender.bounds.size.z / 2)));
        Debug.DrawRay(rayPosBottom, transform.TransformDirection(Vector3.back * (RAYCASTDISTANCE + roomRender.bounds.size.z / 2)), Color.yellow);

        // cast ray left
        Vector3 rayPosLeft = new Vector3(transform.position.x + (roomRender.bounds.size.x / 2), 0, transform.position.z + (roomRender.bounds.size.z / 2));
        _room.leftRay = new Ray(rayPosLeft, transform.TransformDirection(Vector3.left * (RAYCASTDISTANCE + roomRender.bounds.size.x / 2)));
        Debug.DrawRay(rayPosLeft, transform.TransformDirection(Vector3.left * (RAYCASTDISTANCE + roomRender.bounds.size.x / 2)), Color.red);
    }
}
