#!/bin/bash

RABBIT_MQ_HOST="rabbitmq"
RABBIT_MQ_PORT=15672
RABBIT_MQ_USER="guest"
RABBIT_MQ_PASSWORD="guest"
EXCHANGE_NAME="message-exchange"
QUEUE_NAME="message_queue"

check_health_rabbitmq() {
  /opt/rabbitmq/cli/rabbitmqadmin -u $RABBIT_MQ_USER -p $RABBIT_MQ_PASSWORD -H $RABBIT_MQ_HOST -P $RABBIT_MQ_PORT list exchanges name | grep "message-exchange"
  if [ $? -ne 0 ]; then
    echo "Message Exchange Not Exists"
    return 1
  fi
  /opt/rabbitmq/cli/rabbitmqadmin -u $RABBIT_MQ_USER -p $RABBIT_MQ_PASSWORD -H $RABBIT_MQ_HOST -P $RABBIT_MQ_PORT list queues name | grep "message_queue"
  if [ $? -ne 0 ]; then
    echo "Message Queue Not Exists"
    return 1
  fi
  /opt/rabbitmq/cli/rabbitmqadmin -u $RABBIT_MQ_USER -p $RABBIT_MQ_PASSWORD -H $RABBIT_MQ_HOST -P $RABBIT_MQ_PORT list bindings source destination routing_key | grep "message-exchange" | grep "message_queue" 
  if [ $? -ne 0 ]; then
    echo "Message Queue Not Bound To Message Exchange"
    return 1
  fi
  echo "Exchange, Queue, and Binding are all healthy."
  return 0
}

until check_health_rabbitmq; do
  echo "Service Unhealthy"
  sleep 5  # optional sleep to avoid constant re-checking
done
echo "Service Healthy"