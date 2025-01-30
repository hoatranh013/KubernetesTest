#!/bin/bash
echo "Health Check For Delete Friend Notifications Table Consumers Service"

if ps aux | grep "FriendNotificationsTableConsumers.dll" | grep -v grep > /dev/null; then
  echo "Process is healthy."
else
  echo "Process is unhealthy"
  exit 1
fi