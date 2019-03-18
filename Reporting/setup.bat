@ECHO OFF
set mongodb=%1
set klov=%2
start cmd /K "%mongodb%"
start cmd /K "java -jar %klov%\klov-0.2.5.jar"
