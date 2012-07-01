using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService
{
    internal class RecRequestCaster
    {
        private OperationCaster OpCaster { get; set; }

        internal RecRequestCaster(OperationCaster opCaster)
        {
            OpCaster = opCaster;
        }

        public T As<T>()
            where T : GetMalRecsRequest
        {
            return OpCaster.As<Operation<T>>().Payload;
        }
    }
}
