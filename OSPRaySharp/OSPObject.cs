using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPObjectHandle : OSPHandle
    {
        protected override bool ReleaseHandle()
        {
            NativeMethods.ospRelease(handle);
            return true;
        }

        public static readonly OSPObjectHandle Empty = new OSPObjectHandle();
    }

    public abstract class OSPObject : IDisposable
    {
        internal abstract OSPObjectHandle Handle { get; }

       
        public void SetParam<T>(string parameterId, T parameterValue) where T : unmanaged
        {
            OSPDataType dataType = OSPDataTypeUtil.GetDataTypeOrThrow<T>();
            unsafe
            {
                NativeMethods.ospSetParam(Handle, parameterId, dataType, &parameterValue);
            }
            OSPDevice.CheckLastDeviceError();
        }

        internal unsafe void SetParam<T>(string parameterId, OSPDataType dataType, params T[] values) where T : unmanaged
        {
            
            fixed (T* pValues = values)
            {
                NativeMethods.ospSetParam(Handle, parameterId, dataType, pValues);
            }
            OSPDevice.CheckLastDeviceError();
        }

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

        /// <summary>
        /// Sets an object parameter. The functions gets the native OSPRay object handle and pass it as parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterId"></param>
        /// <param name="parameterValue"></param>
        public void SetObjectParam<T>(string parameterId, T? parameterValue) where T : OSPObject
        {
            OSPDataType dataType = OSPDataTypeUtil.GetDataTypeOrThrow<T>();
            IntPtr value = parameterValue != null ? parameterValue.Handle.DangerousGetHandle() : IntPtr.Zero;
            unsafe
            {
                NativeMethods.ospSetParam(Handle, parameterId, dataType, &value);
            }
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
