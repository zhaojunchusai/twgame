using UnityEngine;
using System.Collections;
//修改模型渲染层级
public class ModelSortingOrder : MonoBehaviour {

    public int sortingOrder = 10;
    void Awake() { renderer.sortingOrder = sortingOrder; }

    IEnumerator Start()
    {
        yield return 1;
        renderer.sortingOrder = sortingOrder;
    }
}
