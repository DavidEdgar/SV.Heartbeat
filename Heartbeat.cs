using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SD.Utils.Heartbeat
{
    using System.Data;

    public class Heartbeat
    {

        public delegate bool BoolDelegate();

        public delegate string StringDelegate();

        public delegate void OnStatusChangedHandler(HeartbeatStatus status, string CallerStatusString);

        public delegate void OnHeartbeatHandler(HeartbeatStatus status, string CallerStatusString);

        private readonly BoolDelegate _greenCondition;
        private readonly BoolDelegate _yellowCondition;
        private readonly StringDelegate _callerStatusString;
        private HeartbeatStatus heartbeatStatus;

        public StatusEnum Status { get; private set; }

        public event OnStatusChangedHandler OnStatusChanged;
        public event OnHeartbeatHandler OnHeartbeat;

        public Heartbeat(string instanceName, BoolDelegate greenCondition, BoolDelegate yellowCondition, StringDelegate callerStatusString)
        {
            heartbeatStatus = new HeartbeatStatus();
            heartbeatStatus.InstanceName = instanceName;

            _greenCondition += greenCondition;
            _yellowCondition += yellowCondition;
            _callerStatusString += callerStatusString;
        }

        public void DoHeartbeat()
        {
            StatusEnum previousStatus = Status;
            bool greenCondition = _greenCondition();
            bool yellowCondition = _yellowCondition();
            string callerStatusString = _callerStatusString();

            if (greenCondition)
            {
                Status = StatusEnum.Green;
            }
            else if (yellowCondition)
            {
                Status = StatusEnum.Yellow;
            }
            else
            {
                Status = StatusEnum.Red;
            }

            heartbeatStatus.Update(Status);

            if (OnHeartbeat != null)
            {
                OnHeartbeat(heartbeatStatus, callerStatusString);
            }

            if (OnStatusChanged != null && previousStatus != Status)
            {
                OnStatusChanged(heartbeatStatus, callerStatusString);
            }
        }
    }
}
