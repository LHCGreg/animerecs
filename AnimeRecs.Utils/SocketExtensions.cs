using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace AnimeRecs.Utils
{
    public static class SocketExtensions
    {
        /// <summary>
        /// Adds cancellation and timeout functionality to ConnectAsync.
        /// If canceled, an OperationCanceledException will be thrown.
        /// If the timeout expires, throws a SocketTimeoutException.
        /// If an exception is thrown, the socket is diposed.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <param name="connectTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.OperationCanceledException">Cancellation was requested.</exception>
        /// <exception cref="AnimeRecs.Utils.SocketTimeoutException">The connection attempt timed out.</exception>
        public static async Task ConnectAsync(this Socket socket, IPAddress address, int port, TimeSpan connectTimeout, CancellationToken cancellationToken)
        {
            using (CancellationTokenSource timeout = new CancellationTokenSource(connectTimeout))
            using (CancellationTokenSource timeoutOrUserCancel = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token))
            {
                Task connectTask = socket.ConnectAsync(address, port);
                try
                {
                    await connectTask.WaitAsync(timeoutOrUserCancel.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    try
                    {
                        socket.Dispose();
                    }
                    catch (Exception)
                    {
                        // ¯\_(ツ)_/¯
                    }

                    // If cancellation was due to user cancellation, keep the OperationCanceledException moving up
                    if (cancellationToken.IsCancellationRequested)
                    {
                        throw;
                    }
                    else
                    {
                        // Otherwise it was due to timeout and should be considered an error, not cancellation.
                        throw new SocketTimeoutException(string.Format("Connection attempt to {0}:{1} timed out.", address, port));
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        socket.Dispose();
                    }
                    catch (Exception)
                    {
                        // ¯\_(ツ)_/¯
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// Adds cancellation, timeout, and "send ALL these bytes" functionality to SendAsync.
        /// If canceled, an OperationCanceledException will be thrown.
        /// If the timeout expires, throws a SocketTimeoutException.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="bytes"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.OperationCanceledException">Cancellation was requested.</exception>
        /// <exception cref="AnimeRecs.Utils.SocketTimeoutException">The connection attempt timed out.</exception>
        public static async Task SendAllAsync(this Socket socket, byte[] bytes, TimeSpan sendAllTimeout, CancellationToken cancellationToken)
        {
            using (CancellationTokenSource timeout = new CancellationTokenSource(sendAllTimeout))
            using (CancellationTokenSource timeoutOrUserCancel = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token))
            {
                int numBytesSent = 0;
                while (numBytesSent < bytes.Length)
                {
                    Task<int> sendTask = socket.SendAsync(new ArraySegment<byte>(bytes, offset: numBytesSent, count: bytes.Length - numBytesSent), SocketFlags.None);
                    int numSentThisTime;
                    try
                    {
                        numSentThisTime = await sendTask.WaitAsync(timeoutOrUserCancel.Token).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        // If cancellation was due to user cancellation, keep the OperationCanceledException moving up
                        if (cancellationToken.IsCancellationRequested)
                        {
                            throw;
                        }
                        else
                        {
                            // Otherwise it was due to timeout and should be considered an error, not cancellation.
                            throw new SocketTimeoutException("Sending data timed out.");
                        }
                    }

                    numBytesSent += numSentThisTime;
                }
            }
        }

        /// <summary>
        /// Adds cancellation, timeout, and "receive exactly this many bytes" functionality to ReceiveAsync.
        /// If canceled, an OperationCanceledException will be thrown.
        /// If the timeout expires, throws a SocketTimeoutException.
        /// If the remote end does not send enough bytes, throws a EndOfStreamException.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="numBytesToReceive"></param>
        /// <param name="receiveAllTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.OperationCanceledException">Cancellation was requested.</exception>
        /// <exception cref="AnimeRecs.Utils.SocketTimeoutException">The connection attempt timed out.</exception>
        /// <exception cref="System.IO.EndOfStreamException">The remote end closed its end of the connection before sending the requested number of bytes.</exception>
        public static async Task<byte[]> ReceiveAllAsync(this Socket socket, int numBytesToReceive, TimeSpan receiveAllTimeout, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[numBytesToReceive];
            await socket.ReceiveAllAsync(buffer, offset: 0, numBytesToReceive: numBytesToReceive, receiveAllTimeout: receiveAllTimeout, cancellationToken: cancellationToken).ConfigureAwait(false);
            return buffer;
        }

        /// <summary>
        /// Adds cancellation, timeout, and "receive exactly this many bytes" functionality to ReceiveAsync.
        /// If canceled, an OperationCanceledException will be thrown.
        /// If the timeout expires, throws a SocketTimeoutException.
        /// If the remote end does not send enough bytes, throws a EndOfStreamException.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="receiveAllTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.OperationCanceledException">Cancellation was requested.</exception>
        /// <exception cref="AnimeRecs.Utils.SocketTimeoutException">The connection attempt timed out.</exception>
        /// <exception cref="System.IO.EndOfStreamException">The remote end closed its end of the connection before sending the requested number of bytes.</exception>
        public static Task ReceiveAllAsync(this Socket socket, byte[] buffer, TimeSpan receiveAllTimeout, CancellationToken cancellationToken)
        {
            return socket.ReceiveAllAsync(buffer, 0, buffer.Length, receiveAllTimeout, cancellationToken);
        }

        /// <summary>
        /// Adds cancellation, timeout, and "receive exactly this many bytes" functionality to ReceiveAsync.
        /// If canceled, an OperationCanceledException will be thrown.
        /// If the timeout expires, throws a SocketTimeoutException.
        /// If the remote end does not send enough bytes, throws a EndOfStreamException.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="numBytesToReceive"></param>
        /// <param name="receiveAllTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.OperationCanceledException">Cancellation was requested.</exception>
        /// <exception cref="AnimeRecs.Utils.SocketTimeoutException">The connection attempt timed out.</exception>
        /// <exception cref="System.IO.EndOfStreamException">The remote end closed its end of the connection before sending the requested number of bytes.</exception>
        public static Task ReceiveAllAsync(this Socket socket, byte[] buffer, int numBytesToReceive, TimeSpan receiveAllTimeout, CancellationToken cancellationToken)
        {
            return socket.ReceiveAllAsync(buffer, 0, numBytesToReceive, receiveAllTimeout, cancellationToken);
        }

        /// <summary>
        /// Adds cancellation, timeout, and "receive exactly this many bytes" functionality to ReceiveAsync.
        /// If canceled, an OperationCanceledException will be thrown.
        /// If the timeout expires, throws a SocketTimeoutException.
        /// If the remote end does not send enough bytes, throws a EndOfStreamException.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="numBytesToReceive"></param>
        /// <param name="receiveAllTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.OperationCanceledException">Cancellation was requested.</exception>
        /// <exception cref="AnimeRecs.Utils.SocketTimeoutException">The connection attempt timed out.</exception>
        /// <exception cref="System.IO.EndOfStreamException">The remote end closed its end of the connection before sending the requested number of bytes.</exception>
        public static async Task ReceiveAllAsync(this Socket socket, byte[] buffer, int offset, int numBytesToReceive, TimeSpan receiveAllTimeout, CancellationToken cancellationToken)
        {
            int numBytesReceived = 0;
            using (CancellationTokenSource timeout = new CancellationTokenSource(receiveAllTimeout))
            using (CancellationTokenSource timeoutOrUserCancel = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token))
            {
                while (numBytesReceived < numBytesToReceive)
                {
                    Task<int> receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(buffer, offset: offset + numBytesReceived, count: numBytesToReceive - numBytesReceived), SocketFlags.None);

                    int numReceivedThisTime;
                    try
                    {
                        numReceivedThisTime = await receiveTask.WaitAsync(timeoutOrUserCancel.Token).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        // If cancellation was due to user cancellation, keep the OperationCanceledException moving up
                        if (cancellationToken.IsCancellationRequested)
                        {
                            throw;
                        }
                        else
                        {
                            // Otherwise it was due to timeout and should be considered an error, not cancellation.
                            throw new SocketTimeoutException("Receiving data timed out.");
                        }
                    }

                    if (numReceivedThisTime == 0)
                    {
                        throw new EndOfStreamException(string.Format("Expected the remote end to send {0} bytes but only received {1} bytes.", numBytesToReceive, numBytesReceived));
                    }

                    numBytesReceived += numReceivedThisTime;
                }
            }
        }
    }
}
