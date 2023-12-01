using UnityEngine;
using DG.Tweening;
using PathCreation;
using PathCreation.Examples;

public class VehicleMovementController : MonoBehaviour
{
    #region Self Variables

    #region Private Variables

    private VehicleMovementData _movementData;
    private Vector3 _moveDirection;
    private float _currentSpeed;
    private bool _isReadyToPlay, _isReadyToMove;

    #endregion

    #endregion
    
    internal void SetData(VehicleMovementData movementData) => _movementData = movementData;
    
    private void FixedUpdate()
    {
        if(_isReadyToMove) MovePark();
    }
    
    private void MovePark()
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, _movementData.VehicleSpeed, Time.fixedDeltaTime * _movementData.VehicleAcceleration); // Smoothly increase the speed of the object towards the maximum speed
        transform.position += _moveDirection * (Time.fixedDeltaTime * _currentSpeed); // Move the object in the current direction with the calculated speed
    }
    
    private void ReadyToMove() => _isReadyToMove = true;
    
    private void SetDirection(Vector3 direction) => _moveDirection = direction;
    
    public void ReadyToStop()
    {
        _currentSpeed = 0;
        _isReadyToMove = false;
    }
    
    public void Move(Vector3 direction)
    {
        DOTween.Kill(transform, true);

        var rotation = transform.InverseTransformDirection(direction);
        (rotation.x, rotation.z) = (-rotation.z, rotation.x);

        transform.DOPunchRotation(rotation * 3f, .4f, 8).SetId(transform);

        direction.y = 0;
        
        SetDirection(direction);
        ReadyToMove();
    }
}