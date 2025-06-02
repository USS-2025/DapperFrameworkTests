
REM SET SETTING_NAME_CONNECTION_STRING="OracleVirtaulBoxDb"
SET CONNECTION_STRING="User Id=system;Password=oracle;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.0.200)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=freepdb1)))"
SET PROVIDER="Oracle.EntityFrameworkCore"

SET schema1=SYSTEM

REM dotnet ef dbcontext scaffold ConnectionStrings:OracleVirtaulBoxDb %PROVIDER% --table HELP --table mview$_adv_parameters
REM dotnet ef dbcontext scaffold Name=ConnectionStrings:%SETTING_NAME_CONNECTION_STRING% %PROVIDER% --schema %schema1% --context-dir Data --output-dir Models --data-annotations
dotnet ef dbcontext scaffold %CONNECTION_STRING% %PROVIDER% --schema %schema1% --context-dir Data --output-dir Models --data-annotations --use-database-names --force 

PAUSE
