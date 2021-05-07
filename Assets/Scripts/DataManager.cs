using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    static public DataManager instance;
    public int LastStage;

    public bool[] IsHard;
    public bool[] StageClear;
    public int[] StageClearStar;
    public int[] StageHardClearStar;

    public float[] StageScore;

    public bool[] IsLockOff;

    public float VolumeSettingValue;
    public float SoundSettingValue;
    public int SelectedChar;
    public GameObject PAUSEDPanel;


    public Slider SoundSlider;
    public Slider MusicSlider;
    
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;

    public float[,] StarTimer;
    public float[,] StarHardTimer;

    public bool IsKorean;
    public Toggle KoreanTog;
    public Toggle EngTog;

    public bool IsTuto;

    public Vector3[] CameraStartPos = new[] { new Vector3(1, 1, 1),
        new Vector3(1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1),
        new Vector3(1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1),
        new Vector3(1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1) };

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        color1 = new Color(57 / 255f, 91 / 255f, 183 / 255f);
        color2 = new Color(30 / 255f, 130 / 255f, 76 / 255f);
        color3 = new Color(255 / 255f, 197 / 255f, 73 / 255f);
        color4 = new Color(192 / 255f, 57 / 255f, 43 / 255f);
        SelectedChar = 1;
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }

        StageClear = new bool[13];
        IsHard = new bool[13];
        IsLockOff = new bool[13];
        StageClearStar = new int[25];
        StageHardClearStar = new int[25];
        StageScore = new float[25];
        if (ES3.KeyExists("LastStage") == false)
        {
            SetDefaultSetting();
            SaveData();
        }
        else
        {
            LoadData();
        }
    }

   

    // Start is called before the first frame update
    void Start()
    {
        SelectedChar = 1;

        StarTimer = new float[50, 3];
        StarHardTimer = new float[50, 3];

        StarTimer[0, 2] = 920627;
        StarTimer[0, 1] = 920627;
        StarTimer[0, 0] = 920627;

        StarTimer[1, 2] = 90;
        StarTimer[1, 1] = 150;
        StarTimer[1, 0] = 360;

        StarTimer[2, 2] = 150;  
        StarTimer[2, 1] = 210;
        StarTimer[2, 0] = 420;

        StarTimer[3, 2] = 360;
        StarTimer[3, 1] = 420;
        StarTimer[3, 0] = 630;

        StarTimer[4, 2] = 300;
        StarTimer[4, 1] = 360;
        StarTimer[4, 0] = 570;

        StarTimer[5, 2] = 270;
        StarTimer[5, 1] = 330;
        StarTimer[5, 0] = 540;

        StarTimer[6, 2] = 270;
        StarTimer[6, 1] = 330;
        StarTimer[6, 0] = 540;
                          
        StarTimer[7, 2] = 300;
        StarTimer[7, 1] = 360;
        StarTimer[7, 0] = 570;
                          
        StarTimer[8, 2] = 330;
        StarTimer[8, 1] = 390;
        StarTimer[8, 0] = 600;
                          
        StarTimer[9, 2] = 390;
        StarTimer[9, 1] = 450;
        StarTimer[9, 0] = 660;
                          
        StarTimer[10, 2] =300;
        StarTimer[10, 1] =360;
        StarTimer[10, 0] =570;
                          
        StarTimer[11, 2] =940;
        StarTimer[11, 1] =940;
        StarTimer[11, 0] =940;
                          
        StarTimer[12, 2] =940;
        StarTimer[12, 1] =940;
        StarTimer[12, 0] =940;
        
        
        StarHardTimer[0, 2] = 920627;
        StarHardTimer[0, 1] = 920627;
        StarHardTimer[0, 0] = 920627;
            
        StarHardTimer[1, 2] = 60;
        StarHardTimer[1, 1] = 90;
        StarHardTimer[1, 0] = 350;
            
        StarHardTimer[2, 2] = 100;
        StarHardTimer[2, 1] = 130;
        StarHardTimer[2, 0] = 390;
            
        StarHardTimer[3, 2] = 180;
        StarHardTimer[3, 1] = 210;
        StarHardTimer[3, 0] = 470;
            
        StarHardTimer[4, 2] = 130;
        StarHardTimer[4, 1] = 160;
        StarHardTimer[4, 0] = 420;
            
        StarHardTimer[5, 2] = 100;
        StarHardTimer[5, 1] = 130;
        StarHardTimer[5, 0] = 390;
            
        StarHardTimer[6, 2] = 130;
        StarHardTimer[6, 1] = 160;
        StarHardTimer[6, 0] = 420;
            
        StarHardTimer[7, 2] = 220;
        StarHardTimer[7, 1] = 250;
        StarHardTimer[7, 0] = 510;
            
        StarHardTimer[8, 2] = 220;
        StarHardTimer[8, 1] = 250;
        StarHardTimer[8, 0] = 510;
            
        StarHardTimer[9, 2] = 250;
        StarHardTimer[9, 1] = 280;
        StarHardTimer[9, 0] = 540;
            
        StarHardTimer[10, 2] = 210;
        StarHardTimer[10, 1] = 240;
        StarHardTimer[10, 0] = 500;
            
        StarHardTimer[11, 2] = 940;
        StarHardTimer[11, 1] = 940;
        StarHardTimer[11, 0] = 940;
            
        StarHardTimer[12, 2] = 940;
        StarHardTimer[12, 1] = 940;
        StarHardTimer[12, 0] = 940;


        if (IsKorean) KoreanTog.isOn = true;
        else EngTog.isOn = true;
        IsTuto = false;
    }
    private void SetDefaultSetting()
    {
        LastStage = 1;
        VolumeSettingValue = 1;
        SoundSettingValue = 1;
        SelectedChar = 1;
        IsKorean = true;
        for (int i = 0; i < StageClear.Length; i++)
            StageClear[i] = false;

        for (int i = 0; i < StageClearStar.Length; i++)
            StageClearStar[i] = 0;  
        
        for (int i = 0; i < StageHardClearStar.Length; i++)
            StageHardClearStar[i] = 0;        

        for (int i = 0; i < IsHard.Length; i++)
            IsHard[i] = false;

        for (int i = 0; i < IsLockOff.Length; i++)
            IsLockOff[i] = false;

        for (int i = 0; i < StageScore.Length; i++)
            StageScore[i] = 0;
    }

    public void SaveData()
    {
        ES3.Save<int>("LastStage", LastStage);
        ES3.Save<bool[]>("StageClear", StageClear);
        ES3.Save<bool[]>("IsHard", IsHard);
        ES3.Save<bool[]>("IsLockOff", IsLockOff);
        ES3.Save<bool>("IsKorean", IsKorean);
        ES3.Save<int[]>("StageClearStar", StageClearStar);
        ES3.Save<int[]>("StageHardClearStar", StageHardClearStar);
        ES3.Save<float[]>("StageScore", StageScore);
        ES3.Save<float>("VolumeSettingValue", VolumeSettingValue);
        ES3.Save<float>("SoundSettingValue", SoundSettingValue);
    }
    public void LoadData()
    {
        if(ES3.KeyExists("LastStage") == false)
            ES3.Save<int>("LastStage", LastStage);
        LastStage = ES3.Load<int>("LastStage");

        if (ES3.KeyExists("StageClear") == false)
            ES3.Save<bool[]>("StageClear", StageClear);
        StageClear = ES3.Load<bool[]>("StageClear");

        if (ES3.KeyExists("IsHard") == false)
            ES3.Save<bool[]>("IsHard", IsHard);
        IsHard = ES3.Load<bool[]>("IsHard");

        if (ES3.KeyExists("IsLockOff") == false)
            ES3.Save<bool[]>("IsLockOff", IsLockOff);
        IsLockOff = ES3.Load<bool[]>("IsLockOff");

        if (ES3.KeyExists("IsKorean") == false)
            ES3.Save<bool>("IsKorean", IsKorean);
        IsKorean = ES3.Load<bool>("IsKorean");        

        if (ES3.KeyExists("StageClearStar") == false)
            ES3.Save<int[]>("StageClearStar", StageClearStar);
        StageClearStar = ES3.Load<int[]>("StageClearStar");

        if (ES3.KeyExists("StageHardClearStar") == false)
            ES3.Save<int[]>("StageHardClearStar", StageHardClearStar);
        StageHardClearStar = ES3.Load<int[]>("StageHardClearStar");

        if (ES3.KeyExists("StageScore") == false)
            ES3.Save<float[]>("StageScore", StageScore);
        StageScore = ES3.Load<float[]>("StageScore");

        if (ES3.KeyExists("VolumeSettingValue") == false)
            ES3.Save<float>("VolumeSettingValue", VolumeSettingValue);
        VolumeSettingValue = ES3.Load<float>("VolumeSettingValue");
        MusicSlider.value = VolumeSettingValue;

        if (ES3.KeyExists("SoundSettingValue") == false)
            ES3.Save<float>("SoundSettingValue", SoundSettingValue);
        SoundSettingValue = ES3.Load<float>("SoundSettingValue");
        SoundSlider.value = SoundSettingValue;        
    }

    public void SettingExit(GameObject SettingPanel)
    {
        SoundManager.instance.Play(2);
        SettingPanel.SetActive(false);
    }

    public void SettingKorean(Toggle toggle)
    {
        SoundManager.instance.Play(2);

        if (toggle.isOn)
            IsKorean = true;
    }
    public void SettingEnglish(Toggle toggle)
    {
        SoundManager.instance.Play(2);

        if (toggle.isOn)
            IsKorean = false;
    }
}
