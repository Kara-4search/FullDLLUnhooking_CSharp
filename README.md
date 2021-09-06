# FullDLLUnhooking_CSharp

Blog link: Not gonna update 

- A project fully unhook a DLL via file mapping
- Base on my other projects down below:
	1. [MappingInjection](https://github.com/Kara-4search/MappingInjection_CSharp)
	2. [HookDetection](https://github.com/Kara-4search/HookDetection_CSharp)
	3. [NewNtdllBypassInlineHook](https://github.com/Kara-4search/NewNtdllBypassInlineHook_CSharp)

- Just load a fresh copy of a DLL on disk, and copy the DLL's .text section to unhook.
- Steps
	1. Load a fresh new copy of ntdll.dll via file mapping.
	2. Get current process's hooked DLL address. (HookNtdll_address in Program.cs)
	3. Load the copy into memory and get a "BaseAddress".(FleshNtdll_address in Program.cs)
	4. Use the "BaseAddress" to find IMAGE_FILE_HEADER's address.
	5. Iterate all section via "IMAGE_FILE_HEADER_instance.NumberOfSections"
	6. **If a section contains ".text", replace the whole section from the clean copy base on the section's RVA.**
:) not my pic~
![avatar](https://raw.githubusercontent.com/Kara-4search/ProjectPics/main/FullDLLUnhooking_unhookpic.png)

- Tested on Win10/x64 works fine, to works on x86 you may need some modification about finding the IMAGE_SECTION_HEADER address.
- After unhooking you could do other stuff. enjoy~ :)

	
## Usage
1. Just launch through a white-list application
* I use another project(https://github.com/Kara-4search/HookDetection_CSharp) to test this.
* Before unhook, detect the hooking status.
	![avatar](https://raw.githubusercontent.com/Kara-4search/ProjectPics/main/FullDLLUnhooking_hookdetection.png)

* Unhooking the DLL
	![avatar](https://raw.githubusercontent.com/Kara-4search/ProjectPics/main/FullDLLUnhooking_unhooking.png)
	
* Detect again
	![avatar](https://raw.githubusercontent.com/Kara-4search/ProjectPics/main/FullDLLUnhooking_unhook%26detection.png)

- You could see the APIs have been unhooked
- **Although highly effective at detecting functions hooked with inline patching, this method returns a few false positives when enumerating hooked functions inside ntdll.dll, such as:**
**False Positives**
```
	NtGetTickCount
	NtQuerySystemTime
	NtdllDefWindowProc_A
	NtdllDefWindowProc_W
	NtdllDialogWndProc_A
	NtdllDialogWndProc_W
	ZwQuerySystemTime
```
**The above functions are not hooked.** 

## TO-DO list
- works on x86
- Maybe even unhook other DLL in userland.

## Update history

## Reference link:
	1. https://blog.csdn.net/qq_42253797/article/details/105090943
	2. https://blog.csdn.net/jiangqin115/article/details/79757041
	3. https://blog.csdn.net/huninglei3333/article/details/78725725
	4. https://idiotc4t.com/defense-evasion/reload-ntdll-.text-section
	5. https://www.ired.team/offensive-security/defense-evasion/how-to-unhook-a-dll-using-c++
	6. https://makosecblog.com/malware-dev/dll-unhooking-csharp/
	7. http://pinvoke.net/default.aspx/Structures.IMAGE_DOS_HEADER
	8. https://github.com/MakoSec
	9. https://github.com/TheWover/DInvoke
	10. https://docs.microsoft.com/en-us/windows/win32/api/psapi/nf-psapi-getmoduleinformation
	11. https://blog.csdn.net/qingzai_/article/details/76919402
	12. https://blog.csdn.net/tianxiayijia1998/article/details/50119435
	13. https://stackoverflow.com/questions/2658380/how-can-i-copy-unmanaged-data-in-c-sharp-and-how-fast-is-it/43589444
