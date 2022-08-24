@echo off
echo ――――――――――――――――――
echo  ２．DLLをユーザ環境のDLLフォルダにコピー
echo ――――――――――――――――――

rem カレントディレクトリを設定
SET PROJECTDIR="%CD%"

rem DLLフォルダを設定
SET DLLPATH="%CD%\DLL"

rem 初期化
set USR_INPUT_STR=
rem 入力要求
set /P USR_INPUT_STR="コピーする先（ユーザ環境「DLL」フォルダフルパス）を入力してください: "
rem 入力値echo
echo 入力した文字は %USR_INPUT_STR% です

IF NOT EXIST "%USR_INPUT_STR%" (
echo;
echo コピー先が存在しません。
echo;
) ELSE (
rem ディレクトリを移動
CD %USR_INPUT_STR%
rem ファイル削除
DEL /F /Q "%USR_INPUT_STR%\*.dll"
DEL /F /Q "%USR_INPUT_STR%\*.pdb"
DEL /F /Q "%USR_INPUT_STR%\*.xml"
rem ファイルコピー
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.BizProperty.dll" Jpsys.HaishaManageV10.BizProperty.dll
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.BizProperty.pdb" Jpsys.HaishaManageV10.BizProperty.pdb
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.ComLib.dll" Jpsys.HaishaManageV10.ComLib.dll
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.ComLib.pdb" Jpsys.HaishaManageV10.ComLib.pdb
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.Frame.dll" Jpsys.HaishaManageV10.Frame.dll
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.Frame.pdb" Jpsys.HaishaManageV10.Frame.pdb
COPY "%PROJECTDIR%\DLL\SendGrid.dll" SendGrid.dll
COPY "%PROJECTDIR%\DLL\SendGrid.pdb" SendGrid.pdb
COPY "%PROJECTDIR%\DLL\SendGrid.xml" SendGrid.xml
COPY "%PROJECTDIR%\DLL\Newtonsoft.Json.dll" Newtonsoft.Json.dll
COPY "%PROJECTDIR%\DLL\Newtonsoft.Json.xml" Newtonsoft.Json.xml
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.FrameLib.dll" Jpsys.HaishaManageV10.FrameLib.dll
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.FrameLib.pdb" Jpsys.HaishaManageV10.FrameLib.pdb
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.FrameLib.xml" Jpsys.HaishaManageV10.FrameLib.xml
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.Model.dll" Jpsys.HaishaManageV10.Model.dll
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.Model.pdb" Jpsys.HaishaManageV10.Model.pdb
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.Property.dll" Jpsys.HaishaManageV10.Property.dll
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.Property.pdb" Jpsys.HaishaManageV10.Property.pdb
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.ReportAppendix.dll" Jpsys.HaishaManageV10.ReportAppendix.dll
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.ReportAppendix.pdb" Jpsys.HaishaManageV10.ReportAppendix.pdb
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.ReportDAL.dll" Jpsys.HaishaManageV10.ReportDAL.dll
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.ReportDAL.pdb" Jpsys.HaishaManageV10.ReportDAL.pdb
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.ReportFrame.dll" Jpsys.HaishaManageV10.ReportFrame.dll
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.ReportFrame.pdb" Jpsys.HaishaManageV10.ReportFrame.pdb
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.ReportModel.dll" Jpsys.HaishaManageV10.ReportModel.dll
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.ReportModel.dll" Jpsys.HaishaManageV10.ReportModel.pdb
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.SQLServerDAL.dll" Jpsys.HaishaManageV10.SQLServerDAL.dll
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.SQLServerDAL.dll" Jpsys.HaishaManageV10.SQLServerDAL.pdb
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.VBReport.dll" Jpsys.HaishaManageV10.VBReport.dll
COPY "%PROJECTDIR%\DLL\Jpsys.HaishaManageV10.VBReport.dll" Jpsys.HaishaManageV10.VBReport.pdb
)

pause;