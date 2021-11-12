/*
StateSystem.cs and State.cs implement a generic Hierarchical Pushdown Automata.
It is a combination of  Hierarchical States and a Pushdown Automata
as described in Game Programming Patterns' Chapter II.7: State
http://gameprogrammingpatterns.com/state.html
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateSystem : MonoBehaviour
{
    private Stack<State> stack = new Stack<State>();
    [SerializeField] private State defaultState;
    public UnityEvent StateChanged;

    [SerializeField] private bool debug = false;

    // Start is called before the first frame update
    void Start()
    {
        pushState(defaultState);
    }

    // Update is called once per frame
    void Update()
    {
        if (stack.Count == 0) {
            stack.Push(defaultState);
        }

        // Repeat stateUpdate() until the stack is stable. This prevents states from only changing once per frame.
        int maxLoopsPerFrame = 10;
        int loop = 0;
        State oldState;
        do {
            oldState = stack.Peek();
            stack.Peek().stateUpdate();
            //if (loop > 0)
            //printStack();
            //if (loop > 0) Debug.Log("loop: " + loop);
        } while (stack.Count > 0 && stack.Peek() != oldState && loop++ < maxLoopsPerFrame);
    }

    public void popState()
    {
        popStateInner();

        printStack();

        stack.Peek().enter();
        StateChanged.Invoke();
    }

    public void popStateInner()
    {
        if (stack.Count > 0) {
            stack.Peek().exit();
            stack.Pop();
        }
        if (stack.Count == 0) {
            stack.Push(defaultState);
        }
    }

    public void switchState(State state)
    {
        // TODO: if needed, add a "allow transition to self" option to State.cs
        popStateInner();
        pushState(state);
        //printStack();
    }

    // Change the active state. If state == null, pop the current state
    public bool pushState(State state)
    {
        if (stack.Count > 0 && state == stack.Peek()) { // TODO: if needed, add a "allow transition to self" option to State.cs
            return false;
        }
        if (stack.Count > 0) stack.Peek().exit();
        
        if (state) {
            stack.Push(state);
            
            printStack();

            state.enter();
        }


        StateChanged.Invoke();

        return true;
    }

    private void printStack() {
        if (!debug) return;
        string logstring = gameObject.name + " state stack: [";
        //Debug.Log("state stack:");
        int i = 0;
        var arr = stack.ToArray();
        for (i = arr.Length; i > 0; i--) {
            logstring += arr[i-1].GetType().Name + "    ";
            //Debug.Log("\t" + (arr.Length - i) + ": " + arr[i-1].ToString());
        }
        logstring = logstring.Trim();
        logstring += "]";
        Debug.Log(logstring);

    }
}
