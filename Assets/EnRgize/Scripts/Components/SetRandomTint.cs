using UnityEngine;
using System.Collections;

public class SetRandomTint : MonoBehaviour
{
    public float minHue, maxHue; // [0, 360]
    public float minSaturation, maxSaturation; // [0, 1]
    public float minValue, maxValue; // [0, 1]
    public float minAlpha, maxAlpha; // [0, 1]

    void Start() {
        Material materialCopy = renderer.material;

        float hue = Random.Range(minHue, maxHue);
        float saturation = Random.Range(minSaturation, maxSaturation);
        float value = Random.Range(minValue, maxValue);
        float alpha = Random.Range(minAlpha, maxAlpha);

        Color color = ColorUtility.ColorFromHSV(hue, saturation, value, alpha);

        materialCopy.color = color;
        renderer.material = materialCopy;
    }

    void Update() {

    }
}
