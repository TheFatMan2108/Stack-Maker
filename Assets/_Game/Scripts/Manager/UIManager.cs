using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour,ISaveable
{
    public static UIManager instance { get; private set; }
    [SerializeField] Button btnPlayMain, btnExitMain,btnPlayAgainInGame,btnExitInGame,btnMenuInGame,btnCloseInGame,btnMute;
    [SerializeField] GameObject uiSelectLevels,uiMenu,uiLevels,uiInGame,uiMute;
    [SerializeField] TextMeshProUGUI txtLevel;
    [SerializeField] Sprite mute,unMute;
    [SerializeField] AudioSource bgMusic;
    private bool isMute;
    private List<Button> levels = new List<Button>();

    private void Awake()
    {
        if (instance == null)instance = this;
        else Destroy(instance);
    }
    void Start()
    {
        GameManager.instance.StarSave(this);
    }
    public void SetTextLevel(int level)
    {
        txtLevel.text = string.Format("Level {0}",level);
    }
    private void OnInit()
    {
        
        btnPlayMain.onClick.AddListener(ShowUISelectLevels);
        btnCloseInGame.onClick.AddListener(HideUiAll);
        btnMenuInGame.onClick.AddListener(ShowUIInGame);
        btnPlayAgainInGame.onClick.AddListener(PlayAgain);
        btnExitInGame.onClick.AddListener(ExitMainMenu);
        btnMute.onClick.AddListener(()=>SetMute(isMute));
        for (int i = 0; i < uiLevels.transform.childCount; i++)
        {
            Button button = uiLevels.transform.GetChild(i).GetComponent<Button>();
            button.interactable = false;
            int.TryParse( uiLevels.transform.GetChild(i).name,out int index);
            button.onClick.AddListener(()=> GameManager.instance.Onlevel(index));

        }
        for (int i = 0; i < GameManager.instance.currentLevel+1; i++)
        {
            uiLevels.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }
    public void SetMute(bool mute)
    {
        isMute=!mute;
        if(isMute)
        {
            uiMute.GetComponent<Image>().sprite = this.mute;
        }
        else
        {
            uiMute.GetComponent<Image>().sprite = this.unMute;
        } 
        bgMusic.mute = isMute;
        GameManager.instance.OnSave();
    }
    public void PlayAgain()
    {
        HideUiAll();
        GameManager.instance.OnStop();
    }
    public void ExitMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ShowUIInGame()
    {
        uiInGame.gameObject.SetActive(true);
    }
    public void HideUiAll()
    {

        uiInGame.gameObject.SetActive(false);
        uiMenu.SetActive(false);    
        uiSelectLevels.SetActive(false);
    }
    public void ShowUISelectLevels()
    {
        if(uiSelectLevels.activeInHierarchy) uiSelectLevels.SetActive(false);
        else uiSelectLevels.SetActive(true);
    }

    public void Save()
    {
        GameManager.instance.data.isMuste = isMute;
    }

    public void Load()
    {
        OnInit();
        SetMute(!GameManager.instance.data.isMuste);

    }
}
