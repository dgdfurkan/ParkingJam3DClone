using System.Collections.Generic;
using Cinemachine;
using PathCreation;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Self Variables

    #region Serialized Variables
    
    [SerializeField] private Transform levelHolder;
    [SerializeField] private Transform roadMeshHolder;

    #endregion

    #region Private Variables
    
    private LevelData _levelData;
    private byte _totalLevelCount;
    private byte _levelID;
    private List<Transform> _roadMeshList = new List<Transform>();

    #endregion

    #endregion
    
    #region SubscribeEvents and UnsubscribeEvents
    
    private void OnEnable() => SubscribeEvents();
    
    private void SubscribeEvents()
    {
        Signals.onLevelInitialize += OnLevelInitialize;
        Signals.onLevelClear += OnLevelClear;
        Signals.onGetLevelValue += OnGetActiveLevelCount;
        Signals.onLevelNext += OnLevelNext;
        Signals.onLevelRestart += OnLevelRestart;
    }

    private void UnsubscribeEvents()
    {
        Signals.onLevelInitialize -= OnLevelInitialize;
        Signals.onLevelClear -= OnLevelClear;
        Signals.onGetLevelValue -= OnGetActiveLevelCount;
    }
    
    private void OnDisable() => UnsubscribeEvents();
    
    #endregion

    private void Awake()
    {
        Transform[] children = roadMeshHolder.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            _roadMeshList.Add(child);
            child.gameObject.SetActive(false);
        }
        _roadMeshList[0].gameObject.SetActive(true);
        _roadMeshList.RemoveAt(0);
        _totalLevelCount = (byte)Resources.LoadAll("Prefabs/LevelPrefabs", typeof(GameObject)).Length;
        _levelID = GetActiveLevel();
    }

    //private LevelData GetLevelData() => Resources.Load<CD_Level>("Data/CD_Level");
    
    private byte GetActiveLevel()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            return (byte)PlayerPrefs.GetInt("Level");
        }
        return 0;
    }
    
    private void Start()
    {
        Signals.onLevelInitialize?.Invoke((byte)(_levelID % _totalLevelCount));
    }
    
    private void OnLevelInitialize(byte level)
    {
        _roadMeshList[level].gameObject.SetActive(true);
        GameObject go = Instantiate(Resources.Load<GameObject>($"Prefabs/LevelPrefabs/Level {level}"), levelHolder);
        GameManager.Instance.NewLevelInit(go.GetComponentInChildren<PathCreator>(), go.GetComponentInChildren<Vehicle>().gameObject.transform.parent);
    }
    
    private void OnLevelClear()
    {
        foreach (Transform child in _roadMeshList)
        {
            child.gameObject.SetActive(false);
        }
        if (levelHolder.childCount > 0) Destroy(levelHolder.GetChild(0).gameObject);
    }
    
    private byte OnGetActiveLevelCount() => (byte)(_levelID % _totalLevelCount);
    
    private void OnLevelNext()
    {
        _levelID++;
        Signals.onLevelClear?.Invoke();
        Signals.onReset?.Invoke();
        Signals.onLevelInitialize?.Invoke((byte)(_levelID % _totalLevelCount));
    }

    private void OnLevelRestart()
    {
        Signals.onLevelClear?.Invoke();
        Signals.onReset?.Invoke();
        Signals.onLevelInitialize?.Invoke((byte)(_levelID % _totalLevelCount));
    }
}