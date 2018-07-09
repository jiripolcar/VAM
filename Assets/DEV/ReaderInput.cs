using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReaderInput : MonoBehaviour
{
    public Text clearText;
    public Text rawText;

    private Dictionary<string, string> recode;

    private void Start()
    {
        recode = new Dictionary<string, string>();
        recode.Add("+", "1");
        recode.Add("ě", "2");
        recode.Add("š", "3");
        recode.Add("č", "4");
        recode.Add("ř", "5");
        recode.Add("ž", "6");
        recode.Add("ý", "7");
        recode.Add("á", "8");
        recode.Add("í", "9");
        recode.Add("é", "0");
    }

    // Update is called once per frame
    void Update()
    {
        string s = Input.inputString;
        rawText.text += s;
        if (s.Length > 0)
        {            
            string output = "";

            for (int i = 0; i < s.Length; i++)
            {         
                string ch = s.Substring(i, 1);
                string outValue = "XX";
                if (recode.TryGetValue(ch, out outValue))
                    output += outValue;
                else
                    output += ch;

            }
            clearText.text += output;
        }
    }
}
