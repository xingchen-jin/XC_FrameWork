
namespace XC_Framework.FSM
{
    public interface IState
    {
        void Initialization(FSMContext context);
        void OnEnter();
        void OnExit();
        void OnUpdate();
        // void OnInput(PlayerInputData inputData);
        void OnFixedUpdate();
        void OnLateUpdate();
        // void OnCheck();
    }
}
