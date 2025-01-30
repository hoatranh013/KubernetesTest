#!/bin/bash
echo "Creating Topics For Kafka"

input_file="kafka_topics.txt"
kafka_broker="kafka:9093"

if [[ ! -f "$input_file" ]]; then
    echo "File not found!"
    exit 1
fi

while IFS='~' read -r topicname partition offset
do
    echo "1 $topicname 1"
    echo "1 $partition 1"
    echo "1 $offset 1"
  /bin/kafka-topics --create --topic "$topicname" --bootstrap-server "$kafka_broker" --partitions "$partition" --replication-factor "$offset"
done < "$input_file"