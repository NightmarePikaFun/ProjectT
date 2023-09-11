using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Mass_script : MonoBehaviour
{
    [SerializeField]
    private Text massText;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        massText.text = rb.mass.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMass()
    {
        rb.mass += 1;
        massText.text = rb.mass.ToString();
    }

    public void RemoveMass()
    {
        rb.mass -= 1;
        massText.text = rb.mass.ToString();
    }
}
