using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    /// <summary>
    /// Device specific options
    /// </summary>
    public struct OSPDeviceOptions
    {
        /// <summary>
        /// The maximum logging level or null for default
        /// </summary>
        public LogLevel? LogLevel { get; set; }
        /// <summary>
        /// Threat warning as error or null for default
        /// </summary>
        public bool? WarningAsError { get; set; }
        /// <summary>
        /// Number of threads OSPRay should use or null for default
        /// </summary>
        public int? NumberOfThreads { get; set; }
        /// <summary>
        /// Bind software threads to hardware threads or null for default
        /// </summary>
        public bool? SetThreadAffinity { get; set; }
        /// <summary>
        /// Set debug mode or null for default
        /// </summary>
        public bool? DebugMode { get; set; }
    }


    public class OSPLibrary : IDisposable
    {
        public OSPLibrary(params string[] args)
        {
            int length = args.Length;
            int error = NativeMethods.ospInit(ref length, args);
            if (error != 0)
            {
                throw new OSPException(error,
                    NativeMethods.ospDeviceGetLastErrorMessage(OSPDeviceHandle.Empty));
            }

            // get the default device
            var handle = NativeMethods.ospGetCurrentDevice();
            if (handle.IsInvalid)
                throw new OSPException(
                    NativeMethods.ospDeviceGetLastErrorCode(handle),
                    NativeMethods.ospDeviceGetLastErrorMessage(handle));

            Device = new OSPDevice(handle);
        }

        /// <summary>
        /// Gets the default device
        /// </summary>
        public OSPDevice Device
        {
            get;
        }

        public bool TryLoadModule(string name)
        {
            try
            {
                LoadModule(name);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Loads an additional OSPRay module.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="OSPException"></exception>
        public void LoadModule(string name)
        {
            int error = NativeMethods.ospLoadModule(name);
            if (error != 0)
            {
                throw new OSPException(error,
                    NativeMethods.ospDeviceGetLastErrorMessage(OSPDeviceHandle.Empty));
            }
        }

        /// <summary>
        /// Creates a cpu device with it's default parameters.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="warningAsError"></param>
        /// <param name="numberOfThreads"></param>
        /// <param name="setThreadAffinity"></param>
        /// <param name="debugMode"></param>
        /// <returns></returns>
        public OSPDevice CreateCPUDevice(LogLevel? logLevel = null,
                                         bool? warningAsError = null,
                                         int? numberOfThreads = null,
                                         bool? setThreadAffinity = null,
                                         bool? debugMode = null)
        {
            var device = CreateDevice("cpu");
            try
            {
                device.SetParam("logLevel", logLevel);
                device.SetParam("warningAsError", warningAsError);
                device.SetParam("numThreads", numberOfThreads);
                device.SetParam("setAffinity", setThreadAffinity);
                device.SetParam("debug", debugMode);
                device.Commit();
            }
            catch (Exception) {
                device.Dispose();
                throw;
            }

            return device;
        }


        /// <summary>
        /// Creates a device of a specific type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="OSPException"></exception>
        public OSPDevice CreateDevice(string type = "default")
        {
            var handle = NativeMethods.ospNewDevice(type);
            if (handle.IsInvalid)
            {
                throw new OSPException(
                    NativeMethods.ospDeviceGetLastErrorCode(handle),
                    NativeMethods.ospDeviceGetLastErrorMessage(handle));
            }
            return new OSPDevice(handle);
        }

        /// <summary>
        /// Shutdown the OSPRay library
        /// </summary>
        public void Dispose()
        {
            NativeMethods.ospShutdown();
        }
    }
}
