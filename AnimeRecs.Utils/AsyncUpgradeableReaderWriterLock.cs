using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace AnimeRecs.Utils
{
    /// <summary>
    /// AsyncReaderWriterLock that supports taking an upgradeable read lock, that blocks out writers but allows readers,
    /// and can be upgraded to a write lock the blocks out readers as well.
    /// 
    /// There is no help for you if you use this class incorrectly! It does not attempt to detect erroneous usage.
    /// </summary>
    public sealed class AsyncUpgradeableReaderWriterLock
    {
        // To get readable lock: get readable lock

        // To get upgradeable read lock: get mutex. This prevents writers and other upgradeable read locks,
        // but allows readers with their readable lock.

        // To upgrade upgradeable read lock: get writeable lock

        // To get writeable lock: get mutex, get writeable lock

        private AsyncReaderWriterLock m_readerWriterLock;
        private AsyncLock m_potentialWriterLock;

        public AsyncUpgradeableReaderWriterLock()
        {
            m_readerWriterLock = new AsyncReaderWriterLock();
            m_potentialWriterLock = new AsyncLock();
        }

        public AwaitableDisposable<IDisposable> EnterReadLockAsync(CancellationToken cancelletionToken)
        {
            return m_readerWriterLock.ReaderLockAsync(cancelletionToken);
        }

        public AwaitableDisposable<IDisposable> EnterUpgradeableReadLockAsync(CancellationToken cancellationToken)
        {
            return m_potentialWriterLock.LockAsync(cancellationToken);
        }

        public AwaitableDisposable<IDisposable> UpgradeToWriteLock(CancellationToken cancellationToken)
        {
            return m_readerWriterLock.WriterLockAsync(cancellationToken);
        }

        public AwaitableDisposable<IDisposable> EnterWriteLockAsync(CancellationToken cancellationToken)
        {
            return new AwaitableDisposable<IDisposable>(EnterWriteLockAsyncImpl(cancellationToken));
        }

        private async Task<IDisposable> EnterWriteLockAsyncImpl(CancellationToken cancellationToken)
        {
            IDisposable potentialWriterLockRelease = await m_potentialWriterLock.LockAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                IDisposable readerWriterLockRelease = await m_readerWriterLock.WriterLockAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    WriteLockRelease release = new WriteLockRelease(potentialWriterLockRelease, readerWriterLockRelease);
                    return release;
                }
                catch
                {
                    readerWriterLockRelease.Dispose();
                    throw;
                }
            }
            catch
            {
                potentialWriterLockRelease.Dispose();
                throw;
            }
        }

        private struct WriteLockRelease : IDisposable
        {
            private IDisposable m_potentialWriterLockRelease;
            private IDisposable m_readerWriterLockRelease;

            public WriteLockRelease(IDisposable potentialWriterLockRelease, IDisposable readerWriterLockRelease)
            {
                m_potentialWriterLockRelease = potentialWriterLockRelease;
                m_readerWriterLockRelease = readerWriterLockRelease;
            }

            public void Dispose()
            {
                m_readerWriterLockRelease.Dispose();
                m_potentialWriterLockRelease.Dispose();
            }
        }
    }
}
