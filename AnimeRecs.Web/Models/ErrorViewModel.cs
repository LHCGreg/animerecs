using System;

namespace AnimeRecs.Web.Models
{
    public class ErrorViewModel
    {
        public Exception Exception { get; private set; }

        public ErrorViewModel(Exception exception)
        {
            Exception = exception;
        }
    }
}