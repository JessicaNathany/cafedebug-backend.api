#!/bin/bash
set -e

# --- Configuration ---
STACK_NAME="cafedebug-stack"
SERVICE_NAME="cafedebug-api"
FULL_SERVICE_NAME="${STACK_NAME}_${SERVICE_NAME}"
BASE_IMAGE_NAME="ghcr.io/regisbarros/cafedebug-backend.api"

# --- Script Logic ---

# 1. Get the new image tag/version from the first argument (passed by CI/CD)
NEW_TAG=$1
if [ -z "$NEW_TAG" ]; then
    echo "Error: No new image tag provided. Usage: $0 <new_image_tag>"
    exit 1
fi

NEW_IMAGE_FULL="${BASE_IMAGE_NAME}:${NEW_TAG}"

echo "Starting rolling update for ${FULL_SERVICE_NAME} using image: ${NEW_IMAGE_FULL}"

# 2. Pull the new image locally
# This verifies the image is available and prevents the update from hanging on a pull failure.
echo "Pulling new image: ${NEW_IMAGE_FULL}"
docker pull "$NEW_IMAGE_FULL"
if [ $? -ne 0 ]; then
    echo "❌ Error: Failed to pull new image ${NEW_IMAGE_FULL}"
    exit 1
fi

# 3. Trigger the Rolling Update on the running Swarm service
# Swarm reads the deployment, health check, and rollback configuration from the service.
echo "Initiating rolling update on Swarm service..."
docker service update \
    --image "$NEW_IMAGE_FULL" \
    --with-rollback \
    --force \
    "$FULL_SERVICE_NAME"

# Check the exit status of the update command
if [ $? -eq 0 ]; then
    echo "✓ Successfully initiated rolling update"
    echo "✓ Monitor with: docker service ps ${FULL_SERVICE_NAME}"
else
    echo "❌ Error: Failed to initiate Docker Swarm service update"
    exit 1
fi

exit 0
