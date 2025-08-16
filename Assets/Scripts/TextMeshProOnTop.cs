using UnityEngine;
using TMPro;

public class TextMeshProOnTop : MonoBehaviour
{
    public int sortingOrder = 32767;
    private TextMeshPro textMeshPro;

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        UpdateSorting();
    }

    void UpdateSorting()
    {
        if (textMeshPro != null)
        {
            textMeshPro.sortingLayerID = SortingLayer.NameToID("UI");
            textMeshPro.sortingOrder = sortingOrder;
        }
    }

    void OnValidate()
    {
        if (textMeshPro == null)
            textMeshPro = GetComponent<TextMeshPro>();

        UpdateSorting();
    }
}