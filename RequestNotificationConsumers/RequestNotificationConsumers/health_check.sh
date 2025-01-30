#!/bin/bash
echo "Health Check For Request Notification Consumers Service"

if ps aux | grep "RequestNotificationConsumers.dll" | grep -v grep > /dev/null; then
  echo "Process is healthy."
else
  echo "Process is unhealthy"
  exit 1
fi