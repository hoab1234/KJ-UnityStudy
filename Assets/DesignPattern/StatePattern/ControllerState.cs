using UnityEngine;

public class ControllerState : MonoBehaviour, IState
{
    // ��Ʈ�ѷ��� ���� �ΰ� ��ġ ����
    public ContentLocater locater;
    bool focus;

    public ControllerState(ContentLocater locater)
    {
        this.locater = locater;
    }

    public void Enter()
    {
        focus = true;
        Debug.Log($"ControllerState Enter");
    }

    public void Update()
    {
        if (focus && locater.State is not ContentLocaterState.Controllerable)
        {
            IState nextState = locater.State == ContentLocaterState.Moveable ? locater.StateMachine.moveState : locater.StateMachine.rotateState;
            locater.StateMachine.TransitionTo(nextState);
            Debug.Log($"Next State = {nextState}");
        }
    }

    public void Exit()
    {
        focus = false;
        Debug.Log($"ControllerState Exit");
    }
}
