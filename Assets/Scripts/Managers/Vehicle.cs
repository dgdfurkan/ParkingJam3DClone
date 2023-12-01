using UnityEngine;
using UnityEngine.Events;
using PathCreation.Examples;

public class Vehicle : MonoBehaviour
{
    #region Self Variables

    #region Public Variables

    public UnityAction<bool> onMove = delegate {  };
    public UnityAction onPathAreaEntered = delegate {  };
    public UnityAction onFinishAreaEntered = delegate {  };
    
    #endregion

    #region Serialized Variables

    [SerializeField] private VehicleMovementController movementController;
    [SerializeField] private PathFollower pathFollower;
    [SerializeField] private VehiclePhysicsController physicsController;
    
    #endregion
    
    #region Private Variables

    private VehicleData _vehicleData;

    #endregion

    #endregion

    private void Awake()
    {
        _vehicleData = GetVehicleData();
        SendDataToControllers();
    }
    
    private VehicleData GetVehicleData() => Resources.Load<CD_Vehicle>("Data/CD_Vehicle").VehicleData;
    
    private void SendDataToControllers()
    {
        movementController.SetData(_vehicleData.MovementData);
        pathFollower.SetData(_vehicleData.MovementData);
        physicsController.SetData(_vehicleData.PhysicData);
    }

    #region SubscribeEvents and UnsubscribeEvents
    
    private void OnEnable() => SubscribeEvents();
    
    private void SubscribeEvents()
    {
        onMove += OnMove;
    }
    
    private void UnsubscribeEvents()
    {
        onMove -= OnMove;
    }
    
    private void OnDisable() => UnsubscribeEvents();
    
    #endregion

    #region Vehicle Move Core
    
    public void StopMovement() => movementController.ReadyToStop();
    
    private void OnMove(bool isForward) => movementController.Move(isForward ? transform.forward : -transform.forward);
    
    #endregion
    
}