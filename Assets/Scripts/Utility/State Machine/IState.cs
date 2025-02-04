using System;

namespace ServiceLocator.Utility
{
    public interface IStateOwner<T> where T : class
    {
        public T Owner { get; set; }
    }

    public interface IState<T, U> where T : class, IStateOwner<T> where U : Enum
    {
        public T Owner { get; set; }
        public void OnStateEnter();
        public void Update();
        public void FixedUpdate();
        public void OnStateExit();
    }
}