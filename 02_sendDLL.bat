@echo off
echo �\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\
echo  �Q�DDLL�����[�U����DLL�t�H���_�ɃR�s�[
echo �\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\

rem �J�����g�f�B���N�g����ݒ�
SET PROJECTDIR="%CD%"

rem DLL�t�H���_��ݒ�
SET DLLPATH="%CD%\DLL"

rem ������
set USR_INPUT_STR=
rem ���͗v��
set /P USR_INPUT_STR="�R�s�[�����i���[�U���uDLL�v�t�H���_�t���p�X�j����͂��Ă�������: "
rem ���͒lecho
echo ���͂��������� %USR_INPUT_STR% �ł�

IF NOT EXIST "%USR_INPUT_STR%" (
echo;
echo �R�s�[�悪���݂��܂���B
echo;
) ELSE (
rem �f�B���N�g�����ړ�
CD %USR_INPUT_STR%
rem �t�@�C���폜
DEL /F /Q "%USR_INPUT_STR%\*.dll"
DEL /F /Q "%USR_INPUT_STR%\*.pdb"
DEL /F /Q "%USR_INPUT_STR%\*.xml"
rem �t�@�C���R�s�[
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