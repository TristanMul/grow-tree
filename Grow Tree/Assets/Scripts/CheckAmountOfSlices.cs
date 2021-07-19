using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckAmountOfSlices : MonoBehaviour
{
    int maxSlices;
    int slicesLeft;
    [SerializeField] private Text sliceText;
    public static CheckAmountOfSlices instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void UpdateSlices(int _maxSlices, int _slicesLeft)
    {
        maxSlices = _maxSlices;
        slicesLeft = _slicesLeft;
        sliceText.text = slicesLeft + " of " + maxSlices + " slices left";
    }
}
