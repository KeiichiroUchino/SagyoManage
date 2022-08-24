@echo off
echo ――――――――――――――――――
echo  １．最新DLLをDLLフォルダにコピー
echo ――――――――――――――――――

rem カレントディレクトリを設定
SET PROJECTDIR="%CD%"

rem DLLフォルダを設定
SET DLLPATH="%CD%\DLL"

rem ディレクトリを移動
CD %DLLPATH%

rem ファイル削除
DEL /F /Q "%DLLPATH%\*.dll"
DEL /F /Q "%DLLPATH%\*.pdb"
DEL /F /Q "%DLLPATH%\*.xml"

rem ファイルコピー
COPY "%PROJECTDIR%\BizProperty\bin\Debug\Jpsys.HaishaManageV10.BizProperty.dll" Jpsys.HaishaManageV10.BizProperty.dll
COPY "%PROJECTDIR%\BizProperty\bin\Debug\Jpsys.HaishaManageV10.BizProperty.pdb" Jpsys.HaishaManageV10.BizProperty.pdb
COPY "%PROJECTDIR%\ComLib\bin\Debug\Jpsys.HaishaManageV10.ComLib.dll" Jpsys.HaishaManageV10.ComLib.dll
COPY "%PROJECTDIR%\ComLib\bin\Debug\Jpsys.HaishaManageV10.ComLib.pdb" Jpsys.HaishaManageV10.ComLib.pdb
COPY "%PROJECTDIR%\Frame\bin\Debug\Jpsys.HaishaManageV10.Frame.dll" Jpsys.HaishaManageV10.Frame.dll
COPY "%PROJECTDIR%\Frame\bin\Debug\Jpsys.HaishaManageV10.Frame.pdb" Jpsys.HaishaManageV10.Frame.pdb
COPY "%PROJECTDIR%\Frame\bin\Debug\SendGrid.dll" SendGrid.dll
COPY "%PROJECTDIR%\Frame\bin\Debug\SendGrid.pdb" SendGrid.pdb
COPY "%PROJECTDIR%\Frame\bin\Debug\SendGrid.xml" SendGrid.xml
COPY "%PROJECTDIR%\Frame\bin\Debug\Newtonsoft.Json.dll" Newtonsoft.Json.dll
COPY "%PROJECTDIR%\Frame\bin\Debug\Newtonsoft.Json.xml" Newtonsoft.Json.xml
COPY "%PROJECTDIR%\FrameLib\bin\Debug\Jpsys.HaishaManageV10.FrameLib.dll" Jpsys.HaishaManageV10.FrameLib.dll
COPY "%PROJECTDIR%\FrameLib\bin\Debug\Jpsys.HaishaManageV10.FrameLib.pdb" Jpsys.HaishaManageV10.FrameLib.pdb
COPY "%PROJECTDIR%\FrameLib\bin\Debug\Jpsys.HaishaManageV10.FrameLib.xml" Jpsys.HaishaManageV10.FrameLib.xml
COPY "%PROJECTDIR%\Model\bin\Debug\Jpsys.HaishaManageV10.Model.dll" Jpsys.HaishaManageV10.Model.dll
COPY "%PROJECTDIR%\Model\bin\Debug\Jpsys.HaishaManageV10.Model.pdb" Jpsys.HaishaManageV10.Model.pdb
COPY "%PROJECTDIR%\Property\bin\Debug\Jpsys.HaishaManageV10.Property.dll" Jpsys.HaishaManageV10.Property.dll
COPY "%PROJECTDIR%\Property\bin\Debug\Jpsys.HaishaManageV10.Property.pdb" Jpsys.HaishaManageV10.Property.pdb
COPY "%PROJECTDIR%\ReportAppendix\bin\Debug\Jpsys.HaishaManageV10.ReportAppendix.dll" Jpsys.HaishaManageV10.ReportAppendix.dll
COPY "%PROJECTDIR%\ReportAppendix\bin\Debug\Jpsys.HaishaManageV10.ReportAppendix.pdb" Jpsys.HaishaManageV10.ReportAppendix.pdb
COPY "%PROJECTDIR%\ReportDAL\bin\Debug\Jpsys.HaishaManageV10.ReportDAL.dll" Jpsys.HaishaManageV10.ReportDAL.dll
COPY "%PROJECTDIR%\ReportDAL\bin\Debug\Jpsys.HaishaManageV10.ReportDAL.pdb" Jpsys.HaishaManageV10.ReportDAL.pdb
COPY "%PROJECTDIR%\ReportFrame\bin\Debug\Jpsys.HaishaManageV10.ReportFrame.dll" Jpsys.HaishaManageV10.ReportFrame.dll
COPY "%PROJECTDIR%\ReportFrame\bin\Debug\Jpsys.HaishaManageV10.ReportFrame.pdb" Jpsys.HaishaManageV10.ReportFrame.pdb
COPY "%PROJECTDIR%\ReportModel\bin\Debug\Jpsys.HaishaManageV10.ReportModel.dll" Jpsys.HaishaManageV10.ReportModel.dll
COPY "%PROJECTDIR%\ReportModel\bin\Debug\Jpsys.HaishaManageV10.ReportModel.dll" Jpsys.HaishaManageV10.ReportModel.pdb
COPY "%PROJECTDIR%\SQLServerDAL\bin\Debug\Jpsys.HaishaManageV10.SQLServerDAL.dll" Jpsys.HaishaManageV10.SQLServerDAL.dll
COPY "%PROJECTDIR%\SQLServerDAL\bin\Debug\Jpsys.HaishaManageV10.SQLServerDAL.dll" Jpsys.HaishaManageV10.SQLServerDAL.pdb
COPY "%PROJECTDIR%\VBReport\bin\Debug\Jpsys.HaishaManageV10.VBReport.dll" Jpsys.HaishaManageV10.VBReport.dll
COPY "%PROJECTDIR%\VBReport\bin\Debug\Jpsys.HaishaManageV10.VBReport.dll" Jpsys.HaishaManageV10.VBReport.pdb

pause;