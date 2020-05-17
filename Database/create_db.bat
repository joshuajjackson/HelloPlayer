echo off

rem batch file to run a script to create a db
rem 9/5/2019

sqlcmd -S localhost\sqlexpress -E -i HelloPlayerDB.sql

echo . 
echo if no error messages appear DB was created
pause
