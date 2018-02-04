using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.DAL;

namespace AnimeRecs.UpdateStreams
{
    class AnimeStreamInfo : IEquatable<AnimeStreamInfo>
    {
        public string AnimeName { get; private set; }
        public string Url { get; private set; }
        public StreamingService Service { get; private set; }

        public AnimeStreamInfo(string animeName, string url, StreamingService service)
        {
            AnimeName = animeName;
            Url = url;
            Service = service;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AnimeStreamInfo);
        }

        public bool Equals(AnimeStreamInfo other)
        {
            if (other == null) return false;
            return this.AnimeName == other.AnimeName && this.Url == other.Url && this.Service == other.Service;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 23;
                hash = hash * 17 + AnimeName.GetHashCode();
                hash = hash * 17 + Url.GetHashCode();
                hash = hash * 17 + Service.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("AnimeName=\"{0}\" Url={1} Service={2}", AnimeName, Url, Service);
        }
    }
}
