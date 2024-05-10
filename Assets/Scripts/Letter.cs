using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    public Text letter;

    public void Initialize(string _value)
    {
        letter.text=_value;
    }
}
