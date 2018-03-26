using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: AssemblyProduct("SDR#")]
[assembly: CompilationRelaxations(8)]
[assembly: AssemblyCopyright("Copyright © Youssef TOUIL 2012")]
[assembly: AssemblyTitle("RTL-SDR Controller")]
[assembly: AssemblyDescription("USB interface for RTL2832U based DVB-T dongles")]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[assembly: AssemblyVersion("0.0.0.0")]
