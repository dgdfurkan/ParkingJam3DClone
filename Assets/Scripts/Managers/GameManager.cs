using DG.Tweening;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;
using GunduzDev;
public class GameManager : MonoBehaviour
{
    #region Self Variables

    #region Public Variables
    /// TODO: Şu an böyle ama daha sonra Level Sisteme bağlı olarak alacağız
    
    public PathCreator myPathCreator;

    #endregion

    #region Private Variables

    private Transform _vehicleParent;
    private byte _vehicleCount;

    #endregion
    
    #endregion

    #region Singleton
    
    public static GameManager Instance;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Debug.Log("GameManager Works");
        
        Application.targetFrameRate = 60;
    }
    
    #endregion

    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioTypes.MusicCityscapes);
    }

    public void NewLevelInit(PathCreator creator, Transform vehicleParent)
    {
        myPathCreator = creator;
        RoadMeshCreator rmc = myPathCreator.gameObject.transform.GetComponent<RoadMeshCreator>();
        rmc.textureTiling +=1;
        rmc.autoUpdate = true;
        _vehicleParent = vehicleParent;
        _vehicleCount = (byte)_vehicleParent.childCount;
    }

    public void UpdateVehicleCount()
    {
        _vehicleCount = (byte)_vehicleParent.childCount;
        if (_vehicleCount is not 0) return;
        Signals.onSFXPlay(AudioTypes.LevelWin);
        VFXManager.Instance.PlayHitEffect(VFXManager.Instance.confettiParticle, VFXManager.Instance.confettiParticle.gameObject.transform.position);
        DOVirtual.DelayedCall(1.5f, () =>
        {
            Signals.onLevelNext?.Invoke();
        });
    }
}