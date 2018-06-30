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

    bool isShowing = false;


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
       
        StartCoroutine(SeeText(message));
    }

    IEnumerator SeeText(string message)
    {      
        while (isShowing)
        {            
            yield return waitFixed;
        }

        isShowing = true;
        var color = MessageText.color;
        MessageText.text = message;

        while (color.a < 1)
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
        isShowing = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
