using UnityEngine;
using System.Collections;

public class JumpPadController : MonoBehaviour {

    private JumpPad jumpPad;
    public float jumpForce = 6000f;

	// Use this for initialization
	void Start () {
        jumpPad = new JumpPad();
        jumpPad.JumpForce = jumpForce;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag.ToString().Equals("Player"))
        {
            if (coll.contacts[0].normal == new Vector2(0, -1))
            {
                coll.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpPad.JumpForce), ForceMode2D.Impulse);

            }
        }
       

        

    }
    
}
