using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketJuleBu : SocketBaseEvent<SocketJuleBu>
{
    public void Link()
    {
        ISocketEngineSink("103.239.245.151", 7272);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
