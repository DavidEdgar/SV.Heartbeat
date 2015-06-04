using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SD.Utils.Heartbeat
{
    public class HeartbeatStatus
    {
        public string InstanceName { get; set; }

        public StatusEnum Status { get; set; }
        public StatusEnum LastStatus { get; set; }

        public DateTime LastHeartbeat { get; set; }

        public DateTime? LastGreen { get; set; }

        public DateTime? LastYellow { get; set; }

        public DateTime? LastRed { get; set; }

        public HeartbeatStatus()
        {
            this.Status = StatusEnum.Unknown;
        }

        internal void Update(StatusEnum status)
        {
            this.LastStatus = Status;
            this.Status = status;
            this.LastHeartbeat = DateTime.UtcNow;

            //based on the status passed, also modify other datetimes
            switch (this.Status)
            {
                case StatusEnum.Green:
                    this.LastGreen = this.LastHeartbeat;
                    break;
                case StatusEnum.Yellow:
                    this.LastYellow = this.LastHeartbeat;
                    break;
                case StatusEnum.Red:
                    this.LastRed = this.LastHeartbeat;
                    break;
            }
        }

    }
}
