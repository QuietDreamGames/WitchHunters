using System.Collections.Generic;

namespace Features.FiniteStateMachine.Interfaces
{
    public interface IMachine
    {
        void ChangeState(string stateID);
        
        void AddExtension<T>(T extension) where T : class;

        T GetExtension<T>() where T : class;
        public IEnumerable<T> GetExtensions<T>() where T : class;
    }
}