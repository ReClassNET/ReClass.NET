using System;
using System.Collections.Generic;

namespace ReClassNET.Memory
{
	public interface IProcessReader : IRemoteMemoryReader
	{
		Section GetSectionToPointer(IntPtr address);

		Module GetModuleToPointer(IntPtr address);

		Module GetModuleByName(string name);

		bool EnumerateRemoteSectionsAndModules(out List<Section> sections, out List<Module> modules);
	}
}
