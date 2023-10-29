using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        AI = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
