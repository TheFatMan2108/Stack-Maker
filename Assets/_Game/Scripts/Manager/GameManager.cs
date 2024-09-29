using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour,ISaveable
{
    public static GameManager instance { get; private set; }
    public string KEY_DATA = "34535353453543453453";
    [SerializeField] List<GameObject> levels;
    public DataGame data {  get; set; }
    public int currentLevel { get; private set; } = 2;
    List<IBrickable> brickables;
    List<IWinable> winkables;
    List<ISaveable> saveables;
    private void Awake()
    {
        if (instance == null)instance = this;
        else Destroy(gameObject);
        brickables = new List<IBrickable>();
        winkables = new List<IWinable>();
        saveables = new List<ISaveable>();
        
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
        OnLoad();
    }
    #region Observer
    public void StartBrick(IBrickable b)=>brickables.Add(b);
    public void StartWin(IWinable b)=>winkables.Add(b);
    public void StarSave(ISaveable b)=>saveables.Add(b);
    public void EndBrick(IBrickable b)=>brickables.Remove(b);
    public void EndWin(IWinable b)=>winkables.Remove(b);
    public void EndSave(ISaveable b)=>saveables.Remove(b);
    public void AddBrick()
    {
        foreach (IBrickable item in brickables)
        {
            item.AddBrick();
        }
    }
    public void OnWin()
    {
        foreach (IWinable item in winkables)
        {
            item.OnWin();
        }
        currentLevel++;
        if (currentLevel >= levels.Count) currentLevel = 0;
        OnSave();
    }
    public void OnStop()
    {
        foreach (IWinable item in winkables)
        {
            item.OnStop();
        }
        Onlevel();
    }
    public void RemoveBrick()
    {
        foreach (IBrickable item in brickables)
        {
            item.RemoveBrick();
        }
    }
    public void ClearBrick()
    {
        foreach (IBrickable item in brickables)
        {
            item.ClearBrick();
        }
    }
    public void OnSave()
    {
        foreach (ISaveable item in saveables)
        {
            item.Save();
        }
        Save();
    }
    public void OnLoad()
    {
        Load();
        foreach (ISaveable item in saveables)
        {
            item.Load();
        }
    }
    #endregion 
    public void Onlevel()
    {
        ClearBrick();
        UIManager.instance.SetTextLevel(currentLevel+1);
        Instantiate(levels[currentLevel], Vector3.zero, Quaternion.identity);
    } 
    public void Onlevel(int index)
    {
        UIManager.instance.SetTextLevel(index);
        currentLevel =index-1;
        ClearBrick();
        if (currentLevel >= levels.Count) currentLevel = 0;
        Instantiate(levels[currentLevel], Vector3.zero, Quaternion.identity);
        UIManager.instance.HideUiAll();
    }

    public void Save()
    {
        if (data.leves <= currentLevel)
        {
            data.leves = currentLevel;
        }
       
        string nData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(KEY_DATA,nData);
        Debug.Log(nData);
    }

    public void Load()
    {
       string nData = PlayerPrefs.GetString(KEY_DATA,"");
        if (nData.Equals(""))
        {
            data = new DataGame();
            currentLevel = data.leves;
            return;
        }
        data = JsonUtility.FromJson<DataGame>(nData);
        currentLevel = data.leves;
    }
}
