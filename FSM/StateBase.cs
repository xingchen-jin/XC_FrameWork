
namespace XC_Framework.FSM
{

    public abstract class StateBase<TContext> : IState where TContext : FSMContext
    {
        protected TContext ctx;
        public StateBase(){}
        public void Initialization(FSMContext ctx)
        {
            this.ctx = ctx as TContext;
        }
        public virtual void OnEnter(){}
        public virtual void OnExit(){}
        public virtual void OnUpdate(){}
        public virtual void OnFixedUpdate(){}

        public virtual void OnLateUpdate(){}
  
    }

}
