#!/bin/bash
echo "Checking Topics Available in Kafka"
KAFKA_BROKER="kafka:9093"
KAFKA_TOPICS_COMMAND="kafka-topics --list --bootstrap-server $KAFKA_BROKER"
TOPIC_LIST_FILE="/opt/KafkaSeedsVolume/kafka_topics.txt"
if [[ ! -f "$TOPIC_LIST_FILE" ]]; then
  echo "Error: Cannot find the file: $TOPIC_LIST_FILE"
  exit 1
fi
while IFS='' read -r topicname partition offset
do
  echo "$topicname"
  if $KAFKA_TOPICS_COMMAND | grep -w "$topicname" > /dev/null; then
    echo "Topic '$topicname' exists."
  else 
    echo "Topic '$topicname' does not exist."
    exit 1
  fi
done < "$TOPIC_LIST_FILE"