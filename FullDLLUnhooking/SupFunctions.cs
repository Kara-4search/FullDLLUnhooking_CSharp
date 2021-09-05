using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static FullDLLUnhooking.NativeFunctions;
using static FullDLLUnhooking.NativeStructs;

namespace FullDLLUnhooking
{
    class SupFunctions
    {
        /*
        public static Object[] LocateImageFileHeader(IntPtr BaseAddress, IntPtr CurrentHandle)
        {

            IMAGE_DOS_HEADER IMAGE_DOS_HEADER_instance = (IMAGE_DOS_HEADER)Marshal.PtrToStructure(
                BaseAddress,
                typeof(IMAGE_DOS_HEADER));

            IntPtr IMAGE_NT_HEADER64_address = BaseAddress + IMAGE_DOS_HEADER_instance.e_lfanew;
            IMAGE_NT_HEADER64 IMAGE_NT_HEADER64_instance = (IMAGE_NT_HEADER64)Marshal.PtrToStructure(
                IMAGE_NT_HEADER64_address,
                typeof(IMAGE_NT_HEADER64));

            IntPtr IMAGE_FILE_HEADER_address = (IntPtr)(IMAGE_NT_HEADER64_address + Marshal.SizeOf(IMAGE_NT_HEADER64_instance.Signature));
            IMAGE_FILE_HEADER IMAGE_FILE_HEADER_instance = (IMAGE_FILE_HEADER)Marshal.PtrToStructure(
                IMAGE_FILE_HEADER_address,
                typeof(IMAGE_FILE_HEADER));
            /*
            // int IMAGE_DIRECTORY_ENTRY_EXPORT = 0;
            IMAGE_DOS_HEADER IMAGE_DOS_HEADER_instance = new IMAGE_DOS_HEADER();
            IMAGE_DOS_HEADER_instance = (IMAGE_DOS_HEADER)FindObjectAddress(
                BaseAddress,
                IMAGE_DOS_HEADER_instance,
                CurrentHandle);

            IntPtr IMAGE_NT_HEADER64_address = (IntPtr)(BaseAddress.ToInt64() + (int)IMAGE_DOS_HEADER_instance.e_lfanew);
            IMAGE_NT_HEADER64 IMAGE_NT_HEADER64_instance = new IMAGE_NT_HEADER64();
            IMAGE_NT_HEADER64_instance = (IMAGE_NT_HEADER64)FindObjectAddress(
                IMAGE_NT_HEADER64_address,
                IMAGE_NT_HEADER64_instance,
                CurrentHandle);
   
            IntPtr IMAGE_FILE_HEADER_address = (IntPtr)(IMAGE_NT_HEADER64_address + Marshal.SizeOf(IMAGE_NT_HEADER64_instance.Signature));
            IMAGE_FILE_HEADER IMAGE_FILE_HEADER_instace = new IMAGE_FILE_HEADER();
            IMAGE_FILE_HEADER_instace = (IMAGE_FILE_HEADER)FindObjectAddress(
                IMAGE_FILE_HEADER_address,
                IMAGE_FILE_HEADER_instace,
                CurrentHandle);
            
            // Console.WriteLine(IMAGE_EXPORT_DIRECTORY_instance.AddressOfNames);
            // Console.WriteLine(ExportDirectoryRVA_address);
            // Console.WriteLine(IMAGE_NT_HEADER64_instance.Signature);
            // Console.WriteLine(IMAGE_NT_HEADER64_Address);
            // Console.WriteLine(IMAGE_DOS_HEADER_instance.e_lfanew);
            
            Object[] Object_list = new Object[2];
            Object_list[0] = IMAGE_DOS_HEADER_instance;
            Object_list[1] = IMAGE_FILE_HEADER_instance;

            return Object_list;
        }
        */

        public static IntPtr LoadFleshNtdllAddress()
        {
            string filename_path = "c:\\windows\\system32\\ntdll.dll";
            // IntPtr CurrentProcess_handle = Process.GetCurrentProcess().Handle;

            IntPtr NtdllFile_handle = CreateFileA(
                filename_path,
                EFileAccess.GenericRead,
                EFileShare.Read,
                IntPtr.Zero,
                EFileMode.OpenExisting,
                0,
                IntPtr.Zero);

            IntPtr NtdllMapping_handle = CreateFileMapping(
                NtdllFile_handle,
                IntPtr.Zero,
                FileMapProtection.PageReadonly | FileMapProtection.SectionImage,
                0,
                0,
                null);

            IntPtr NtdllMapViewOfFile_address = MapViewOfFile(NtdllMapping_handle, FileMapAccessType.Read, 0, 0, 0);

            return NtdllMapViewOfFile_address;
        }

        public static IntPtr LoadHookNtdllAddress(IntPtr CurrentProcess_handle)
        {

            IntPtr Module_handle = GetModuleHandle("ntdll.dll");

            MODULEINFO Module_info = new MODULEINFO();

            GetModuleInformation(
                CurrentProcess_handle,
                Module_handle,
                out Module_info,
                (uint)Marshal.SizeOf(Module_info));

            IntPtr HookNtdll_address = Module_info.lpBaseOfDll;

            return HookNtdll_address;
        }
    }
}
