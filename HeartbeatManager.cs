using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace SD.Utils.Heartbeat
{
	public class HeartbeatManager
	{
		private IDictionary<Guid, Task> _heartbeats;

		public HeartbeatManager()
		{
			_heartbeats = new Dictionary<Guid, Task> ();
		}
		
		public Guid StartBeating(Heartbeat heartbeat){
            TaskScheduler scheduler = TaskScheduler.Current;

			Guid id = Guid.NewGuid();
			var task = Task.Factory.StartNew (() => {
				do {                     
					heartbeat.DoHeartbeat ();
					Thread.Sleep (5000);
				} while (true);
			},
				            CancellationToken.None,
				            TaskCreationOptions.None,
				            scheduler);

			_heartbeats.Add (id, task);

			return id;
		}
	}
}