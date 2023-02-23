using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardMouse : MonoBehaviour
{
    [SerializeField]
    private GameInput gameInput;

    [SerializeField]
    private new Camera camera;

    // Start is called before the first frame update
    void Start()
    {
    }

    void FixedUpdate()
    {

    }

    public Quaternion GetMousePosition()
    {
        //get mouse position
        Vector2 mouseScreenPosition = gameInput.GetMousePosition();
        Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(mouseScreenPosition);
        //calculate direction
        Vector3 targetDirection = mouseWorldPosition - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0f, -angle, 0f));
    }




}

