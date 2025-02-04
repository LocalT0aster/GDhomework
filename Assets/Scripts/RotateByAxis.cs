using UnityEngine;

public class RotateByAxis : MonoBehaviour {
    public Vector3 RotateDegPerSecond = new(0f, 0f, 0f);
    void Update(){
        transform.Rotate(RotateDegPerSecond * Time.deltaTime);
    }
}
