using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Player
{
    private InputController _inputController = null;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (AI)
        {
            AI = false;
        }
        _inputController = GameObject.Find("InputController").GetComponent<InputController>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            _inputController.HandleInput(Input.mousePosition, gameObject);
        }
    }
}
