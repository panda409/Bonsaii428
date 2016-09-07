@echo off
{replace with your path}\gacutil /i Riss.Devices.dll
%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\regasm {replace with your path}\Riss.Devices.dll /tlb:{replace with your path}\Riss.Devices.tlb
pause