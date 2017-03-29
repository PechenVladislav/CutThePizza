using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPercent : MonoBehaviour {

    [SerializeField]
    private Text textComponent;
    [SerializeField]
    private Animator animator;

    private void OnEnable()
    {
        MainPizza.dispalyText += DisplayText;
    }

    private void OnDisable()
    {
        MainPizza.dispalyText -= DisplayText;
    }

    private void DisplayText(float percent)
    {
        textComponent.enabled = true;
        textComponent.text =  "-" + percent.ToString("0.00") + "%";
        animator.CrossFade("TextFade", 0.5f);
    }
}
