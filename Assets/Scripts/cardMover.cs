using UnityEngine;
using UnityEngine.UIElements;


public class cardMover : MonoBehaviour
{

    [SerializeField] bool isUp;
    [SerializeField] private bool isMoving; 
    [SerializeField] float moveSpeed;

    [SerializeField] float yMax;
    [SerializeField] float yMin;
    private Vector3 set;

    [SerializeField] Vector3 upPos;
    [SerializeField] Vector3 downPos;

    private void Start()
    {
        upPos = new Vector3(this.transform.position.x, yMax, this.transform.position.z);
        downPos = new Vector3(this.transform.position.x, yMin, this.transform.position.z);
    }


    void OnMouseOver()
    {
        if (!isUp)
        {
            isUp = true;
            isMoving = true; 
            //this.transform.position = upPos;
        }
        
        
        //If your mouse hovers over the GameObject with the script attached, output this message
        //Debug.Log("Mouse is over GameObject.");
    }

    void OnMouseExit()
    {
        if(isUp)
        {
            isUp = false;
            isMoving = true;
            //this.transform.position = downPos;
        }

        //The mouse is no longer hovering over the GameObject so output this message each frame
        //Debug.Log("Mouse is no longer on GameObject.");
    }


    private void Update()
    {

        if (isMoving)
        {
            if (isUp)
            {
                this.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

                if (this.transform.position.y >= yMax)
                {
                    set = new Vector3(this.transform.position.x, yMax, this.transform.position.z);
                    this.transform.position = set;
                    isMoving = false;
                }

            }
            else
            {
                this.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
                if (this.transform.position.y <= yMin)
                {
                    set = new Vector3(this.transform.position.x, yMin, this.transform.position.z);
                    this.transform.position = set;
                    isMoving = false;
                }

            }
        }
    }
}
