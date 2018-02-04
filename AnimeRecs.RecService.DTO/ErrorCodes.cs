using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    public static class ErrorCodes
    {
        public static string InvalidMessage { get { return "InvalidMessage"; } }
        public static string NoSuchOp { get { return "NoSuchOp"; } }
        public static string InvalidArgument { get { return "InvalidArgument"; } }
        public static string NoSuchRecSource { get { return "NoSuchRecSource"; } }
        public static string Maintenance { get { return "Maintenance"; } }
        public static string NoTrainingData { get { return "NoTrainingData"; } }
        public static string Finalized { get { return "Finalized"; } }
        
        /// <summary>
        /// Used for any errors that do not have their own error code.
        /// </summary>
        public static string Unknown { get { return "Unknown"; } }
    }
}
