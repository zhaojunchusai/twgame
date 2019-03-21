using UnityEngine;
using System.Collections;

public class ScaleObjBaseOnHeight : MonoBehaviour {

	void Start ()
	{
        float defaultH = GlobalConst.DEFAULT_SCREEN_SIZE_Y;
        float defaultW = GlobalConst.DEFAULT_SCREEN_SIZE_X;
        float factor = ((float)Screen.height / Screen.width) / (defaultH / defaultW);
        transform.localScale = new Vector3(factor, factor, factor);
        
	}

}
