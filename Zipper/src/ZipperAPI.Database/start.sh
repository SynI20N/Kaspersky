#!/bin/bash
set -euo pipefail

# Path to secret provided by Docker Swarm secrets
MONGO_PASS_FILE="/run/secrets/mongo_root_password"

touch /tmp/logs

if [ -f "$MONGO_PASS_FILE" ]; then
    MONGO_PASS=$(cat "$MONGO_PASS_FILE")
else
    echo "MongoDB root password secret not found!" >> /tmp/logs
    exit 1
fi

mongod --port 8100 --bind_ip_all &

echo "Initializing MongoDB root user..." >> /tmp/logs

# Wait until MongoDB is ready
until [ "$(mongosh --port 8100 --eval 'db.adminCommand({ ping: 1 }).ok')" = "1" ]; do
    echo "Waiting for MongoDB to start..." >> /tmp/logs
    sleep 1
done

echo "MongoDB is ready" >> /tmp/logs

# Create api user in 'logs' database (on port 8100)
mongosh --port 8100 <<EOF
use logs;
db.createUser({
  user: "api",
  pwd: "$MONGO_PASS", 
  roles: [ { role: "readWrite", db: "logs" } ]
});
EOF

echo "Root user created successfully" >> /tmp/logs

wait