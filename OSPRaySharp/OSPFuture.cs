using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPFutureHandle : OSPObjectHandle
    {
    }

    public class OSPFuture : OSPObject
    {
        private OSPFutureHandle handle;

        internal OSPFuture(OSPFutureHandle handle)
        {
            this.handle = handle;
        }

        internal override OSPObjectHandle Handle => handle;

        /// <summary>
        /// Query the progress of the asynchronous operation 
        /// </summary>
        public double Progress
        {
            get
            {
                double progress = NativeMethods.ospGetProgress(handle);
                return progress;
            }
        }

        /// <summary>
        /// Query how long an asynchronous operation ran
        /// </summary>
        public double Duration
        {
            get
            {
                double duration = NativeMethods.ospGetProgress(handle);
                return duration;

            }
        }

        /// <summary>
        /// Cancel the asynchronous operation
        /// </summary>
        public void Cancel()
        {
            NativeMethods.ospCancel(handle);
            OSPDevice.CheckLastDeviceError();
        }

        /// <summary>
        /// Blocks the current thread until the asynchronous operation is completed.
        /// </summary>
        /// <param name="syncEvent"></param>
        public void Wait(OSPSyncEvent syncEvent = OSPSyncEvent.TaskFinished)
        {
            NativeMethods.ospWait(handle, syncEvent);
            OSPDevice.CheckLastDeviceError();
        }

        /// <summary>
        /// Query whether particular events are completed.
        /// </summary>
        /// <param name="syncEvent"></param>
        /// <returns></returns>
        public bool IsReady(OSPSyncEvent syncEvent = OSPSyncEvent.TaskFinished)
        {
            bool value = NativeMethods.ospIsReady(handle, syncEvent) == 1;
            return value;
        }


    }
}
