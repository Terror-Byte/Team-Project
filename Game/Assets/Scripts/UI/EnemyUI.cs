using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyUI : MonoBehaviour {

    public RectTransform healthTransform;
    public float minXValue;
    public float maxXValue;
    public float healthY;
    public Enemy enemy;
    public float maxHealth;

    bool initialized = false;

	// Use this for initialization
	void Start () {
        healthTransform = gameObject.GetComponentsInChildren<Image>()[1].rectTransform;
        //maxXValue = healthTransform.localPosition.x;
        minXValue = 0;              
	}
	
	// Update is called once per frame
	void Update () {
        // Didn't seem to like initializing enemy in start
        if (!initialized)
        {
            enemy = gameObject.GetComponent<Enemy>();
            maxHealth = (float)enemy.health;
            initialized = true;
        }

        float currentHealthX = CurrentPosition((float)enemy.health, maxHealth, 100.0f);
        healthTransform.localPosition = new Vector3(currentHealthX - 100, healthTransform.localPosition.y);
	}

    float CurrentPosition(float currentVal, float maxVal, float length)
    {
        // healthPercentage = current health / max health (100 for now)
        // current x = maxXvalue - (maxXvalue * healthPercentage)

        // Percentage = current value / max value
        // current x pos = maxXvalue - (maxVcalue * percentage)

        //float percentage = currentVal / maxVal;
        //float result = maxXValue - (maxXValue * percentage);
        //return result;

        float percent = currentVal / maxVal;
        float amount = length * percent;
        return (minXValue + amount);
    }
}
