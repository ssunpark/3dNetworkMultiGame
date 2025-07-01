using Unity.Cinemachine;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance;
    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        if (_impulseSource == null) Debug.LogError("CinemachineImpulseSource 컴포넌트를 찾을 수 없습니다.");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Shake()
    {
        if (_impulseSource != null) _impulseSource.GenerateImpulse();
    }
}