using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features.BTrees.Interfaces
{
    public interface IBTreeMachine
    {
        T GetExtension<T>() where T : Object;
        IEnumerable<T> GetExtensions<T>() where T : Object;
    }
}