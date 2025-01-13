#!/bin/bash

aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 975050152649.dkr.ecr.us-east-1.amazonaws.com

kubectl create secret docker-registry ecr-secret --docker-server=975050152649.dkr.ecr.us-east-1.amazonaws.com --docker-username=AWS --docker-password=$(aws ecr get-login-password --region us-east-1) --docker-email=mongtuyhoa@gmail.com


# Apply the deployment.yaml file
kubectl apply -f deployment.yaml

echo "Waiting for RabbitMQ to be ready..."
kubectl wait --for=condition=available --timeout=600s deployment/rabbit-mq-server

echo "Waiting for PostgreSQL to be ready..."
kubectl wait --for=condition=available --timeout=600s deployment/postgresql-database

echo "Wait To Executing Rabbit MQ"

# Check if RabbitMQ pod is running and execute script
if kubectl get pods -l rabbitmq=rabbit-mq-server -o jsonpath='{.items[0].status.phase}' | grep -q 'Running'; then
    echo "RabbitMQ is running. Executing script..."
    ./rabbit-mq-executing.sh
else
    echo "RabbitMQ is not running. Exiting..."
    exit 1
fi

echo "Wait To Executing PostgreSQL"

# Check if PostgreSQL pod is running and execute script
if kubectl get pods -l database=postgresql-database -o jsonpath='{.items[0].status.phase}' | grep -q 'Running'; then
    echo "PostgreSQL is running. Executing script..."
    ./postgres-sql-executing.sh
else
    echo "PostgreSQL is not running. Exiting..."
    exit 1
fi