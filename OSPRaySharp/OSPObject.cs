using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPObjectHandle : OSPHandle
    {
        public OSPObjectHandle(IntPtr handle): base(handle) { }
        public OSPObjectHandle():base() { }

        protected override bool ReleaseHandle()
        {
            NativeMethods.ospRelease(handle);
            return true;
        }

        public static readonly OSPObjectHandle Empty = new OSPObjectHandle();
    }

    /// <summary>
    /// Base class for all OSPRay objects
    /// </summary>
    public abstract class OSPObject : IDisposable
    {
        internal abstract OSPObjectHandle Handle { get; }

       
        /// <summary>
        /// Sets a single value parameter by name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterId"></param>
        /// <param name="parameterValue"></param>
        public void SetParam<T>(string parameterId, T parameterValue) where T : unmanaged
        {
            OSPDataType dataType = OSPDataTypeUtil.GetDataTypeOrThrow<T>();
            unsafe
            {
                NativeMethods.ospSetParam(Handle, parameterId, dataType, &parameterValue);
            }
            OSPDevice.CheckLastDeviceError();
        }

        public OSPBounds Bounds
        {
            get
            {
                var bounds = NativeMethods.ospGetBounds(Handle);
                OSPDevice.CheckLastDeviceError();
                return bounds;
            }
        }

        /// <summary>
        /// Sets a string parameter
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="parameterValue"></param>
        public void SetParam(string parameterId, string parameterValue)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(parameterValue + "\0");
            SetParam(parameterId, OSPDataType.String, bytes);
        }

        /// <summary>
        /// Sets a tuple parameter with explicit data type i.e Vec2F, Box1F...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterId"></param>
        /// <param name="dataType"></param>
        /// <param name="values"></param>
        public unsafe void SetParam<T>(string parameterId, OSPDataType dataType, params T[] values) where T : unmanaged
        {
            
            fixed (T* pValues = values)
            {
                NativeMethods.ospSetParam(Handle, parameterId, dataType, pValues);
            }
            OSPDevice.CheckLastDeviceError();
        }

        /// <summary>
        /// Sets an array parameter as data object of a specific type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterId"></param>
        /// <param name="parameterValues"></param>
        public void SetArrayParam<T>(string parameterId, T[]? parameterValues) where T : unmanaged
        {
            if (parameterValues != null)
            {
                using (var dataArray = OSPDataFactory.CreateData1D(parameterValues))
                {
                    SetObjectParam(parameterId, dataArray);
                }
            }
            else
            {
                SetObjectParam(parameterId, null as OSPData<T>);
            }
            OSPDevice.CheckLastDeviceError();
        }

        public void SetObjectParam(string parameterId, OSPDataType dataType, OSPObject? parameterValue)
        {
            IntPtr value = parameterValue != null ? parameterValue.Handle.DangerousGetHandle() : IntPtr.Zero;
            unsafe
            {
                NativeMethods.ospSetParam(Handle, parameterId, dataType, &value);
            }
        }

        /// <summary>
        /// Sets an object parameter. The functions gets the native OSPRay object handle and pass it as parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterId"></param>
        /// <param name="parameterValue"></param>
        public void SetObjectParam<T>(string parameterId, T? parameterValue) where T : OSPObject
        {
            OSPDataType dataType = OSPDataTypeUtil.GetDataTypeOrThrow<T>();
            SetObjectParam(parameterId, dataType, parameterValue);
        }

        /// <summary>
        /// Sets an array of OSPRay objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterId"></param>
        /// <param name="parameterValues"></param>
        public void SetObjectArrayParam<T>(string parameterId, T[]? parameterValues) where T : OSPObject
        {
            if (parameterValues != null) 
            {
                using (var dataArray = OSPDataFactory.CreateObjectArray(parameterValues))
                {
                    SetObjectParam(parameterId, dataArray);
                }
            }
            else
            {
                SetObjectParam(parameterId, null as OSPData<IntPtr>);
            }
            OSPDevice.CheckLastDeviceError();
        }

        /// <summary>
        /// Resets a parameter to it's default. 
        /// </summary>
        /// <param name="parameterId"></param>
        public void RemoveParam(string parameterId)
        {
            NativeMethods.ospRemoveParam(Handle, parameterId);
            OSPDevice.CheckLastDeviceError();
        }

        /// <summary>
        /// Commit parameter changes to OSPRay.
        /// </summary>
        public void Commit()
        {
            NativeMethods.ospCommit(Handle);
            OSPDevice.CheckLastDeviceError();
        }

        public virtual void Dispose()
        {
            Handle.Dispose();
        }
    }
}
