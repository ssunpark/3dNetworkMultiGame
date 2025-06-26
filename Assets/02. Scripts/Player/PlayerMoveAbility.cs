using System;
using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float JumpSpeed = 2.5f;
    public float Gravity = -20f;
    public float RotationSpeed = 2.5f;
    
    private float _ySpeed = 0f;
    private CharacterController _characterController;
    private Animator _animator;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float h =  Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        Vector3 inputDir =  new Vector3(h, 0, v).normalized;
        Vector3 moveDir = transform.TransformDirection(inputDir);
        _animator.SetFloat("Move", inputDir.magnitude);

        if (inputDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
        
        if (_characterController.isGrounded)
        {
            _ySpeed = -1f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _ySpeed = JumpSpeed;
            }
        }
        else
        {
            _ySpeed += Gravity * Time.deltaTime;
        }
        
        moveDir.y = _ySpeed;
        _characterController.Move(moveDir * MoveSpeed * Time.deltaTime);
    }
}
