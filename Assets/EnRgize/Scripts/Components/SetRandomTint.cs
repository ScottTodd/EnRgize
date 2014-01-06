using UnityEngine;
using UnityEditor;
using System.Collections;

public class SetRandomTint : MonoBehaviour
{
    public float minHue, maxHue;
    public float minSaturation, maxSaturation;
    public float minValue, maxValue;
    public float minAlpha, maxAlpha;

    void Start() {
        Material materialCopy = renderer.material;

        float hue = Random.Range(minHue, maxHue);
        float saturation = Random.Range(minSaturation, maxSaturation);
        float value = Random.Range(minValue, maxValue);
        float alpha = Random.Range(minAlpha, maxAlpha);

        Color color = EditorGUIUtility.HSVToRGB(hue, saturation, value);
        color.a = alpha;

        materialCopy.color = color;
        renderer.material = materialCopy;
    }

    void Update() {

    }
}
