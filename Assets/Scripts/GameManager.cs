using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventBus.Subscribe<TestEventBusEvent>(_OnTest); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EventBus.Publish<TestEventBusEvent>(new TestEventBusEvent("THIS IS A TEST"));
    }

    void _OnTest(TestEventBusEvent e)
    {
        Debug.Log(e.message);
    }
}

public class TestEventBusEvent{
    public string message;
    public TestEventBusEvent( string _message) { message = _message; }
}
