using UnityEngine;
using System.Collections;

public class RichTextClickEvent : MonoBehaviour {

    public void OnClick() {
        UILabel lbl = GetComponent<UILabel>();
        string url = lbl.GetUrlAtPosition( UICamera.lastHit.point );
        RichTextProcessing processing = transform.parent.GetComponent<RichTextProcessing>();
        processing.ClickHyperLinks( url );
    }
}
