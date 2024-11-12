using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    Player player;
    public Sprite bg_100;
    public Sprite bg_75;
    public Sprite bg_50;
    public Sprite bg_25;

    public GameObject background;

    private Image i;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        i = background.GetComponent<Image>();
        i.sprite = bg_100;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = player.health;

        if (slider.value > 75)
        {
            i.sprite = bg_100;
        }
        else if (slider.value > 50 && slider.value <= 75)
        {
            i.sprite = bg_75;
        }
        else if (slider.value > 25 && slider.value <= 50)
        {
            i.sprite = bg_50;
        }
        else if (slider.value <= 25)
        {
            i.sprite = bg_25;
        }
    }
}