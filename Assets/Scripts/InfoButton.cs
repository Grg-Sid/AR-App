using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoButton : MonoBehaviour
{
    private bool clicked = true;
    [SerializeField]
    private Animator animator;
    public void ShowInfo()
    {
        if (clicked)
        {
            animator.SetTrigger("show");
            clicked = false;
        }
        else
        {
            animator.SetTrigger("hide");
            clicked = true;
        }
    }
}
