using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Text cutText;
    private void Start()
    {
        if (!Generator.instance.alternate)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void Update()
    {
        float Progress = 1 - ((float)Generator.instance._branches.Count / (float)Generator.instance.maxBranchCount);
        fillImage.fillAmount = Progress;
        if(Progress <= 0)
        {
            cutText.text = "Slice to grow";
        }
        else
        {
            cutText.text = "";
        }
    }
}
