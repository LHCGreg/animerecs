using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AnimeRecs.RecService
{
    internal static class ReaderWriterLockSlimExtensions
    {
        public static ReadLockToken ScopedReadLock(this ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            return new ReadLockToken(rwLock);
        }

        public static WriteLockToken ScopedWriteLock(this ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            return new WriteLockToken(rwLock);
        }

        public static UpgradeableReadLockToken ScopedUpgradeableReadLock(this ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            return new UpgradeableReadLockToken(rwLock);
        }

        internal class ReadLockToken : IDisposable
        {
            private ReaderWriterLockSlim m_rwLock;

            public ReadLockToken(ReaderWriterLockSlim rwLock)
            {
                m_rwLock = rwLock;
            }

            public void Dispose()
            {
                m_rwLock.ExitReadLock();
            }
        }

        internal class UpgradeableReadLockToken : IDisposable
        {
            private ReaderWriterLockSlim m_rwLock;

            public UpgradeableReadLockToken(ReaderWriterLockSlim rwLock)
            {
                m_rwLock = rwLock;
            }

            public void Dispose()
            {
                m_rwLock.ExitUpgradeableReadLock();
            }
        }

        internal class WriteLockToken : IDisposable
        {
            private ReaderWriterLockSlim m_rwLock;

            public WriteLockToken(ReaderWriterLockSlim rwLock)
            {
                m_rwLock = rwLock;
            }

            public void Dispose()
            {
                m_rwLock.ExitWriteLock();
            }
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecService.
//
// AnimeRecs.RecService is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.  If not, see <http://www.gnu.org/licenses/>.