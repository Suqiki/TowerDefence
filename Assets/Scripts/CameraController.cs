using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool doMovement = true;
    
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public float minY= 10f;
    public float maxY = 80f;
    public float minX = -50;
    public float maxX = 50;
    public float minZ = -50;
    public float maxZ = 50;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            doMovement = !doMovement;

        if (!doMovement)
            return;
        
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        //Debug.Log(scroll);
        
        Vector3 pos = transform.position;
        pos.y -= scroll * 1000 * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        
        transform.position = pos;
    }
}
