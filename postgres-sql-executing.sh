#!/bin/bash

POD_NAME=$(kubectl get pods -l database=postgresql-database -o jsonpath='{.items[0].metadata.name}')

if [ -z "$POD_NAME" ]; then
  echo "PostgreSQL pod not found."
  exit 1
fi

echo "Successfully DB"
kubectl exec -it $POD_NAME -- psql -U myuser -c "DROP DATABASE test;"
kubectl exec -it $POD_NAME -- psql -U myuser -c "CREATE DATABASE test;"
if [ $? -eq 0 ]; then
  echo "Database test created successfully"
else 
  echo "Failed to create database"
  exit 1
fi
kubectl exec -it $POD_NAME -- psql -U myuser -d test -c "CREATE TABLE messsage(id SERIAL PRIMARY KEY, content VARCHAR(100) NOT NULL, createddate timestamp(6));"
echo "Succeed"