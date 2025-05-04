using UnityEngine;

public class ObstacleDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionDistance = 10f;
    public LayerMask floorLayer;
    public LayerMask obstacleLayer;

    [Header("Marker Settings")]
    public GameObject marker;     // ���� Prefab
    GameObject markerInstance;    // ���������ɵ�ʵ��

    [Header("Debug Settings")]
    public bool debugRay = true;

    void Start()
    {
        if (marker != null)
        {
            // �ڳ���������һ��ʵ����������
            markerInstance = Instantiate(marker);
            markerInstance.SetActive(false);
        }
        else
        {
            Debug.LogError("���� Inspector ������ marker Prefab��");
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

            // �������ʵ��λ��
            markerInstance.SetActive(true);
            markerInstance.transform.position = hit.point + hit.normal * 0.01f;
            markerInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            // ��ѡ�����������־
            int hitMask = 1 << hit.collider.gameObject.layer;
            if ((hitMask & floorLayer) != 0) Debug.Log($"����: {hit.collider.name}");
            if ((hitMask & obstacleLayer) != 0) Debug.Log($"�ϰ�: {hit.collider.name}");
        }
        else
        {
            if (debugRay) Debug.DrawRay(origin, dir * detectionDistance, Color.green);
            if (markerInstance != null) markerInstance.SetActive(false);
        }
    }
}
