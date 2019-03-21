using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMemz : Button
{
    protected override void Activate()
    {
        base.Activate();
        Application.OpenURL("https://github.com/Horter/MEMZ-4.0");
    }
}
