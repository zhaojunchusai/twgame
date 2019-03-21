using UnityEngine;
using System.Collections;

public class SceneFitScreen : MonoBehaviour
{
    private float size;
    private Camera _camera;
	// Use this for initialization
	void Start ()
	{
	    _camera = gameObject.GetComponent<Camera>();
	    float factor = ((1024f/576f)/((float) Screen.width/(float) Screen.height));
	    _camera.orthographicSize = _camera.orthographicSize * factor;
	}
}
