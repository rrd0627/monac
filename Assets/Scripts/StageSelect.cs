using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;

public class StageSelect : MonoBehaviour
{
    public int CurStage;

    public Image[] MapSprite;
    public Image[] MapHardSprite;

    public Text StageNumText;

    public Transform MapSelectPos;

    private const int SpaceBetweenMapAndMap = 1300;

    private bool IsClicked;

    public ScrollRectFaster Scroll;

    public GameObject StartButt;

    public bool[] IsHard;
    public GameObject HardButtonImage;
    public GameObject HardLockImage;
    public GameObject HardTitleImage;
    public GameObject HardNormalTextImage;
    public GameObject HardBackGround;

    public GameObject HardShine;

    public GameObject LockPrefab;
    private GameObject LockPrefab_temp;

    public GameObject[] Stars;

    public GameObject TutorialPanel;

    public GameObject NormalToHardEffect;
    private GameObject NormalToHardEffect_prefab;

    public Text ScoreText;
    private string timeStr;
    private int min;
    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        IsClicked = false;            

        IsHard = new bool[DataManager.instance.IsHard.Length];
        for(int i=0;i< DataManager.instance.IsHard.Length;i++)
        {
            IsHard[i] = false;
        }

        CurStage = DataManager.instance.LastStage;

        MapSelectPos.localPosition = -Vector2.right * (CurStage-1) * 1300;

        for(int i=0;i< MapSprite.Length;i++)
        {
            if(DataManager.instance.StageClear[i])
            {
                MapSprite[i].gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            }            
        }
    }
    private void OnEnable()
    {
        IsClicked = false;

        IsHard = new bool[DataManager.instance.IsHard.Length];
        for (int i = 0; i < DataManager.instance.IsHard.Length; i++)
        {
            IsHard[i] = false;
        }

        CurStage = DataManager.instance.LastStage;

        MapSelectPos.localPosition = -Vector2.right * (CurStage - 1) * 1300;

        for (int i = 0; i < MapSprite.Length; i++)
        {
            if (DataManager.instance.StageClear[i])
            {
                MapSprite[i].gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            }
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {            
            SoundManager.instance.Play(2);

            DataManager.instance.LastStage = CurStage;

            StageSelectPrefab.instance.gameObject.SetActive(false);
            CharSelectPrefab.instance.gameObject.SetActive(true);
        }

        StageNumText.text = "              " + CurStage.ToString();

        if(CurStage != (int)((-MapSelectPos.localPosition.x + SpaceBetweenMapAndMap / 2f) / 1300) + 1)
        {
            SoundManager.instance.Play(3);
        }

        CurStage = (int)((-MapSelectPos.localPosition.x + SpaceBetweenMapAndMap / 2f) / 1300) + 1;

        
        if (DataManager.instance.StageClear[CurStage - 1] && CurStage != 12 && CurStage != 11)
        {//전스테이지 깼으면            
            MapSprite[CurStage - 1].color = Color.white;
            StartButt.SetActive(true);
        }
        else
        {
            //MapSprite[CurStage - 1].color = Color.black;
            StartButt.SetActive(false);
            HardButtonImage.GetComponent<Image>().color = Color.black;
            HardTitleImage.SetActive(false);
            MapSprite[CurStage-1].enabled = true;
            MapHardSprite[CurStage - 1].enabled = false;
        }

        if(DataManager.instance.StageClearStar[CurStage]==3)
        {
            MapHardSprite[CurStage - 1].color = Color.white;
        }

        for (int i = 0; i < MapSprite.Length; i++)
        {
            if (i == CurStage - 1) continue;

            if (DataManager.instance.StageClear[i])            
                MapSprite[i].color = Color.gray;                           
            else
                MapSprite[i].color = Color.black;


            if (DataManager.instance.StageClearStar[i+1]==3)
                MapHardSprite[i].color = Color.gray;
            else
                MapHardSprite[i].color = Color.black;

            
        }

        if (DataManager.instance.StageClear[CurStage])
        {//이번스테이지 깼으면
            if(DataManager.instance.StageClearStar[CurStage]==3)
            { //그 스테이지가 3성이면
                Stars[0].SetActive(true);
                Stars[1].SetActive(true);
                Stars[2].SetActive(true);
                if (DataManager.instance.IsLockOff[CurStage])
                {//자물쇠 풀었으면
                    HardLockImage.SetActive(false);
                    IsHard[CurStage] = DataManager.instance.IsHard[CurStage];
                    if (DataManager.instance.IsHard[CurStage])
                    {
                        HardTitleImage.SetActive(true);
                        StartButt.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Hard");
                        HardNormalTextImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/HardText");
                        HardNormalTextImage.GetComponent<Image>().color = Color.white;
                        HardButtonImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/HardSword");
                        HardButtonImage.GetComponent<Image>().color = Color.white;
                        HardBackGround.SetActive(true);

                        MapSprite[CurStage - 1].enabled = false;
                        MapHardSprite[CurStage - 1].enabled = true;


                        if (DataManager.instance.StageHardClearStar[CurStage] == 3)
                        {
                            Stars[0].SetActive(true);
                            Stars[1].SetActive(true);
                            Stars[2].SetActive(true);
                        }
                        else if(DataManager.instance.StageHardClearStar[CurStage] == 2)
                        {
                            Stars[0].SetActive(true);
                            Stars[1].SetActive(true);
                            Stars[2].SetActive(false);
                        }
                        else if(DataManager.instance.StageHardClearStar[CurStage] == 1)
                        {
                            Stars[0].SetActive(true);
                            Stars[1].SetActive(false);
                            Stars[2].SetActive(false);
                        }
                        else
                        {
                            Stars[0].SetActive(false);
                            Stars[1].SetActive(false);
                            Stars[2].SetActive(false);
                        }
                    }
                    else
                    {
                        HardTitleImage.SetActive(false);
                        StartButt.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Normal");
                        HardNormalTextImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/NormalText");
                        HardNormalTextImage.GetComponent<Image>().color = Color.white;
                        HardButtonImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/NormalSword");
                        HardButtonImage.GetComponent<Image>().color = Color.white;
                        HardBackGround.SetActive(false);

                        MapSprite[CurStage - 1].enabled = true;
                        MapHardSprite[CurStage - 1].enabled = false;
                    }
                }
                else
                {//자물쇠 안풀었으면  풀릴것처럼 막!
                    HardLockImage.SetActive(true);
                    HardShine.SetActive(true);
                    HardTitleImage.SetActive(false);
                    StartButt.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Normal");
                    HardNormalTextImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/NormalText");
                    HardNormalTextImage.GetComponent<Image>().color = Color.black;
                    HardButtonImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/NormalSword");
                    HardButtonImage.GetComponent<Image>().color = Color.black;
                    HardBackGround.SetActive(false);

                    MapSprite[CurStage - 1].enabled = true;
                    MapHardSprite[CurStage - 1].enabled = false;
                }
            }
            else if(DataManager.instance.StageClearStar[CurStage] == 2)
            {//그 스테이지가 2성이면
                HardButtonImage.GetComponent<Image>().color = Color.black;

                HardToNormal();

                Stars[0].SetActive(true);
                Stars[1].SetActive(true);
                Stars[2].SetActive(false);
            }
            else
            {//그 스테이지가 1성이면
                HardButtonImage.GetComponent<Image>().color = Color.black;

                HardToNormal();

                Stars[0].SetActive(true);
                Stars[1].SetActive(false);
                Stars[2].SetActive(false);
            }
        }
        else
        {//못깼으면
            HardToNormal();

            StartButt.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Normal");
            HardNormalTextImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/NormalText");
            HardNormalTextImage.GetComponent<Image>().color = Color.black;
            HardButtonImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/NormalSword");
            HardButtonImage.GetComponent<Image>().color = Color.black;
            HardBackGround.SetActive(false);


            Stars[0].SetActive(false);
            Stars[1].SetActive(false);
            Stars[2].SetActive(false);
        }


        if (Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                IsClicked = true;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                IsClicked = false;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            IsClicked = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            IsClicked = false;
        }
        if(IsClicked && Scroll.velocity.magnitude >= 1500)
        {
            StageNumText.enabled = false;
            StageNumText.transform.localScale = Vector3.one * 0.5f;
        }
        if(!IsClicked && Scroll.velocity.magnitude < 1500)
        {
            StageNumText.text = "              " + CurStage.ToString();

            StageNumText.enabled = true;

            MapSelectPos.localPosition = Vector2.Lerp(MapSelectPos.localPosition, -Vector2.right * (CurStage - 1) * 1300, Time.deltaTime * 10);
            if (!DataManager.instance.StageClear[CurStage])
            {//못깻으면
                HardToNormal();
            }

            if (StageNumText.transform.localScale.x<1f)
                StageNumText.transform.localScale *= 1.1f;            

            if(DataManager.instance.IsHard[CurStage])
            {
                min = (int)DataManager.instance.StageScore[CurStage+12] / 60;
                timer = DataManager.instance.StageScore[CurStage+12] - 60 * min;

                timeStr = min.ToString("00") + ":" + timer.ToString("00.00");
                ScoreText.text = timeStr;
            }
            else
            {
                min = (int)DataManager.instance.StageScore[CurStage] / 60;
                timer = DataManager.instance.StageScore[CurStage] - 60 * min;

                timeStr = min.ToString("00") + ":" + timer.ToString("00.00");
                ScoreText.text = timeStr;
            }

            
        }

        if(MapSprite[CurStage -1].transform.localScale.x <=1.2f)
            MapSprite[CurStage - 1].transform.localScale *= 1.01f;
        for(int i=0;i<MapSprite.Length;i++)
        {
            if (i == CurStage - 1) continue;
            if (i == 10 || i == 11) continue;

            if (MapSprite[i].transform.localScale.x > 0.9f)
                MapSprite[i].transform.localScale *= 0.99f;
        }
    }
    public void StartButton()
    {
        SoundManager.instance.Play(2);

        DataManager.instance.LastStage = CurStage;
        StageSelectPrefab.instance.gameObject.SetActive(false);
        CharSelectPrefab.instance.gameObject.SetActive(true);
        //SceneManager.LoadScene("CharacterSelect");


        /*
        AsyncOperation oper = new AsyncOperation();
        oper = SceneManager.LoadSceneAsync("CharacterSelect");
        oper.allowSceneActivation = true;*/
    }

    public void BackButton()
    {
        SoundManager.instance.Play(2);

        BGMManager.instance.Play(1);
        BGMManager.instance.FadeInMusic();

        TitlePrefab.instance.gameObject.SetActive(true);
        StageSelectPrefab.instance.gameObject.SetActive(false);
        CharSelectPrefab.instance.gameObject.SetActive(false);
        /*
        AsyncOperation oper = new AsyncOperation();
        oper = SceneManager.LoadSceneAsync("Title");
        oper.allowSceneActivation = true;
        */
    }

    public void HardButton()
    {
        SoundManager.instance.Play(2);
        
        if(IsHard[CurStage])
        {
            HardToNormal();
        }
        else
        {
            NormalToHard();
        }
    }
    public void HardToNormal()
    {
        IsHard[CurStage] = false;
        DataManager.instance.IsHard[CurStage] = false;

        HardTitleImage.SetActive(false);
        HardLockImage.SetActive(true);
        HardShine.SetActive(false);
        MapSprite[CurStage - 1].enabled = true;
        MapHardSprite[CurStage - 1].enabled = false;
        StartButt.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Normal");

        HardNormalTextImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/NormalText");

        HardButtonImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/NormalSword");

        HardBackGround.SetActive(false);

    }
    public void NormalToHard()
    {
        IsHard[CurStage] = true;
        DataManager.instance.IsHard[CurStage] = true;

        HardTitleImage.SetActive(true);
        StartButt.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Hard");

        HardNormalTextImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/HardText");
        HardNormalTextImage.GetComponent<Image>().color = Color.white;
        HardButtonImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/HardSword");
        HardButtonImage.GetComponent<Image>().color = Color.white;
        HardBackGround.SetActive(true);
        MapSprite[CurStage - 1].enabled = false;
        MapHardSprite[CurStage - 1].enabled = true;
        NormalToHardEffect_prefab = Instantiate(NormalToHardEffect, HardButtonImage.transform.position , Quaternion.identity);
        Destroy(NormalToHardEffect_prefab, 1f);
    }

    public void TutorialCancel()
    {
        SoundManager.instance.Play(2);
        TutorialPanel.SetActive(false);
    }

    public void TutorialClick()
    {
        SoundManager.instance.Play(2);
        TutorialPanel.SetActive(true);
    }

    public void Tutorial()
    {
        TutorialPanel.SetActive(false);

        DataManager.instance.IsTuto = true;

        AsyncOperation oper = new AsyncOperation();

        oper = SceneManager.LoadSceneAsync("Loading");

        oper.allowSceneActivation = true;
    }

    public void ClickLock()
    {
        if(DataManager.instance.StageClearStar[CurStage]==3)
        {
            LockPrefab_temp = Instantiate(LockPrefab, HardLockImage.transform.position, Quaternion.identity);
            SoundManager.instance.Play(8);
            LockPrefab_temp.SetActive(true);
            DataManager.instance.IsLockOff[CurStage] = true;
            DataManager.instance.SaveData();
        }      
    }


    public void RankButtonClick()
    {
        if (CurStage >= 11)
            return;


        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate(AuthenticateHandler);
    }
    void AuthenticateHandler(bool isSuccess)
    {
        string leaderBoardId = "CgkIu8CaxfkWEAIQAQ";

        if(IsHard[CurStage])
        {
            switch (CurStage)
            {
                case 1:
                    leaderBoardId = "CgkIu8CaxfkWEAIQCw";
                    break;
                case 2:
                    leaderBoardId = "CgkIu8CaxfkWEAIQDA";
                    break;
                case 3:
                    leaderBoardId = "CgkIu8CaxfkWEAIQDQ";
                    break;
                case 4:
                    leaderBoardId = "CgkIu8CaxfkWEAIQDg";
                    break;
                case 5:
                    leaderBoardId = "CgkIu8CaxfkWEAIQDw";
                    break;
                case 6:
                    leaderBoardId = "CgkIu8CaxfkWEAIQEA";
                    break;
                case 7:
                    leaderBoardId = "CgkIu8CaxfkWEAIQEQ";
                    break;
                case 8:
                    leaderBoardId = "CgkIu8CaxfkWEAIQEg";
                    break;
                case 9:
                    leaderBoardId = "CgkIu8CaxfkWEAIQEw";
                    break;
                case 10:
                    leaderBoardId = "CgkIu8CaxfkWEAIQFA";
                    break;
            }
        }
        else
        {
            switch (CurStage)
            {
                case 1:
                    leaderBoardId = "CgkIu8CaxfkWEAIQFg";
                    break;
                case 2:
                    leaderBoardId = "CgkIu8CaxfkWEAIQFw";
                    break;
                case 3:
                    leaderBoardId = "CgkIu8CaxfkWEAIQGA";
                    break;
                case 4:
                    leaderBoardId = "CgkIu8CaxfkWEAIQGQ";
                    break;
                case 5:
                    leaderBoardId = "CgkIu8CaxfkWEAIQGg";
                    break;
                case 6:
                    leaderBoardId = "CgkIu8CaxfkWEAIQGw";
                    break;
                case 7:
                    leaderBoardId = "CgkIu8CaxfkWEAIQHA";
                    break;
                case 8:
                    leaderBoardId = "CgkIu8CaxfkWEAIQHQ";
                    break;
                case 9:
                    leaderBoardId = "CgkIu8CaxfkWEAIQHg";
                    break;
                case 10:
                    leaderBoardId = "CgkIu8CaxfkWEAIQHw";
                    break;

            }
        }
        


        if(isSuccess)
        {
            float highSocre;
            if (IsHard[CurStage])
            {
                highSocre = DataManager.instance.StageScore[CurStage+12]*1000;                
            }
            else
            {
                highSocre = DataManager.instance.StageScore[CurStage]*1000;
            }
            if (highSocre == 0)
            {
                PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderBoardId);
                return;
            }

            Social.ReportScore((long)highSocre, leaderBoardId, (bool success) =>
            {
                 if (success)
                 {
                     PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderBoardId);
                 }
                 else
                 {
                     //upload highsocre fail
                 }
            });
        }
        else
        {
            //login fail
        }
     
    }

}
