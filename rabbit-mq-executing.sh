#!/bin/bash

POD_NAME=$(kubectl get pods -l rabbitmq=rabbit-mq-server -o jsonpath='{.items[0].metadata.name}')

if [ -z "$POD_NAME" ]; then
  echo "RabbitMQ pod not found."
  sleep 10
  exit 1
fi

echo "Waiting"
sleep 5

kubectl exec -it $POD_NAME -c rabbit-mq-server -- rabbitmqctl change_password guest password
if [ $? -eq 0 ]; then
  echo "Password changed successfully"
  sleep 10
else 
  echo "Failed to change password"
  sleep 10
fi

