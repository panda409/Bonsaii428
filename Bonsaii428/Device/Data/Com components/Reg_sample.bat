@echo off
D:\SDK\VB6\COM\gacutil /i D:\SDK\VB6\COM\Riss.Devices.dll
%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\regasm D:\SDK\VB6\COM\Riss.Devices.dll /tlb:D:\SDK\VB6\COM\Riss.Devices.tlb
pause