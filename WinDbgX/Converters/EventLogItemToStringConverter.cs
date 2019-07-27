﻿using DebuggerEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WinDbgX.Models;

namespace WinDbgX.Converters {
	class EventLogItemToStringConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			var item = (EventLogItem)value;
			TargetThread thread;
			TargetProcess process;
			TargetModule module;

			switch (item.Type) {
				case EventLogItemType.ProcessCreate:
					process = (TargetProcess)item.EventData;
					return $"Process {process.PID} created";

				case EventLogItemType.ThreadCreate:
					thread = (TargetThread)item.EventData;
					return $"Thread {thread.TID} created in process {thread.Process.PID}";

				case EventLogItemType.ThreadExit:
					thread = (TargetThread)item.EventData;
					return $"Thread {thread.TID} exited with code {thread.ExitCode}";

				case EventLogItemType.ProcessExit:
					process = (TargetProcess)item.EventData;
					return $"Process {process.PID} exited with code {process.ExitCode}";

				case EventLogItemType.ModuleLoad:
					module = (TargetModule)item.EventData;
					return $"Module {module.ImageName} was loaded in process {module.Process.PID} at address 0x{module.BaseAddress:X}";

				case EventLogItemType.ModuleUnload:
					module = (TargetModule)item.EventData;
					return $"Module {module.Name} was unloaded from process {module.Process.PID}";

			}
			return Binding.DoNothing;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
