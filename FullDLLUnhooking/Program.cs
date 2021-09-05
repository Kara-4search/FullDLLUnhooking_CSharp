using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static FullDLLUnhooking.NativeStructs;
using static FullDLLUnhooking.NativeFunctions;


namespace FullDLLUnhooking
{
    class Program
    {
		public static void InvokeMain()
        {
			IntPtr CurrentProcess_handle = Process.GetCurrentProcess().Handle;

			IntPtr FleshNtdll_address = SupFunctions.LoadFleshNtdllAddress();
			IntPtr HookNtdll_address = SupFunctions.LoadHookNtdllAddress(CurrentProcess_handle);

			IMAGE_DOS_HEADER IMAGE_DOS_HEADER_instance = (IMAGE_DOS_HEADER)Marshal.PtrToStructure(
				HookNtdll_address,
				typeof(IMAGE_DOS_HEADER));

			IntPtr IMAGE_NT_HEADER64_address = HookNtdll_address + IMAGE_DOS_HEADER_instance.e_lfanew;
			IMAGE_NT_HEADER64 IMAGE_NT_HEADER64_instance = (IMAGE_NT_HEADER64)Marshal.PtrToStructure(
				IMAGE_NT_HEADER64_address,
				typeof(IMAGE_NT_HEADER64));

			IntPtr IMAGE_FILE_HEADER_address = (IntPtr)(IMAGE_NT_HEADER64_address + Marshal.SizeOf(IMAGE_NT_HEADER64_instance.Signature));
			IMAGE_FILE_HEADER IMAGE_FILE_HEADER_instance = (IMAGE_FILE_HEADER)Marshal.PtrToStructure(
				IMAGE_FILE_HEADER_address,
				typeof(IMAGE_FILE_HEADER));

			IntPtr IMAGE_SECTION_HEADER_address = (
				HookNtdll_address + IMAGE_DOS_HEADER_instance.e_lfanew + 
				Marshal.SizeOf(typeof(IMAGE_NT_HEADER64)));

			IMAGE_SECTION_HEADER IMAGE_SECTION_HEADER_instance = new IMAGE_SECTION_HEADER();

			for (int count = 0; count < IMAGE_FILE_HEADER_instance.NumberOfSections; count++)
            {
				IMAGE_SECTION_HEADER_instance = (IMAGE_SECTION_HEADER)Marshal.PtrToStructure(
					IMAGE_SECTION_HEADER_address + count * Marshal.SizeOf(IMAGE_SECTION_HEADER_instance), 
					typeof(IMAGE_SECTION_HEADER));

				Console.WriteLine(IMAGE_SECTION_HEADER_instance.SectionName);

				if (IMAGE_SECTION_HEADER_instance.SectionName.Contains(".text"))
                {
					uint oldProtect = 0;

					IntPtr HookNtdllSection_address = IntPtr.Add(HookNtdll_address, (int)IMAGE_SECTION_HEADER_instance.VirtualAddress);
					IntPtr FleshNtdllSection_address = IntPtr.Add(FleshNtdll_address, (int)IMAGE_SECTION_HEADER_instance.VirtualAddress);
					
					VirtualProtect(
						HookNtdllSection_address, 
						(UIntPtr)IMAGE_SECTION_HEADER_instance.VirtualSize, 
						0x40, 
						out oldProtect);

					CopyMemory(HookNtdllSection_address, FleshNtdllSection_address, IMAGE_SECTION_HEADER_instance.VirtualSize);
					
					VirtualProtect(
						HookNtdllSection_address, 
						(UIntPtr)IMAGE_SECTION_HEADER_instance.VirtualSize, 
						oldProtect, 
						out oldProtect);
				}
			}

			Console.WriteLine(IMAGE_FILE_HEADER_instance.NumberOfSections);


		}

        static void Main(string[] args)
        {

			InvokeMain();
			
		}
    }
}
