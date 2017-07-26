using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AnimeRecs.Utils.UnitTests
{
    public class AsyncUpgradeableReaderWriterLockTests
    {
        // Possible states:
        // Nothing
        // 1 reader
        // multiple readers
        // multiple readers and an upgradeable
        // upgradeable
        // upgraded
        // writer

        [Fact]
        public void TestEmptyState()
        {
            AsyncUpgradeableReaderWriterLock rwLock = new AsyncUpgradeableReaderWriterLock();

            // No readers/writers -> enter reader should not be contended
            AssertReadLockUncontended(rwLock);

            // No readers/writers -> enter upgradeable and upgrading should not be contended
            AssertUpgradeableLockUncontended(rwLock);
            AssertUpgradeableLockUpgradeUncontended(rwLock);

            // No readers/writers -> enter writer should not be contended
            AssertWriteLockUncontended(rwLock);
        }

        [Fact]
        public void TestOneReader()
        {
            AsyncUpgradeableReaderWriterLock rwLock = new AsyncUpgradeableReaderWriterLock();
            using (CancellationTokenSource readLockExit = new CancellationTokenSource())
            using (ManualResetEventSlim setWhenReadLockHeld = new ManualResetEventSlim(initialState: false))
            {
                try
                {
                    Task readLockHold = HoldReadLockAsync(rwLock, setWhenReadLockHeld, readLockExit.Token);
                    setWhenReadLockHeld.Wait();

                    // 1 reader -> enter reader should not be contended
                    AssertReadLockUncontended(rwLock);

                    // 1 reader -> enter upgradeable should not be contended
                    AssertUpgradeableLockUncontended(rwLock);

                    // 1 reader -> upgrading upgradeable lock should be contended
                    AssertUpgradeableLockUpgradeContended(rwLock);

                    // 1 reader -> enter writer should be contended
                    AssertWriteLockContended(rwLock);
                }
                finally
                {
                    readLockExit.Cancel();
                }
            }
        }

        [Fact]
        public void TestMultipleReadersAndUpgradeable()
        {
            AsyncUpgradeableReaderWriterLock rwLock = new AsyncUpgradeableReaderWriterLock();
            using (CancellationTokenSource readLock1Exit = new CancellationTokenSource())
            using (CancellationTokenSource upgradeableLockExit = new CancellationTokenSource())
            using (CancellationTokenSource readLock2Exit = new CancellationTokenSource())
            using (ManualResetEventSlim setWhenReadLock1Held = new ManualResetEventSlim(initialState: false))
            using (ManualResetEventSlim setWhenUpgradeableLockHeld = new ManualResetEventSlim(initialState: false))
            using (ManualResetEventSlim setWhenReadLock2Held = new ManualResetEventSlim(initialState: false))
            {
                try
                {
                    Task readLock1Hold = HoldReadLockAsync(rwLock, setWhenReadLock1Held, readLock1Exit.Token);
                    setWhenReadLock1Held.Wait();
                    Task upgradeableLockHold = HoldUpgradeableLockAsync(rwLock, setWhenUpgradeableLockHeld, upgradeableLockExit.Token);
                    setWhenUpgradeableLockHeld.Wait();
                    Task readLock2Hold = HoldReadLockAsync(rwLock, setWhenReadLock2Held, readLock2Exit.Token);
                    setWhenReadLock2Held.Wait();

                    // multiple readers and upgradeable -> enter reader should not be contended
                    AssertReadLockUncontended(rwLock);

                    // multiple readers and upgradeable -> enter upgradeable should be contended
                    AssertUpgradeableLockContended(rwLock);

                    // multiple readers and upgradeable -> enter writer should be contended
                    AssertWriteLockContended(rwLock);

                    // exit upgradeable lock that is in background task
                    upgradeableLockExit.Cancel();
                    upgradeableLockHold.Wait();

                    // enter upgradeable lock in foreground and try to upgrade, which should be contended
                    AssertUpgradeableLockUpgradeContended(rwLock);
                }
                finally
                {
                    readLock2Exit.Cancel();
                    upgradeableLockExit.Cancel();
                    readLock1Exit.Cancel();
                }
            }
        }

        [Fact]
        public void TestUpgradeable()
        {
            AsyncUpgradeableReaderWriterLock rwLock = new AsyncUpgradeableReaderWriterLock();
            using (CancellationTokenSource upgradeableLockExit = new CancellationTokenSource())
            using (ManualResetEventSlim setWhenUpgradeableLockHeld = new ManualResetEventSlim(initialState: false))
            {
                try
                {
                    Task upgradeableLockHold = HoldUpgradeableLockAsync(rwLock, setWhenUpgradeableLockHeld, upgradeableLockExit.Token);
                    setWhenUpgradeableLockHeld.Wait();

                    // upgradeable -> enter reader should not be contended
                    AssertReadLockUncontended(rwLock);

                    // upgradeable -> enter upgradeable should be contended
                    AssertUpgradeableLockContended(rwLock);

                    // upgradeable -> enter writer should be contended
                    AssertWriteLockContended(rwLock);

                    // exit upgradeable lock that is in background task
                    upgradeableLockExit.Cancel();
                    upgradeableLockHold.Wait();

                    // enter upgradeable lock in foreground and try to upgrade, which should not be contended
                    AssertUpgradeableLockUpgradeUncontended(rwLock);
                }
                finally
                {
                    upgradeableLockExit.Cancel();
                }
            }
        }

        [Fact]
        public void TestUpgraded()
        {
            AsyncUpgradeableReaderWriterLock rwLock = new AsyncUpgradeableReaderWriterLock();
            using (CancellationTokenSource upgradedLockExit = new CancellationTokenSource())
            using (ManualResetEventSlim setWhenUpgradedLockHeld = new ManualResetEventSlim(initialState: false))
            {
                try
                {
                    Task upgradedLockHold = HoldUpgradedLockAsync(rwLock, setWhenUpgradedLockHeld, upgradedLockExit.Token);
                    setWhenUpgradedLockHeld.Wait();

                    // upgraded -> enter reader should be contended
                    AssertReadLockContended(rwLock);

                    // upgraded -> enter upgradeable should be contended
                    AssertUpgradeableLockContended(rwLock);

                    // upgraded -> enter writer should be contended
                    AssertWriteLockContended(rwLock);
                }
                finally
                {
                    upgradedLockExit.Cancel();
                }
            }
        }

        [Fact]
        public void TestWriter()
        {
            AsyncUpgradeableReaderWriterLock rwLock = new AsyncUpgradeableReaderWriterLock();
            using (CancellationTokenSource writerLockExit = new CancellationTokenSource())
            using (ManualResetEventSlim setWhenWriteLockHeld = new ManualResetEventSlim(initialState: false))
            {
                try
                {
                    Task writeLockHold = HoldWriteLockAsync(rwLock, setWhenWriteLockHeld, writerLockExit.Token);
                    setWhenWriteLockHeld.Wait();

                    // writer -> enter reader should be contended
                    AssertReadLockContended(rwLock);

                    // writer -> enter upgradeable should be contended
                    AssertUpgradeableLockContended(rwLock);

                    // writer -> enter writer should be contended
                    AssertWriteLockContended(rwLock);
                }
                finally
                {
                    writerLockExit.Cancel();
                }
            }
        }

        private Task HoldReadLockAsync(AsyncUpgradeableReaderWriterLock rwLock, ManualResetEventSlim setWhenLockHeld, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                using (rwLock.EnterReadLockAsync(CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    setWhenLockHeld.Set();
                    cancellationToken.WaitHandle.WaitOne();
                }
            });
        }

        private Task HoldUpgradeableLockAsync(AsyncUpgradeableReaderWriterLock rwLock, ManualResetEventSlim setWhenLockHeld, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                using (rwLock.EnterUpgradeableReadLockAsync(CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    setWhenLockHeld.Set();
                    cancellationToken.WaitHandle.WaitOne();
                }
            });
        }

        private Task HoldUpgradedLockAsync(AsyncUpgradeableReaderWriterLock rwLock, ManualResetEventSlim setWhenLockHeld, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                using (rwLock.EnterUpgradeableReadLockAsync(CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    using (rwLock.UpgradeToWriteLock(CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult())
                    {
                        setWhenLockHeld.Set();
                        cancellationToken.WaitHandle.WaitOne();
                    }
                }
            });
        }

        private Task HoldWriteLockAsync(AsyncUpgradeableReaderWriterLock rwLock, ManualResetEventSlim setWhenLockHeld, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                using (rwLock.EnterWriteLockAsync(CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    setWhenLockHeld.Set();
                    cancellationToken.WaitHandle.WaitOne();
                }
            });
        }

        private void AssertReadLockUncontended(AsyncUpgradeableReaderWriterLock rwLock)
        {
            using (CancellationTokenSource timeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(200)))
            // Will throw an OperationCanceledException if unable to get the lock within 200 ms
            using (rwLock.EnterReadLockAsync(timeout.Token).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                ;
            }
        }

        private void AssertUpgradeableLockUncontended(AsyncUpgradeableReaderWriterLock rwLock)
        {
            using (CancellationTokenSource timeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(200)))
            using (rwLock.EnterUpgradeableReadLockAsync(timeout.Token).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                ;
            }
        }

        private void AssertUpgradeableLockUpgradeUncontended(AsyncUpgradeableReaderWriterLock rwLock)
        {
            using (CancellationTokenSource timeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(200)))
            using (rwLock.EnterUpgradeableReadLockAsync(timeout.Token).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                using (CancellationTokenSource upgradeTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(200)))
                using (rwLock.UpgradeToWriteLock(upgradeTimeout.Token).ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    ;
                }
            }
        }

        private void AssertWriteLockUncontended(AsyncUpgradeableReaderWriterLock rwLock)
        {
            using (CancellationTokenSource timeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(200)))
            using (rwLock.EnterWriteLockAsync(timeout.Token).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                ;
            }
        }

        private void AssertReadLockContended(AsyncUpgradeableReaderWriterLock rwLock)
        {
            using (CancellationTokenSource timeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(200)))
            {
                Assert.Throws<TaskCanceledException>(() => rwLock.EnterReadLockAsync(timeout.Token).ConfigureAwait(false).GetAwaiter().GetResult());
            }
        }

        private void AssertUpgradeableLockContended(AsyncUpgradeableReaderWriterLock rwLock)
        {
            using (CancellationTokenSource timeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(200)))
            {
                Assert.Throws<TaskCanceledException>(() => rwLock.EnterUpgradeableReadLockAsync(timeout.Token).ConfigureAwait(false).GetAwaiter().GetResult());
            }
        }

        private void AssertUpgradeableLockUpgradeContended(AsyncUpgradeableReaderWriterLock rwLock)
        {
            using (CancellationTokenSource timeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(200)))
            using (rwLock.EnterUpgradeableReadLockAsync(timeout.Token).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                using (CancellationTokenSource upgradeTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(200)))
                    Assert.Throws<TaskCanceledException>(() => rwLock.UpgradeToWriteLock(timeout.Token).ConfigureAwait(false).GetAwaiter().GetResult());
            }
        }

        private void AssertWriteLockContended(AsyncUpgradeableReaderWriterLock rwLock)
        {
            using (CancellationTokenSource timeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(200)))
            {
                Assert.Throws<TaskCanceledException>(() => rwLock.EnterWriteLockAsync(timeout.Token).ConfigureAwait(false).GetAwaiter().GetResult());
            }
        }
    }
}

// Copyright (C) 2017 Greg Najda
//
// This file is part of AnimeRecs.Utils.UnitTests
//
// AnimeRecs.Utils.UnitTests is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.Utils.UnitTests is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.Utils.UnitTests.  If not, see <http://www.gnu.org/licenses/>.
