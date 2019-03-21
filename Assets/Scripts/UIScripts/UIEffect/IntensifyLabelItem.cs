using UnityEngine;
using System.Collections;

public class IntensifyLabelItem: MonoBehaviour
{
    [HideInInspector]public UILabel Lbl_label;
    float lifeTime = 3f;

    void Awake()
    {
        Initialize();
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }

    public void Initialize()
    {
        Lbl_label = transform.FindChild("Label").gameObject.GetComponent<UILabel>();
    }

    public void UpdateItem(string Lab)
    {

        if (Lbl_label == null)
        {
            Initialize();
        }
        Lbl_label.text = Lab;
    }
    void  OnDisable ()
    {
        if(gameObject != null)
        {
            Destroy(this.gameObject);
        }

    }
       
}
