#!/bin/bash
echo "Health Check For Tcp Server Get Delete Friend Requests"

if ps aux | grep "TcpServerGetDeleteFriendRequests.dll" | grep -v grep > /dev/null; then
  echo "Process is healthy"
else
  echo "Process is unhealthy"
  exit 1
fi