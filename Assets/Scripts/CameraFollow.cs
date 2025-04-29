using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;       // Player transform
    public Vector3 offset;         // Offset from the target

    void LateUpdate()
    {
        if (target != null)
        {
            // 保持相机与目标的偏移位置
            transform.position = new Vector3(
                target.position.x + offset.x,
                target.position.y + offset.y,
                offset.z
            );
        }
    }
}
