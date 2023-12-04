@echo off
setlocal

:: 出力ファイル名を設定
set "outputFile=merged_cs_scripts.txt"

:: 出力ファイルが存在する場合は削除
if exist "%outputFile%" del "%outputFile%"

:: 現在のディレクトリとサブディレクトリから.csファイルを検索し、内容を出力ファイルに追加
for /r %%i in (*.cs) do (
    echo Merging file: %%i
    type "%%i" >> "%outputFile%"
    echo. >> "%outputFile%"
    echo -------------------------------- >> "%outputFile%"
)

echo Merging complete. Output file: %outputFile%
endlocal
