using System;
using GunduzDev;
using UnityEngine;
using UnityEngine.Events;

public static class Signals
{
    // #region Singleton
    //
    // public static Signals Instance;
    //
    // private void Awake()
    // {
    //     if (Instance != null && Instance != this)
    //     {
    //         Destroy(gameObject);
    //         return;
    //     }
    //
    //     Instance = this;
    //     Debug.Log("Signals Works");
    // }
    //
    // #endregion

    #region Input Signals

    public static UnityAction<Touch> onInputTaken = delegate { }; // Event triggered when a finger touches the screen
    public static UnityAction<Touch> onInputReleased = delegate { }; // Event triggered when a finger is released the screen
    public static UnityAction<Touch> onInputDragged = delegate { };
    
    public static UnityAction onEnableInput = delegate { };
    public static UnityAction onDisableInput = delegate { };
    
    #endregion

    #region Core Game Signals

    public static UnityAction onReset = delegate {  };
    public static UnityAction<Vector3, Vector3, float, GameObject> onCollision = delegate(Vector3 arg0, Vector3 vector3, float f, GameObject arg3) {  }; 
    
    #endregion

    #region Level Signals

    public static UnityAction<byte> onLevelInitialize = delegate {  };
    public static UnityAction onLevelClear = delegate {  };
    public static UnityAction onLevelNext = delegate {  };
    public static UnityAction onLevelRestart = delegate {  };
    public static Func<byte> onGetLevelValue = delegate { return 0; };

    #endregion

    #region Audio Signals

    public static UnityAction<AudioTypes> onMusicPlay = delegate {  };
    public static UnityAction onMusicStop = delegate {  };
    public static UnityAction<AudioTypes> onSFXPlay = delegate {  };

    #endregion
}