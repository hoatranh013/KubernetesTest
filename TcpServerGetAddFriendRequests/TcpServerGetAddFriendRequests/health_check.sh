#!/bin/bash
echo "Health Check For Tcp Server Get Add Friend Requests"

if ps aux | grep "TcpServerGetAddFriendRequests.dll" | grep -v grep > /dev/null; then
  echo "Process is healthy"
else
  echo "Process is unhealthy"
  exit 1
fi