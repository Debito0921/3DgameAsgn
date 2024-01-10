using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] public float mouseSen;
    private Transform parent;
    private float CamStart = 0;
    private float timeCam = 12.05f;
    private bool canRotate = false;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        CamStart += Time.deltaTime;

        //When animation finish playing, canRotate = true; ( animation play time 12.033s )
        if (CamStart >= timeCam)
            canRotate = true;

        if (canRotate)
            Rotate();
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSen * Time.deltaTime;
        parent.Rotate(Vector3.up, mouseX);
    }
}
