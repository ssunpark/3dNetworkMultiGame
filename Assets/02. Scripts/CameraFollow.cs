using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0, 5, -7); 
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // 목표 위치 계산
        Vector3 targetPosition = target.position + offset;

        // 부드럽게 따라가기
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // 항상 대상 바라보게 하기 (선택)
        transform.LookAt(target);
    }
}