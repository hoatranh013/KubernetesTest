#!/bin/bash
echo "Checking Topics Available in Kafka"
# Kafka broker address
KAFKA_BROKER="kafka:9093"
# Kafka topics list command (executed inside the Kafka container)
KAFKA_TOPICS_COMMAND="docker exec kafka kafka-topics --describe --bootstrap-server $KAFKA_BROKER --topic"
# File where the topics are listed
TOPIC_LIST_FILE="/var/lib/kafka/data/kafka_topics.txt"
# Check if the topic list file exists
if [[ ! -f "$TOPIC_LIST_FILE" ]]; then
  echo "Error: Cannot find the file: $TOPIC_LIST_FILE"
  exit 1
fi
# Read the topics from the file and check each one
while IFS=' ' read -r topicname partition offset
do
  # Check if the topic exists in Kafka by describing it
  $KAFKA_TOPICS_COMMAND "$topicname" &> /dev/null

  if [ $? -eq 0 ]; then
    echo "Topic '$topicname' exists."
  else 
    echo "Topic '$topicname' does not exist."
  fi
done < "$TOPIC_LIST_FILE"