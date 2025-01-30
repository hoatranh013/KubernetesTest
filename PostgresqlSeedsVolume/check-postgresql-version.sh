#!/bin/bash
DB_HOST="postgresql"
DB_PORT="5432"
DB_NAME="mydb"
DB_USER="myuser"
DB_PASSWORD="mypassword"
QUERY="SELECT value FROM \"version\" LIMIT 1;"
VALUE_FILE="/opt/PostgresqlSeedsVolume/postgresql-version.txt"
VALUE_FILE_CONTENT=$(<"$VALUE_FILE")

QUERY_RESULT=$(psql -h "$DB_HOST" -U "$DB_USER" -d "$DB_NAME" -t -c "$QUERY" | xargs)
# Compare the query result with the value in the file
if [[ "$QUERY_RESULT" == "$VALUE_FILE_CONTENT" ]]; then
  echo "The query result matches the value of current version."
else
  echo "The query result does NOT match the value of current version."
  exit 1
fi