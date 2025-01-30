#!/bin/bash

/usr/local/bin/docker-entrypoint.sh rabbitmq-server &
RABBIT_MQ_HOST="rabbitmq"
RABBIT_MQ_PORT=15672
RABBIT_MQ_USER="guest"
RABBIT_MQ_PASSWORD="guest"
EXCHANGE_NAME="message-exchange"
QUEUE_NAME="message_queue"
ROUTING_KEY="info"
EXCHANGE_TYPE="direct"
check_rabbitmq_connection() {
  local url="http://${RABBIT_MQ_HOST}:15672/api/overview"
  response=$(curl -u "${RABBIT_MQ_USER}:${RABBIT_MQ_PASSWORD}" -s -o /dev/null -w "%{http_code}" "$url")
  echo "$response"  # Return the response code
}

echo "Starting RabbitMQ Services..."
until [ "$(check_rabbitmq_connection)" -eq 200 ]; do
  echo "Waiting for RabbitMQ Connection..."
  sleep 2
done

echo "RabbitMQ Starting Connection..."
mkdir -p /opt/rabbitmq/cli
curl -o /opt/rabbitmq/cli/rabbitmqadmin http://rabbitmq:15672/cli/rabbitmqadmin


if [ "$(check_rabbitmq_connection)" -eq 200 ]; then
  rabbitmq-plugins enable rabbitmq_management
  chmod +x /opt/rabbitmq/cli/rabbitmqadmin
  /opt/rabbitmq/cli/rabbitmqadmin -u $RABBIT_MQ_USER -p $RABBIT_MQ_PASSWORD -H $RABBIT_MQ_HOST -P $RABBIT_MQ_PORT declare exchange name=$EXCHANGE_NAME type=$EXCHANGE_TYPE durable=true || echo "Exchange Exists"
  /opt/rabbitmq/cli/rabbitmqadmin -u $RABBIT_MQ_USER -p $RABBIT_MQ_PASSWORD -H $RABBIT_MQ_HOST -P $RABBIT_MQ_PORT declare queue name=$QUEUE_NAME durable=true auto_delete=false  || echo "Queue Exists"
  /opt/rabbitmq/cli/rabbitmqadmin -u $RABBIT_MQ_USER -p $RABBIT_MQ_PASSWORD -H $RABBIT_MQ_HOST -P $RABBIT_MQ_PORT declare binding source=$EXCHANGE_NAME destination=$QUEUE_NAME routing_key=$ROUTING_KEY 
  echo "Exchange, Queue, and Binding created successfully!"
fi

wait $(jobs -p)
