using System;
using UnityEngine;
using DG.Tweening;
using GunduzDev;
using PathCreation.Examples;
using UnityEngine.Serialization;

public class VehiclePhysicsController : MonoBehaviour
{
    #region Self Variables

    #region Public Variables

    public PathFollower myPathFollower;
    public bool isOnPath = false;

    #endregion
    
    #region Serialized Variables

    [SerializeField] private Vehicle vehicle;
    [SerializeField] private Collider myCollider;

    #endregion

    #region Private

    private VehiclePhysicData _physicData;
    private bool _sBarrierOpen = false;

    #endregion
    
    #endregion
    
    internal void SetData(VehiclePhysicData physicData) => _physicData = physicData;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Path") && !isOnPath) CollisionPath(other); // Collision with path

        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Vehicle")) // Collision with obstacle or vehicle
        {
            if (other.gameObject.transform.TryGetComponent<VehiclePhysicsController>(out var vehiclePhysicsController))
            {
                if(vehiclePhysicsController.isOnPath) return;
            }
            
            var collisionPoint = other.bounds.ClosestPoint(vehicle.transform.position);
            var collisionDirection = collisionPoint - vehicle.transform.position;
            var dot = Vector3.Dot(vehicle.transform.forward.normalized, collisionDirection.normalized);
            
            VFXManager.Instance.PlayHitEffect(VFXManager.Instance.hitParticle ,collisionPoint + new Vector3(0,1,0));
            
            other.gameObject.transform.DOKill(true);
            other.gameObject.transform.DOPunchRotation(Vector3.right * 9f, .15f, 5, .5f);
            
            if(!isOnPath) vehicle.StopMovement(); 
            
            Signals.onSFXPlay(AudioTypes.HitObstacle);
            
            HandleCollision(collisionDirection.normalized, dot); 

            Camera.main.transform.DOKill(true);
            
            Camera.main.transform.DOShakePosition(_physicData.CameraShake.x, _physicData.CameraShake.y);
            Camera.main.transform.DOShakeRotation(_physicData.CameraShake.x, _physicData.CameraShake.y);
        }

        if (other.gameObject.CompareTag("Barrier")) 
        {
            if(!_sBarrierOpen) OpenBarrier(other.gameObject); // Collision with barrier means finish line
        }
        
        if (other.gameObject.CompareTag("Finish"))
        {
            VFXManager.Instance.PlayHitEffect(VFXManager.Instance.barrierParticle, VFXManager.Instance.barrierParticle.gameObject.transform.position);
            Signals.onSFXPlay(AudioTypes.Barrier);
            myCollider.enabled = false; 
            Destroy(gameObject.transform.parent.gameObject, 1f);
            DOVirtual.DelayedCall(1.05f, () =>
            {
                GameManager.Instance.UpdateVehicleCount();
            });
        }
    }
    
    private void CollisionPath(Collider other)
    {
        if (!other.gameObject.CompareTag("Path")) return;
        
        Signals.onSFXPlay(AudioTypes.OnPath);
        
        var collisionPosition = GameManager.Instance.myPathCreator.path.GetClosestPointOnPath(vehicle.transform.position);
        float collisionPoint = GameManager.Instance.myPathCreator.path.GetClosestDistanceAlongPath(collisionPosition);
        var collisionRotation = GameManager.Instance.myPathCreator.path.GetRotationAtDistance(collisionPoint);
        var collisionY = new Quaternion(0, collisionRotation.y, 0, collisionRotation.w);

        myPathFollower.distanceTravelled = collisionPoint;
        
        vehicle.transform.DORotateQuaternion(collisionY, .4f).SetEase(Ease.Linear);
        
        vehicle.transform.DOMove(collisionPosition, .4f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                myPathFollower.isOnRoad = true;
                isOnPath = true;
            });
    }
    
    private void HandleCollision(Vector3 direction, float dotProduct)
    {
        DOTween.Kill(vehicle.transform, true);
        
        var rotation = vehicle.transform.InverseTransformDirection(direction);
        (rotation.x, rotation.z) = (-rotation.z, rotation.x);
        
        vehicle.transform.DOPunchRotation(rotation * 7f, 0.3f, 8).SetId(vehicle.transform);

        if (CheckIsHitFromSides(dotProduct)) return;
        
        var moveDirection = vehicle.transform.forward * MathF.Sign(-dotProduct);

        vehicle.transform.DOLocalMove(vehicle.transform.localPosition + moveDirection * 0.3f, 0.2f)
            .SetId(vehicle.transform)
            .SetUpdate(UpdateType.Fixed);
    }
    
    private bool CheckIsHitFromSides(float dotProduct)
    {
        const float tresHold = 0.9f;
        if (dotProduct < 0) return dotProduct > -tresHold;
        return dotProduct < tresHold;
    }
    
    private void OpenBarrier(GameObject other)
    {
        _sBarrierOpen = true;
        other.transform.GetComponent<BoxCollider>().enabled = false;
        other.transform.DOKill(true);
        other.transform.DOLocalRotate(_physicData.BarrierOpenParameters, .75f,RotateMode.LocalAxisAdd)
            .SetRelative(true)
            .SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                other.transform.DOKill(true);
            });
    }
}