#!/bin/bash

/usr/local/bin/docker-entrypoint.sh postgres &
DB_HOST="postgresql"
DB_PORT=5432
DB_NAME="mydb"
DB_USER="myuser"
export DB_PASSWORD="mypassword"
check_postgresql_connection() {
  export PGPASSWORD="$DB_PASSWORD"  # Set the password for the session
  pg_isready -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d "$DB_NAME" &> /dev/null
  return $?
}

echo "Waiting for PostgreSQL to be ready..."
until check_postgresql_connection; do
  echo "PostgreSQL is not ready, waiting..."
  sleep 5
done

echo "PostgreSQL is ready..."

QUERY_CONTENT=$(cat /opt/PostgresqlSeedsVolume/postgresql_query_seeds.txt)
psql -h $DB_HOST -p $DB_PORT -U $DB_USER -d $DB_NAME -c "$QUERY_CONTENT"
# Check if the insert was successful
if [ $? -eq 0 ]; then
    echo "Data inserted successfully into $DB_NAME."
else
    echo "Failed to insert data into $DB_NAME."
fi

wait $(jobs -p)