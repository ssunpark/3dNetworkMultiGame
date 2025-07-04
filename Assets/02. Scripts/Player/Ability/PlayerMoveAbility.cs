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
        if (!_photonView.IsMine || _owner.IsInputBlocked)
        {
            SyncRemotePlayer();
            return;
        }

        if (!_photonView.IsMine || _owner.IsInputBlocked) return;

        HandleMovement();
    }

    private void SyncRemotePlayer()
    {
        transform.position = Vector3.Lerp(transform.position, _receivedPosition, _ySpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, _receivedRotation, _ySpeed * Time.deltaTime);
    }

    private void HandleMovement()
    {
        float h =  Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        Vector3 inputDir =  new Vector3(h, 0, v).normalized;
        Vector3 moveDir = transform.TransformDirection(inputDir);

        if (inputDir.magnitude > 0.1f)
        {
            RotateTowards(moveDir);
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
                Move(moveDir, _owner.Stat.RunSpeed);
                _owner.SetState(PlayerState.Run);
                _animator.SetTrigger("Run");
            }
            else if (inputDir.magnitude > 0.1f)
            {
                Move(moveDir, _owner.Stat.Movespeed);
                _owner.SetState(PlayerState.Walk);
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

    private void Move(Vector3 direction, float speed)
    {
        _characterController.Move(direction * speed * Time.deltaTime);

    }

    private void RotateTowards(Vector3 direction)
    {   
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _owner.Stat.RotationSpeed * Time.deltaTime);
    }
}
