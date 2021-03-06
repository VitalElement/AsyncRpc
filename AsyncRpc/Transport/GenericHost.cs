﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncRpc.Transport
{
	public class GenericHost
	{
		private readonly IRequestHandler _handler;

		private class Request : IRequest
		{
			private readonly TaskCompletionSource<byte[]> _tcs;

			public Request(byte[] data, TaskCompletionSource<byte[]> tcs)
			{
				Data = data;
				_tcs = tcs;
			}

			public Task RespondAsync(byte[] data)
			{
				_tcs.SetResult(data);
				return Task.FromResult(0);
			}

			public byte[] Data { get; private set; }
		}

		public GenericHost(IRequestHandler handler)
		{
			_handler = handler;
		}

		public Task<byte[]> HandleRequest(byte[] data)
		{
			var tcs = new TaskCompletionSource<byte[]>();
			_handler.HandleRequest(new Request(data, tcs));
			return tcs.Task;
		}
	}
}
