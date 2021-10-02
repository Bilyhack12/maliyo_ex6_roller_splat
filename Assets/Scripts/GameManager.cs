using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public GameObject completedParticles;
    public AudioClip completedClip;
    

    private static AudioSource audioSource;

    private GroundPiece[] allGroundPieces;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetupNewLevel();
    }

    private void SetupNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }

    public void CheckComplete()
    {
        bool isFinished = true;

        for (int i = 0; i < allGroundPieces.Length; i++)
        {
            if (allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }
        }

        if (isFinished)
            StartCoroutine(OnCompletedLevel());
    }

    IEnumerator OnCompletedLevel(){
        audioSource.clip = completedClip;
        Instantiate(completedParticles, completedParticles.transform.position, completedParticles.transform.rotation);
        audioSource.Play();
        yield return new WaitForSeconds(2.0f);
        NextLevel(); 
    }

    private void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex==4){
            SceneManager.LoadScene(0);
        }
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}