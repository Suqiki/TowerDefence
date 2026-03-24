using UnityEngine;

public class Mat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 scale = transform.localScale;
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(scale.x, scale.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
