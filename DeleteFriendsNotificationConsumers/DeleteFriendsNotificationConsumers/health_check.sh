#!/bin/bash
echo "Health Check For Delete Friends Notification Consumers Service"

if ps aux | grep "DeleteFriendsNotificationConsumers.dll" | grep -v grep > /dev/null; then
  echo "Process is healthy."
else
  echo "Process is unhealthy"
  exit 1
fi