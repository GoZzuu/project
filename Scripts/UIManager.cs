using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    public PlayerStats health;
    public Text MessageText;
    public Slider HealthSlider;
    public Slider SoulsSlider;

    public GameObject gameOverScreen;

    WaitForFixedUpdate waitFixed = new WaitForFixedUpdate();
    WaitForSeconds waitTime = new WaitForSeconds(1);


    private void OnEnable()
    {
        health.PlayerDamaged += DoDamage;
    }
    private void OnDisable()
    {
        health.PlayerDamaged -= DoDamage;
    }
    private void Start()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this);


    }
    void DoDamage()
    {
        HealthSlider.value = health.currentHealth / health.health;

        if (health.Died)
        {
            gameOverScreen.SetActive(true);
        }
    }
    
    public void SetText(string message, float time = 1f)
    {
        MessageText.text = message;
        StartCoroutine(SeeText());
    }

    IEnumerator SeeText()
    {
        var color = MessageText.color;
        while(color.a < 1)
        {
            color.a += Time.fixedDeltaTime;
            MessageText.color = color;
            yield return waitFixed;
        }

        yield return waitTime;
        color = MessageText.color;

        while (color.a > 0)
        {
           
            color.a -= Time.fixedDeltaTime;
            MessageText.color = color;
            yield return waitFixed;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
