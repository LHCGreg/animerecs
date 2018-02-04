using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService
{
    internal class OperationDescription
    {
        public Type OperationType { get; private set; }
        
        // Right now this is only used as documentation.
        public Type ResponseType { get; private set; }

        public OperationHandler OperationHandler { get; private set; }

        public OperationDescription(OperationHandler operationHandler, Type operationType, Type responseType)
        {
            OperationHandler = operationHandler;
            OperationType = operationType;
            ResponseType = responseType;
        }
    }
}
