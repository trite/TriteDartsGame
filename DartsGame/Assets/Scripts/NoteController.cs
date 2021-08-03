using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteController : MonoBehaviour
{
    public string Text = "Sample Test. 1234";

    private void DoNothing()
    {
        throw new NotImplementedException();
    }

    private TextMeshPro textBox;

    // Start is called before the first frame update
    void Start()
    {
        textBox = GetComponentInChildren<TextMeshPro>();
        textBox.text = Text;

        //textBox.text = "This is a test. 1234 lots of text. Even more text. Just keep going. How much can we fit in this little space?";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        textBox.text = Text;
    }
}
