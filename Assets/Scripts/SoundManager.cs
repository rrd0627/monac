using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    private AudioSource source;

    public float Volumn;
    public bool loop;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.volume = Volumn;
    }

    public void Play()
    {
        source.Play();
    }
    public void Stop()
    {
        source.Stop();
    }
    public void SetLoop()
    {
        source.loop = true;
    }
    public void SetLoopCancel()
    {
        source.loop = false;
    }

    public void SetVolumn(float _vol)
    {
        Volumn = _vol;
        source.volume = Volumn;
    }
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;

    public List<GameObject> SoundObject;

    [SerializeField]
    public Sound[] sounds;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject(sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
            SoundObject.Add(soundObject);
        }

        for (int i = 0; i < SoundObject.Count; i++)
        {
            SoundObject[i].GetComponent<AudioSource>().volume = DataManager.instance.SoundSettingValue;
        }
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Volumn = DataManager.instance.SoundSettingValue;
        }

    }

    public void Play(int i) //0 build  1 gamestart  2buttonclick  3 stageselect  4 chardie  5 lose   6 win  7 alarm
    {
        sounds[i].Play();
    }
    public void Stop(int i)
    {
        sounds[i].Stop();
    }
    public void SetLoop(int i)
    {
        sounds[i].SetLoop();
    }
    public void SetLoopCancel(int i)
    {
        sounds[i].SetLoopCancel();
    }
    public void SetVolumn(Slider vol)
    {
        if (!DataManager.instance.PAUSEDPanel.activeSelf) return;

        for (int i=0;i< SoundObject.Count;i++)
        {
            SoundObject[i].GetComponent<AudioSource>().volume = vol.value;
        }
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Volumn = vol.value;
        }
        DataManager.instance.SoundSettingValue = vol.value;
    }
    public void SetGameVolumn(Slider vol)
    {
        if (!GameManager.instance.SettingPanel.activeSelf) return;

        for (int i = 0; i < SoundObject.Count; i++)
        {
            SoundObject[i].GetComponent<AudioSource>().volume = vol.value;
        }
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Volumn = vol.value;
        }
        DataManager.instance.SoundSettingValue = vol.value;
    }
}