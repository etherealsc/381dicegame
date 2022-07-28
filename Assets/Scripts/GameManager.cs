using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        //check that this instance exists
        if (instance == null)
        {
            //if it doesnt, make it exist
            instance = this;
        }
        else if (instance != this)
        {
            //destroy duplicate instances
            Destroy(gameObject);
        }
        //set this instance as protected
        DontDestroyOnLoad(gameObject);


    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MenuScene")
        {
            songPlaying = SongPlaying.SONGMENU;
            audioSource.clip = songMenu;
            audioSource.Play();
        }
        else
        {
            songPlaying = SongPlaying.SONGGAME;
            audioSource.clip = songGame;
            audioSource.Play();
        }
    }

    public AudioClip songMenu;
    public AudioClip songGame;

    public GameObject sunDicePrefab;
    public GameObject waterDicePrefab;
    public GameObject roseDicePrefab;
    public GameObject catDicePrefab;




    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }


    public enum GameStates
    {
        MENU,
        LEVELSELECT,
        GAME
    }

    public GameStates gameState;

    public int dicePerSide;
    public float chanceOfOne;
    public float chanceOfTwo;
    public float chanceOfThree;
    public float chanceOfFour;
    public float chanceOfFive;
    public float chanceOfSix;

    public GameObject playerDicePrefab;
    public GameObject enemyDicePrefab;
    public List<GameObject> dicePrefabs = new List<GameObject>();




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void selectSunDice()
    {
        playerDicePrefab = sunDicePrefab;

        List<GameObject> list = new List<GameObject>();
        list.Add(catDicePrefab);
        list.Add(roseDicePrefab);
        list.Add(waterDicePrefab);
        enemyDicePrefab = list[Random.Range(0, list.Count)];
        PlayGame();
    }
    public void selectWaterDice()
    {
        playerDicePrefab = waterDicePrefab;

        List<GameObject> list = new List<GameObject>();
        list.Add(catDicePrefab);
        list.Add(roseDicePrefab);
        list.Add(sunDicePrefab);
        enemyDicePrefab = list[Random.Range(0, list.Count)];
        PlayGame();
    }
    public void selectRoseDice()
    {
        playerDicePrefab = roseDicePrefab;

        List<GameObject> list = new List<GameObject>();
        list.Add(catDicePrefab);
        list.Add(sunDicePrefab);
        list.Add(waterDicePrefab);
        enemyDicePrefab = list[Random.Range(0, list.Count)];
        PlayGame();
    }
    public void selectCatboyDice()
    {
        playerDicePrefab = catDicePrefab;

        List<GameObject> list = new List<GameObject>();
        list.Add(sunDicePrefab);
        list.Add(roseDicePrefab);
        list.Add(waterDicePrefab);
        enemyDicePrefab = list[Random.Range(0, list.Count)];
        PlayGame();
    }


    public AudioSource audioSource;
    public AudioSource sFXSource;

    public enum SongPlaying
    {
        SONGMENU,
        SONGGAME,

    }
    public SongPlaying songPlaying;

    public void PlayClick()
    {
        //sFXSource.PlayOneShot();
    }

    public List<AudioClip> diceNoises = new List<AudioClip>();

    public float musicLevel = 1f;
    public float sfxLevel = 1f;

}
