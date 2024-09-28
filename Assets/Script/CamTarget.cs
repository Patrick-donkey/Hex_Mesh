using UnityEngine;

public class CamTarget : MonoBehaviour
{
    private void OnMouseDown()
    {
        CamControl.instance.target = transform;
    }
}
