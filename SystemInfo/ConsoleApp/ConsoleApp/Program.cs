// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
long totalBytesOfMemoryUsed = currentProcess.WorkingSet64 / 1024 / 1024;
Console.WriteLine(totalBytesOfMemoryUsed);
Console.Read();