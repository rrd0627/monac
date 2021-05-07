using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Loading : MonoBehaviour
{
    public Image progressBar;

    public Image LoadingImage;

    public Sprite[] Images;

    public Image panel;

    //public Text ready_text;
    public Text StageText;

    private int rand_num;
    public Text Info_Title;
    public Text Info_Text;

    
    //int flag;

    Color _color;
    //Color text_color;


    private float timertemp;
    private float Abstime;

    public bool IsFirst;

    private void Start()
    {
        if (IsFirst)
        {
            StartCoroutine(StartLoad());
            return;
        }        
        TitlePrefab.instance.gameObject.SetActive(false);
        
        StageSelectPrefab.instance.gameObject.SetActive(false);
        
        CharSelectPrefab.instance.gameObject.SetActive(false);        

        Time.timeScale = 1;

        BGMManager.instance.FadeOutMusic();

        _color = new Color(0, 0, 0, 0);
        //text_color = ready_text.color;
        //ready_text.text = "Loading...";
        if(DataManager.instance.IsTuto)
        {
            DataManager.instance.SelectedChar = 2;
            StageText.text = "Tutorial";
        }
        else
        {
            StageText.text = "Land " + DataManager.instance.LastStage.ToString();
        }
        LoadingImage.sprite = Images[DataManager.instance.SelectedChar - 1];

        if (DataManager.instance.SelectedChar == 2)
            StageText.GetComponent<Outline>().enabled = true;


        timertemp = 0;
        Abstime = 0;
        //flag = 1;
        DataManager.instance.SaveData();
        StartCoroutine(LoadScene());

        rand_num = Random.Range(0, 14);
        Info_Title.text = InfoTitle(rand_num);
        Info_Text.text = InfoText(rand_num);
    }

    IEnumerator StartLoad()
    {
        yield return null;
        System.GC.Collect();
        AsyncOperation oper = new AsyncOperation();
        oper = SceneManager.LoadSceneAsync("Title");       
        oper.allowSceneActivation = false;
        VideoPlayer VP = GetComponentInChildren<VideoPlayer>();

        float timer = 0.0f;
        while (!oper.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            Abstime += Time.deltaTime * 0.3f;
            timertemp = Mathf.Min(oper.progress * 1.1f, Abstime);

            //ready_text.text = "Loading..." + ((int)(timertemp * 100)).ToString();

            if (timertemp >= 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);


                if (progressBar.fillAmount == 1.0f)
                {

                    /*
                    ready_text.text = "Play";

                    while (true)
                    {
                        if (Input.GetMouseButton(0))
                        {
                            BGMManager.instance.FadeInMusic();
                            BGMManager.instance.Play(1);
                            break;
                        }
                        if(Input.touchCount>0)
                        {
                            BGMManager.instance.FadeInMusic();
                            BGMManager.instance.Play(1);
                            break;
                        }

                        yield return null;

                        text_color.a -= flag * Time.deltaTime;
                        ready_text.color = text_color;
                        if (text_color.a < 0 || text_color.a > 1)
                        {
                            flag *= -1;
                            text_color.a -= flag * Time.deltaTime;
                        }
                    }
                    */

                    while (VP.targetCameraAlpha > 0)
                    {
                        VP.targetCameraAlpha -= 2* Time.deltaTime;
                        yield return null;
                    }
                    
                    oper.allowSceneActivation = true;
                    //GameManager.instance.IsGameStart = false;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, timertemp * 1.1f, timer);
                if (progressBar.fillAmount >= oper.progress)
                {
                    timer = 0f;
                }
            }
        }
    }


    IEnumerator LoadScene()
    {
        yield return null;
        System.GC.Collect();
        AsyncOperation oper = new AsyncOperation();

        
        if(DataManager.instance.IsTuto)
        {
            DataManager.instance.LastStage = 0;
            DataManager.instance.IsTuto = false;
            oper = SceneManager.LoadSceneAsync("Stage0");
        }
        else
        {
            if (!DataManager.instance.IsHard[DataManager.instance.LastStage])
            {
                switch (DataManager.instance.LastStage)
                {
                    case 1:
                        oper = SceneManager.LoadSceneAsync("Stage1");
                        break;

                    case 2:
                        oper = SceneManager.LoadSceneAsync("Stage2");
                        break;

                    case 3:
                        oper = SceneManager.LoadSceneAsync("Stage3");
                        break;

                    case 4:
                        oper = SceneManager.LoadSceneAsync("Stage4");
                        break;

                    case 5:
                        oper = SceneManager.LoadSceneAsync("Stage5");
                        break;

                    case 6:
                        oper = SceneManager.LoadSceneAsync("Stage6");
                        break;

                    case 7:
                        oper = SceneManager.LoadSceneAsync("Stage7");
                        break;

                    case 8:
                        oper = SceneManager.LoadSceneAsync("Stage8");
                        break;

                    case 9:
                        oper = SceneManager.LoadSceneAsync("Stage9");
                        break;

                    case 10:
                        oper = SceneManager.LoadSceneAsync("Stage10");
                        break;

                    case 11:
                        oper = SceneManager.LoadSceneAsync("Stage11");
                        break;

                    case 12:
                        oper = SceneManager.LoadSceneAsync("Stage_ex");
                        break;
                }
            }
            else
            {
                switch (DataManager.instance.LastStage)
                {
                    case 1:
                        oper = SceneManager.LoadSceneAsync("StageH1");
                        break;

                    case 2:
                        oper = SceneManager.LoadSceneAsync("StageH2");
                        break;

                    case 3:
                        oper = SceneManager.LoadSceneAsync("StageH3");
                        break;

                    case 4:
                        oper = SceneManager.LoadSceneAsync("StageH4");
                        break;

                    case 5:
                        oper = SceneManager.LoadSceneAsync("StageH5");
                        break;

                    case 6:
                        oper = SceneManager.LoadSceneAsync("StageH6");
                        break;

                    case 7:
                        oper = SceneManager.LoadSceneAsync("StageH7");
                        break;

                    case 8:
                        oper = SceneManager.LoadSceneAsync("StageH8");
                        break;

                    case 9:
                        oper = SceneManager.LoadSceneAsync("StageH9");
                        break;

                    case 10:
                        oper = SceneManager.LoadSceneAsync("StageH10");
                        break;

                    case 11:
                        oper = SceneManager.LoadSceneAsync("StageH11");
                        break;

                    case 12:
                        oper = SceneManager.LoadSceneAsync("StageH12");
                        break;
                }
            }
        }

        oper.allowSceneActivation = false;

        float timer = 0.0f;
        while (!oper.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            Abstime += Time.deltaTime * 0.5f;
            timertemp = Mathf.Min(oper.progress * 1.1f, Abstime);

            //ready_text.text = "Loading..." + ((int)(timertemp * 100)).ToString();

            if (timertemp >= 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                
                if (progressBar.fillAmount == 1.0f)
                {

                    /*
                    ready_text.text = "Play";

                    while (true)
                    {
                        if (Input.GetMouseButton(0))
                        {
                            BGMManager.instance.FadeInMusic();
                            BGMManager.instance.Play(1);
                            break;
                        }
                        if(Input.touchCount>0)
                        {
                            BGMManager.instance.FadeInMusic();
                            BGMManager.instance.Play(1);
                            break;
                        }

                        yield return null;

                        text_color.a -= flag * Time.deltaTime;
                        ready_text.color = text_color;
                        if (text_color.a < 0 || text_color.a > 1)
                        {
                            flag *= -1;
                            text_color.a -= flag * Time.deltaTime;
                        }
                    }
                    */
                    while (_color.a < 1)
                    {
                        _color.a += 3f * Time.deltaTime;
                        panel.color = _color;
                        yield return null;
                    }

                    oper.allowSceneActivation = true;
                    //GameManager.instance.IsGameStart = false;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, timertemp * 1.1f , timer);
                if (progressBar.fillAmount >= oper.progress)
                {
                    timer = 0f;
                }
            }
        }
    }


    private string InfoTitle(int index)
    {
        string ret="";
        if(DataManager.instance.IsKorean)
        { //e03500 빨강     75c1ea 파랑             719e2b 녹색
            switch (index)
            {
                case 0:
                    ret = "<color=#e03500>" + "지형의 비밀" + "</color>";
                    break;
                case 1:
                    ret = "<color=#e03500>" + "식량관리의 비결" + "</color>";
                    break;
                case 2:
                    ret = "<color=#e03500>" + "전투의 비결1" + "</color>";
                    break;
                case 3:
                    ret = "<color=#e03500>" + "전투의 비결2" + " </color>";
                    break;
                case 4:
                    ret = "<color=#e03500>" + "전투의 비결3" + "</color>";
                    break;
                case 5:
                    ret = "<color=#e03500>" + "전투의 비결4" + "</color>";
                    break;
                case 6:
                    ret = "<color=#719e2b>" + "이동의 비결1" + "</color>";
                    break;
                case 7:
                    ret = "<color=#719e2b>" + "이동의 비결2" + "</color>";
                    break;
                case 8:
                    ret = "<color=#719e2b>" + "식량의 획득" + "</color>";
                    break;
                case 9:
                    ret = "<color=#75c1ea>" + "명령의 우선순위" + "</color>";
                    break;
                case 10:
                    ret = "<color=#75c1ea>" + "조작의 달인1" + "</color>";
                    break;
                case 11:
                    ret = "<color=#75c1ea>" + "조작의 달인2" + "</color>";
                    break;
                case 12:
                    ret = "<color=#75c1ea>" + "성급의 비밀" + "</color>";
                    break;
                case 13:
                    ret = "<color=#75c1ea>" + "하드모드" + "</color>";
                    break;
            }
        }
        else
        {
            switch (index)
            {
                case 0:
                    ret = "<color=#e03500>" + "the secret of terrain" + "</color>";
                    break;
                case 1:
                    ret = "<color=#e03500>" + "The secret of food management" + "</color>";
                    break;
                case 2:
                    ret = "<color=#e03500>" + "The Secret of Battle 1" + "</color>";
                    break;
                case 3:
                    ret = "<color=#e03500>" + "The Secret of Battle 2" + " </color>";
                    break;
                case 4:
                    ret = "<color=#e03500>" + "The Secret of Battle 3" + "</color>";
                    break;
                case 5:
                    ret = "<color=#e03500>" + "The Secret of Battle 4" + "</color>";
                    break;
                case 6:
                    ret = "<color=#719e2b>" + "The secret of movement 1" + "</color>";
                    break;
                case 7:
                    ret = "<color=#719e2b>" + "The secret of movement 2" + "</color>";
                    break;
                case 8:
                    ret = "<color=#719e2b>" + "Food production" + "</color>";
                    break;
                case 9:
                    ret = "<color=#75c1ea>" + "Priority of the command" + "</color>";
                    break;
                case 10:
                    ret = "<color=#75c1ea>" + "Master of control 1" + "</color>";
                    break;
                case 11:
                    ret = "<color=#75c1ea>" + "Master of control 1" + "</color>";
                    break;
                case 12:
                    ret = "<color=#75c1ea>" + "Star secret" + "</color>";
                    break;
                case 13:
                    ret = "<color=#75c1ea>" + "Hard mode" + "</color>";
                    break;
            }
        }
        
        return ret;
    }
    private string InfoText(int index)
    {
        string ret="";
        if(DataManager.instance.IsKorean)
        {
            switch (index)
            {
                case 0:
                    ret = "상대보다 높은 위치에서 공격한다면 추가 데미지를 입힙니다!";
                    break;
                case 1:
                    ret = "부대의 병력수가 많아질수록 소모하는 식량이 크게 늘어납니다.  부대 병력을 적절하게 관리하는 것이 중요합니다.";
                    break;
                case 2:
                    ret = "적 부대를 4방향에서 공격한다면 더 손쉽게 처리할 수 있습니다.";
                    break;
                case 3:
                    ret = "목책을 지어 적의 이동을 잠시 막을 수 있습니다.";
                    break;
                case 4:
                    ret = "전략적인 다리건설은 적의 허를 찌를 수 있습니다.";
                    break;
                case 5:
                    ret = "상대 부대보다 병력이 많을 수록 적은 피해로 승리합니다.";
                    break;
                case 6:
                    ret = "추적을 한다면 기본 이동보다 더 빠른속도로 적에게 다가갈 수 있습니다. 추적하는 동안은 병영을 짓지 못합니다.";
                    break;
                case 7:
                    ret = "3성 부대는 더 빠르게 이동합니다.";
                    break;
                case 8:
                    ret = "식량은 병영이 아닌 지형에서 획득됩니다.";
                    break;
                case 9:
                    ret = "명령에도 우선순위가 있습니다. 상대병력>병영>남의 지형";
                    break;
                case 10:
                    ret = "부대 선택 후 더블 터치 시 주변 아군 부대도 선택됩니다.";
                    break;
                case 11:
                    ret = "대기버튼으로 유닛 이동시 이동 후 그 자리에서 대기합니다";
                    break;
                case 12:
                    ret = "성급이 높은 부대일수록 데미지가 증가합니다";
                    break;
                case 13:
                    ret = "노말모드를 3성으로 클리어시 하드모드가 열립니다";
                    break;
            }
        }
        else
        {
            switch (index)
            {
                case 0:
                    ret = "Attacking on terrain higher than your opponent will deal extra damage!";
                    break;
                case 1:
                    ret = "The more troops you have, the more food you consume.\nIt is important to properly manage your troops.";
                    break;
                case 2:
                    ret = "Attacking enemy units in four directions makes it easier.";
                    break;
                case 3:
                    ret = "Block your enemies for a while by building a barrier.";
                    break;
                case 4:
                    ret = "Strategic bridge construction can create an advantageous situation.";
                    break;
                case 5:
                    ret = "The more troops you have than the opposing forces, the less damage is done.";
                    break;
                case 6:
                    ret = "If you're tracking, you'll be able to reach enemies faster than your basic movement.\nTracking units cannot build barracks.";
                    break;
                case 7:
                    ret = "Three-star units move faster.";
                    break;
                case 8:
                    ret = "Food is obtained from terrain, not barracks.";
                    break;
                case 9:
                    ret = "The command also has priority. Opposing Forces Combat> Barracks> other Terrain.";
                    break;
                case 10:
                    ret = "Double-touching a troop also selects nearby allies.";
                    break;
                case 11:
                    ret = "When you move a unit with the standby button, it waits on the spot after moving.";
                    break;
                case 12:
                    ret = "The higher the rank, the higher the damage.";
                    break;
                case 13:
                    ret = "If you clear normal mode into 3 stars, hard mode will open.";
                    break;
            }
        }
        return ret;
    }


}
