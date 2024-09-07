
echo "running script for mssql db creation: $mssql_db"

sqlcmd -s localhost -U sa -P $mssql_password -q "create database $mssql_db;"

echo "script completed successfully...."