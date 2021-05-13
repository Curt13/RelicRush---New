using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntenenceController : MonoBehaviour
{
    public Animator panelAnim;
    public Animator gameInfoAnim;

    public void OnClickOk()
    {
        if (panelAnim != null && gameInfoAnim != null)
        { 
        panelAnim.SetBool("Out", true);
        gameInfoAnim.SetBool("Out", true);
        }
    }
}
