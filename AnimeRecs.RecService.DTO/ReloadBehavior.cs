using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecService.DTO
{
    public enum ReloadBehavior
    {
        /// <summary>
        /// Old loaded data is discarded before reloading from the database and retraining.
        /// This avoids having to double memory usage but blocks all other operations for the duration of the reload and retrain.
        /// If an error occurs reloading the data from the database, no rec sources will be loaded.
        /// If an error occurs retraining a rec source, that rec source will be dropped.
        /// </summary>
        LowMemory,

        /// <summary>
        /// Keep old rec sources around during a reload/retrain.
        /// This requires double the memory of normal use but keeps the rec service responsive while the reload/retrain is going on.
        /// If an error occurs reloading the data from the database, all rec sources remain loaded.
        /// If an error occurs retraining a rec source, that rec source will be dropped.
        /// </summary>
        HighAvailability
    }
}
