using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine<T>
{
    public State<T> CurrentState => StateStack.Peek();
    public State<T> PrevState => (StateStack.Count > 1) ? StateStack.ElementAt(1) : null;

    public Stack<State<T>> StateStack { get; private set; }

    T owner;
    public StateMachine (T owner)
    {
        this.owner = owner;
        StateStack = new Stack<State<T>>();
    }

    //Executes the current state
    public void Execute()
    {
        CurrentState?.Execute();
    }

    //Pushes new state on top of existing state
    public void Push(State<T> state)
    {
        StateStack.Push(state);
        CurrentState.Enter(owner);
    }


    //Exits current state and goes back to previous state
    public void Pop()
    {
        CurrentState.Exit();
        StateStack.Pop();
    }

    //Replaces current state with new state
    public void ChangeState(State<T> state)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
            StateStack.Pop();
        }

        StateStack.Push(state);
        CurrentState.Enter(owner);
    }

    //Pushes state and waits until that state is popped
    public IEnumerator PushAndWait(State<T> state)
    {
        var prevState = CurrentState;

        Push(state);

        yield return new WaitUntil(() =>  prevState == CurrentState);
    }
}
