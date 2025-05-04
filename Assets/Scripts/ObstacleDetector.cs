using UnityEngine;

public class ObstacleDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionDistance = 10f;
    public LayerMask floorLayer;
    public LayerMask obstacleLayer;

    [Header("Marker Settings")]
    public GameObject marker;     // 拖入 Prefab
    GameObject markerInstance;    // 场景中生成的实例

    [Header("Debug Settings")]
    public bool debugRay = true;

    void Start()
    {
        if (marker != null)
        {
            // 在场景中生成一份实例，并隐藏
            markerInstance = Instantiate(marker);
            markerInstance.SetActive(false);
        }
        else
        {
            Debug.LogError("请在 Inspector 中拖入 marker Prefab！");
        }
    }

    void Update()
    {
        Vector3 origin = transform.position;
        Vector3 dir = transform.forward;
        int combinedMask = floorLayer | obstacleLayer;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, detectionDistance, combinedMask))
        {
            if (debugRay) Debug.DrawLine(origin, hit.point, Color.red);

            // 激活并更新实例位置
            markerInstance.SetActive(true);
            markerInstance.transform.position = hit.point + hit.normal * 0.01f;
            markerInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            // 可选：区分输出日志
            int hitMask = 1 << hit.collider.gameObject.layer;
            if ((hitMask & floorLayer) != 0) Debug.Log($"地面: {hit.collider.name}");
            if ((hitMask & obstacleLayer) != 0) Debug.Log($"障碍: {hit.collider.name}");
        }
        else
        {
            if (debugRay) Debug.DrawRay(origin, dir * detectionDistance, Color.green);
            if (markerInstance != null) markerInstance.SetActive(false);
        }
    }
}
