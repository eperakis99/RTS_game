using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{

    [SerializeField] private Transform transformRelativeTo;
    Camera cam;
    private float startScaleX;
    private float startScaleY;
    
    private float disappearTime = 1f;
    private void Awake()
    {

        startScaleX = transform.localScale.x;
        startScaleY = transform.localScale.y;

    }

    public void startDestruction()
    {
        StartCoroutine(destroyButton());
    }

    protected IEnumerator destroyButton()
    {
        //When clicked the button should slowly disappear
        this.gameObject.SetActive(true);
        while(disappearTime > 0.6)
        {
            disappearTime -= Time.deltaTime;
            Vector3 newScale = new Vector3(startScaleX - (1 - 0.5f*disappearTime), startScaleY - (1 - 0.5f*disappearTime), 0f);
            this.transform.localScale = newScale;
            yield return null;
        }

        disappearTime = 1f;
        Destroy(this.gameObject);
    }
}
