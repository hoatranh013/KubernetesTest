#!/bin/bash 
su
apt-get install sudo
sudo apt-get update
sudo apt install -y netcat-openbsd
KAFKA_HOST="kafka"
KAFKA_PORT=9093
sleep 5
until nc -zv $KAFKA_HOST $KAFKA_PORT; do
  sleep 5
  echo "Waiting for Kafka running..."
done
echo "Kafka is up and running"
dotnet CreateTopicsKafkaScripts.dll