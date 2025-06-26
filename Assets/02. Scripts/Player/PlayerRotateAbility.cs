using System;
using UnityEngine;

public class PlayerRotateAbility : MonoBehaviour
{
    // 목표: 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶다.
    public Transform CameraRoot;
    public float RotationSpeed = 100;

    // 마우스 입력값을 누적할 변수
    private float _mx;
    private float _my;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update()
    {
        // 1. 마우스 입력받기
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        _mx += mouseX * RotationSpeed * Time.deltaTime;
        _my += mouseY * RotationSpeed * Time.deltaTime;
        
        _my = Mathf.Clamp(_my, -90f, 90f);
        
        // - 캐릭터
        //  ㄴ 카메라 루트
        
        // y축 회전은 캐릭터만 한다.
        transform.eulerAngles = new Vector3(0f, _mx, 0f);
        
        // x축 회전은 캐릭터는 하지 않는다. (즉, 카메라 루트만 x축 회전하면 된다.)
        CameraRoot.localEulerAngles = new Vector3(-_my, 0f, 0f);
    }
}
