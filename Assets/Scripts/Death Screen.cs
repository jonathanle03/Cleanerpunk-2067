using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] public EnemyManager enemyManager;

    [SerializeField] GameObject attackButton;
    [SerializeField] GameObject defenseButton;

    [SerializeField] AudioSource bgm;
    [SerializeField] float FadeTime; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackButton.SetActive(false);
        defenseButton.SetActive(false);
        for(int i = 0; i < 3; i++)
        {
            enemyManager.enemies[i].SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(FadeOut(bgm, FadeTime));
    }


    public void deathButton()
    {
        SceneManager.LoadScene(0);
    }

    public static IEnumerator FadeOut(AudioSource music, float FadeTime)
    {
        float startVolume = music.volume;

        while (music.volume > 0)
        {
            music.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        music.Stop();
        music.volume = startVolume;
    }
}
