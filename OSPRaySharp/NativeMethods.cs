using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal static partial class NativeMethods
    {
        private const string LibraryName = "ospray";

        internal static string ospDeviceGetLastErrorMessage(OSPDeviceHandle device)
        {
            var stringPtr = ospDeviceGetLastErrorMsg(device);
            return Marshal.PtrToStringAnsi(stringPtr) ?? "";
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int ospInit(ref int argc, [In, Out] string[] argv);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ospShutdown();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern OSPDeviceHandle ospNewDevice(string type);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ospDeviceCommit(OSPDeviceHandle handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ospSetCurrentDevice(OSPDeviceHandle handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern OSPDeviceHandle ospGetCurrentDevice();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ospDeviceRetain(OSPDeviceHandle handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ospDeviceRelease(IntPtr handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal unsafe static extern void ospDeviceSetParam(OSPDeviceHandle device, string id, [MarshalAs(UnmanagedType.U4)] OSPDataType type, void* mem);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal unsafe static extern void ospDeviceRemoveParam(OSPDeviceHandle device, string id);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern long ospDeviceGetProperty(OSPDeviceHandle device, [MarshalAs(UnmanagedType.U4)] OSPDeviceProperty property);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ospDeviceGetLastErrorMsg(OSPDeviceHandle device);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ospDeviceGetLastErrorCode(OSPDeviceHandle device);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int ospLoadModule(string name);


        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ospCommit(OSPObjectHandle handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ospRetain(OSPObjectHandle handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ospRelease(IntPtr handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal unsafe static extern void ospSetParam(OSPObjectHandle handle, string id, [MarshalAs(UnmanagedType.U4)] OSPDataType type, void* mem);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void ospRemoveParam(OSPObjectHandle handle, string id);


        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern OSPFrameBufferHandle ospNewFrameBuffer(int size_x, int size_y, OSPFrameBufferFormat format, [MarshalAs(UnmanagedType.U4)] OSPFrameBufferChannel channels);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern void* ospMapFrameBuffer(OSPFrameBufferHandle frameBuffer, OSPFrameBufferChannel channel);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern void ospUnmapFrameBuffer(void* mapped, OSPFrameBufferHandle frameBuffer);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float ospGetVariance(OSPFrameBufferHandle handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ospResetAccumulation(OSPFrameBufferHandle handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern OSPImageOperationHandle ospNewImageOperation(string type);


        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern OSPDataHandle ospNewSharedData(
            void* sharedData,
            OSPDataType type,
            [MarshalAs(UnmanagedType.U8)] long numItems1,
            [MarshalAs(UnmanagedType.U8)] long byteStride1,
            [MarshalAs(UnmanagedType.U8)] long numItems2,
            [MarshalAs(UnmanagedType.U8)] long byteStride2,
            [MarshalAs(UnmanagedType.U8)] long numItems3,
            [MarshalAs(UnmanagedType.U8)] long byteStride3);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern OSPDataHandle ospNewData(OSPDataType type,
            [MarshalAs(UnmanagedType.U8)] long numItems1,
            [MarshalAs(UnmanagedType.U8)] long numItems2,
            [MarshalAs(UnmanagedType.U8)] long numItems3);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ospCopyData(
            OSPDataHandle source,
            OSPDataHandle destination,
            [MarshalAs(UnmanagedType.U8)] long destinationIndex1,
            [MarshalAs(UnmanagedType.U8)] long destinationIndex2,
            [MarshalAs(UnmanagedType.U8)] long destinationIndex3);


        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern OSPCameraHandle ospNewCamera(string type);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern OSPWorldHandle ospNewWorld();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern OSPRendererHandle ospNewRenderer(string type);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern OSPFutureHandle ospRenderFrame(OSPFrameBufferHandle frameBuffer, OSPRendererHandle renderer, OSPCameraHandle camera, OSPWorldHandle world);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float ospRenderFrameBlocking(OSPFrameBufferHandle frameBuffer, OSPRendererHandle renderer, OSPCameraHandle camera, OSPWorldHandle world);


        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float ospGetProgress(OSPFutureHandle handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ospCancel(OSPFutureHandle handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ospWait(OSPFutureHandle handle, OSPSyncEvent syncEvent);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ospIsReady(OSPFutureHandle handle, OSPSyncEvent syncEvent);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float ospGetTaskDuration(OSPFutureHandle handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern OSPGeometryHandle ospNewGeometry(string type);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern OSPLightHandle ospNewLight(string type);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern OSPGeometricModelHandle ospNewGeometricModel(OSPGeometryHandle geometry);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern OSPMaterialHandle ospNewMaterial(string? ignored, string materialType);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern OSPGroupHandle ospNewGroup();


        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern OSPInstanceHandle ospNewInstance(OSPGroupHandle group);
    }
}
