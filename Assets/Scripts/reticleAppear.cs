using UnityEngine;

public class reticleAppear : MonoBehaviour
{
    [SerializeField] GameObject reticle;

    void Start()
    {
        reticle.SetActive(false);
    }

    void OnMouseOver()
    {
        reticle.SetActive(true);
    }


    private void OnMouseExit()
    {
        reticle.SetActive(false); 
    }

}
