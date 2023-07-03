using UnityEngine;

namespace Features.ServiceLocators.Core.Service
{
    public abstract class SingleService<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected void Awake()
        {
            var contains = ServiceLocator.Resolve<T>();
            if (contains != null)
            {
                Destroy(gameObject);
            }
            else
            {
                ServiceLocator.Register<T>(this as T);
            }
        }
    }
}