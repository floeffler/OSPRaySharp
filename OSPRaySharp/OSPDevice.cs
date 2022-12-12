using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPDeviceHandle : OSPHandle
    {
        protected override bool ReleaseHandle()
        {
            NativeMethods.ospDeviceRelease(handle);
            return true;
        }

        public static readonly OSPDeviceHandle Empty = new OSPDeviceHandle();
    }

    public enum LogLevel
    {
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
        None = 5
    }

    /// <summary>
    /// OSPRay device object. 
    /// </summary>
    public class OSPDevice : IDisposable
    {
        private OSPDeviceHandle handle;

        internal OSPDevice(OSPDeviceHandle handle)
        {
            this.handle = handle;
        }

        /// <summary>
        /// Gets the version of the device
        /// </summary>
        public Version Version
        {
            get
            {
                int major = (int)NativeMethods.ospDeviceGetProperty(handle, OSPDeviceProperty.OSP_DEVICE_VERSION_MAJOR);
                int minor = (int)NativeMethods.ospDeviceGetProperty(handle, OSPDeviceProperty.OSP_DEVICE_VERSION_MINOR);
                int patch = (int)NativeMethods.ospDeviceGetProperty(handle, OSPDeviceProperty.OSP_DEVICE_VERSION_PATCH);
                return new Version(major, minor, patch);
            }
        }

        
        internal OSPDeviceHandle Handle => handle;

        /// <summary>
        /// Sets a device parameter by name.
        /// </summary>
        /// <typeparam name="T">the type of the parameter</typeparam>
        /// <param name="parameterId">the parameter name/id</param>
        /// <param name="parameterValue">the parameter value or null for default</param>
        public void SetParam<T>(string parameterId, T? parameterValue) where T : unmanaged
        {
            OSPDataType dataType = OSPDataTypeUtil.GetDataTypeOrThrow<T>();
            if (parameterValue.HasValue)
            {
                T value = parameterValue.Value;
                unsafe
                {
                    NativeMethods.ospDeviceSetParam(handle, parameterId, dataType, &value);
                }
            }
            else
            {
                NativeMethods.ospDeviceRemoveParam(handle, parameterId);
            }
            CheckLastError();
        }

        /// <summary>
        /// Check if a previous API call recorded an error. If an error has been
        /// recorded, an OSPException with the error code and the error message is thrown.
        /// </summary>
        public void CheckLastError() => CheckLastDeviceError(handle);

        /// <summary>
        /// Makes this device current. 
        /// All subsequent api calls will use this device.
        /// </summary>
        public void SetCurrent()
        {
            NativeMethods.ospSetCurrentDevice(handle);
            CheckLastError();
        }

        public bool IsCurrent
        {
            get
            {
                var current = NativeMethods.ospGetCurrentDevice();
                return handle.Equals(current);
            }
        }

        /// <summary>
        /// Commit parameter changes to the device
        /// </summary>
        public void Commit()
        {
            NativeMethods.ospDeviceCommit(handle);
            CheckLastError();
        }

        /// <summary>
        /// Dispose this device object and releases the underlaying OSPDevice handle.
        /// </summary>
        public void Dispose()
        {
            handle.Dispose();
        }

        /// <summary>
        /// Gets the current device as an new OSPDevice object. The underlaying handle is retained.
        /// The caller is responsible to dispose the instance.
        /// </summary>
        /// <returns></returns>
        public static OSPDevice? GetCurrentAndRetain()
        {
            var handle = NativeMethods.ospGetCurrentDevice();
            if (handle.IsInvalid)
                return null;

            NativeMethods.ospDeviceRetain(handle);
            return new OSPDevice(handle);
        }

        private static void CheckLastDeviceError(OSPDeviceHandle device)
        {
            int lastError = NativeMethods.ospDeviceGetLastErrorCode(device);
            if (lastError != 0)
            {
                string errorMessage = NativeMethods.ospDeviceGetLastErrorMessage(device);
                throw new OSPException(lastError, errorMessage);
            }
        }
        internal static void CheckLastDeviceError()
        {
            OSPDeviceHandle device = NativeMethods.ospGetCurrentDevice();
            CheckLastDeviceError(device);
        }

        internal static void ThrowLastError()
        {
            OSPDeviceHandle device = NativeMethods.ospGetCurrentDevice();
            int lastError = NativeMethods.ospDeviceGetLastErrorCode(device);
            string errorMessage = NativeMethods.ospDeviceGetLastErrorMessage(device);
            throw new OSPException(lastError, errorMessage);
        }
    }
}
