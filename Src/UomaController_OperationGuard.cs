using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UomaWeb
{
    public partial class UomaController
    {
        private Dictionary<string, Coroutine> _runningOperations = new Dictionary<string, Coroutine>();

        private bool TryStartOperation(string operationKey, Coroutine operation)
        {
            if (_runningOperations.ContainsKey(operationKey))
            {
                Debug.LogWarning($"Operation '{operationKey}' is already running. Ignoring duplicate request.");
                return false;
            }

            _runningOperations[operationKey] = operation;
            return true;
        }

        private void CompleteOperation(string operationKey)
        {
            if (_runningOperations.ContainsKey(operationKey))
            {
                _runningOperations.Remove(operationKey);
            }
        }

        private bool IsOperationRunning(string operationKey)
        {
            return _runningOperations.ContainsKey(operationKey);
        }
    }
}
