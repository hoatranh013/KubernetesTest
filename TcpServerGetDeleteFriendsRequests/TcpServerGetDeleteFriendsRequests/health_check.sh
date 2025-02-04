#!/bin/bash
echo "Health Check For Tcp Server Get Delete Friends Requests"

if ps aux | grep "TcpServerGetDeleteFriendsRequests.dll" | grep -v grep > /dev/null; then
  echo "Process is healthy"
else
  echo "Process is unhealthy"
  exit 1
fi