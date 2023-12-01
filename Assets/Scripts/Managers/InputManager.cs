using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Self Variables

    #region Private Variables
    
    private bool _isSwiped;
    private static LayerMask VehicleLayer => LayerMask.GetMask("Vehicle");
    private RaycastHit _hitData;
    private Ray _ray;
    
    #endregion
    
    #endregion

    private void Update()
    {
        CheckTouchInput(); // Check touch input
    }
    
    private void CheckTouchInput()
    {
        if (Input.touchCount <= 0) return; // If there are no touches, return

        var touch = Input.GetTouch(0); // Get the first touch

        switch (touch.phase)
        {
            case TouchPhase.Began:
                break;
            case TouchPhase.Moved:
                CheckSwipe(touch); // Check for swipe
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Ended:
                ResetSwipe(touch); // Reset the swipe when the touch ends
                break;
            case TouchPhase.Canceled:
                _isSwiped = false; // Reset the swipe when the touch canceled
                break;
            default:
                throw new ArgumentOutOfRangeException(); // Throw an exception for unexpected touch phases
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckSwipe(Touch touch)
    {
        if (!(touch.deltaPosition.magnitude > 3f) || _isSwiped) return;
        
        _isSwiped = true;
        CheckRaycast(touch); // Trigger the finger press event
    }

    private void CheckRaycast(Touch touch)
    {
        if (Camera.main != null)
            _ray = Camera.main.ScreenPointToRay(touch.position); // Convert touch position to a ray in scene

        if (!Physics.Raycast(_ray, out _hitData, 1000, VehicleLayer.value)) return; // Check if the ray hits an object on the "Car" layer
        if (!_hitData.transform.TryGetComponent<VehiclePhysicsController>(out var vehiclePhysicsController)) return; // Try to get the MovableObject component from the hit object
        
        Vehicle vehicle= vehiclePhysicsController.transform.parent.GetComponent<Vehicle>();
        
        // Normalize the swipe delta and convert it to a 3D vector
        var swipeDelta = touch.deltaPosition.normalized;
        var targetDirection = new Vector3(swipeDelta.x, 0, swipeDelta.y);
        
        var forward = vehicle.transform.TransformDirection(Vector3.forward);
        var dot = Vector3.Dot(forward.normalized, targetDirection.normalized);
        vehicle.onMove?.Invoke(dot>0);
    }

    private void ResetSwipe(Touch touch)
    {
        _isSwiped = false;
        Signals.onInputReleased?.Invoke(touch); // Trigger the finger release event
    }
}