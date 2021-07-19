using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
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
    }
}
