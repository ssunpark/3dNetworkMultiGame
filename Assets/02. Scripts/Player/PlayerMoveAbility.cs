using System;
using Photon.Pun;
using UnityEngine;

public class PlayerMoveAbility : PlayerAbility, IPunObservable
{
    public float Gravity = -20f;
    // public bool IsMove =true;
    
    private float _ySpeed = 0f;
    private CharacterController _characterController;
    private Animator _animator;
    
    private Vector3 _receivedPosition = Vector3.zero;
    private Quaternion _receivedRotation = Quaternion.identity;
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (_photonView.IsMine && stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if (_photonView.IsMine == false && stream.IsReading)
        {
            Debug.Log("수신중");
            _receivedPosition = (Vector3)stream.ReceiveNext();
            _receivedRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void Update()
    {
        float h =  Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        if (_photonView.IsMine == false)
        {
            transform.position = Vector3.Lerp(transform.position, _receivedPosition, _ySpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, _receivedRotation, _ySpeed * Time.deltaTime);
            return;
        }
        
        Vector3 inputDir =  new Vector3(h, 0, v).normalized;
        Vector3 moveDir = transform.TransformDirection(inputDir);

        if (inputDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _owner.Stat.RotationSpeed * Time.deltaTime);
        }
        
        
        if (_characterController.isGrounded)
        {
            _ySpeed = -1f;

            if (Input.GetKeyDown(KeyCode.Space) && (_owner.Stat.CurrentStamina >= _owner.Stat.JumpStamina))
            {
                _owner.SetState(PlayerState.Jump);
                _ySpeed = _owner.Stat.JumpPower;
            }
            
            else if (Input.GetKey(KeyCode.LeftShift) && (inputDir.magnitude > 0.1f) && (_owner.Stat.CurrentStamina > 0f))
            {
                _owner.SetState(PlayerState.Run);
                _characterController.Move(moveDir * _owner.Stat.RunSpeed * Time.deltaTime);
                _animator.SetTrigger("Run");
            }
            else if (inputDir.magnitude > 0.1f)
            {
                _owner.SetState(PlayerState.Walk);
                _characterController.Move(moveDir * _owner.Stat.Movespeed * Time.deltaTime);
            }
            else
            {
                _owner.SetState(PlayerState.Idle);
            }
        }
        else
        {
            _ySpeed += Gravity * Time.deltaTime;
        }
        
        moveDir.y = _ySpeed;
        _characterController.Move(moveDir * Time.deltaTime);
        _animator.SetFloat("Move", inputDir.magnitude);
    }
}
