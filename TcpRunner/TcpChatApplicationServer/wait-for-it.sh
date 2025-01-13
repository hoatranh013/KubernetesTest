#!/bin/bash
# wait-for-it.sh
# Example: ./wait-for-it.sh <host>:<port> -- <command>

HOST=$1
PORT=$2
shift 2
cmd="$@"

# Wait for the host:port to be reachable
until nc -z -v -w30 $HOST $PORT
do
  echo "Waiting for $HOST:$PORT..."
  sleep 1
done

echo "$HOST:$PORT is up, executing command"
exec $cmd