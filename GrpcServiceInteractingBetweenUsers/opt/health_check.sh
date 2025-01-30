#!/bin/bash
last_line_logs=$(docker logs kubernetestest-grpcservicehealthcheck-1 | tail -n 1)
if [ "$last_line_logs" == "Serving" ]; then
  echo "The Service Is Healthy"
else
  echo "The Service Is Unhealthy"
  exit 1
fi