#!/bin/bash
set -euo pipefail

# ============================================
# CafeDebug Backend API - Docker Swarm Deployment Script
# ============================================

# --- Configuration ---
STACK_NAME="cafedebug-stack"
# Note: The service name usually includes the stack name prefix
SERVICE_NAME_PART="cafedebug-api" 
FULL_SERVICE_NAME="${STACK_NAME}_${SERVICE_NAME_PART}"

# --- Helpers ---
log() { echo -e "\033[1;32m[deploy]\033[0m $*"; }
warn() { echo -e "\033[1;33m[deploy][WARN]\033[0m $*"; }
fail() { echo -e "\033[1;31m[deploy][ERROR]\033[0m $*" 1>&2; exit 1; }

# --- Load .env securely ---
if [ -f .env ]; then
    log "Loading environment variables from .env..."
    # 'set -a' automatically exports all variables defined subsequently 
    set -a
    # source the file, ignoring comments
    source <(grep -v '^#' .env | sed 's/^export //')
    set +a
else
    warn ".env file not found! Relying on existing environment variables."
fi

# IMAGE_NAME comes from .env (set by GitHub Actions or manually)
BASE_IMAGE_NAME="ghcr.io/${IMAGE_NAME}"

# --- Resolve Image Tag ---
ARG_TAG="${1:-}"
# Priority: Argument > Env Var > Fail
export IMAGE_TAG="${ARG_TAG:-${IMAGE_TAG:-}}"

if [ -z "${IMAGE_TAG}" ]; then
    fail "IMAGE_TAG is not set. Usage: ./deploy.sh <tag>"
fi

NEW_IMAGE_FULL="${BASE_IMAGE_NAME}:${IMAGE_TAG}"

log "------------------------------------------------"
log "Target Stack   : ${STACK_NAME}"
log "Target Service : ${FULL_SERVICE_NAME}"
log "Deploying Image: ${NEW_IMAGE_FULL}"
log "------------------------------------------------"

# --- Pre-flight Checks ---
# 1. Check Swarm
if ! docker info --format '{{.Swarm.LocalNodeState}}' | grep -qE 'active|locked'; then
    fail "Docker Swarm is not active. Run 'docker swarm init' first."
fi

# 2. Pull Image (Vital for minimal downtime)
log "Pulling image to ensure availability..."
docker pull "${NEW_IMAGE_FULL}" || fail "Could not pull image ${NEW_IMAGE_FULL}"

# --- Deployment ---
# We use 'stack deploy' because it handles YAML changes (ports, healthchecks, etc.)
# AND image updates simultaneously.
log "Deploying stack configuration..."

if ! docker stack deploy --with-registry-auth -c docker-compose.yml "${STACK_NAME}"; then
    fail "Stack deployment command failed."
fi

# --- Verification (The 'Wait' Loop) ---
log "Deployment submitted. Waiting for rollout to stabilize..."

# Wait for the service to converge
# Note: 'docker service update --detach=false' exists, but 'stack deploy' is async.
# We must manually verify the service state.

ATTEMPTS=0
MAX_ATTEMPTS=30 # 30 * 2s = 60 seconds wait (adjust based on your healthcheck start_period)

while [ $ATTEMPTS -lt $MAX_ATTEMPTS ]; do
    # Get the number of replicas that are actually 'running'
    # Note: docker service ps doesn't support image filter, so we check running state only
    RUNNING_TASKS=$(docker service ps "${FULL_SERVICE_NAME}" \
        --filter "desired-state=running" \
        --format "{{.CurrentState}}" | grep -c "Running" || true)
    
    # Get desired replica count
    DESIRED_REPLICAS=$(docker service inspect "${FULL_SERVICE_NAME}" --format '{{.Spec.Mode.Replicated.Replicas}}')
    
    # Check if the service image matches what we deployed
    CURRENT_IMAGE=$(docker service inspect "${FULL_SERVICE_NAME}" --format '{{.Spec.TaskTemplate.ContainerSpec.Image}}')
    
    if [ "$RUNNING_TASKS" -ge "$DESIRED_REPLICAS" ] && [[ "$CURRENT_IMAGE" == *"${IMAGE_TAG}"* ]]; then
        log "SUCCESS: Service converged with $RUNNING_TASKS/$DESIRED_REPLICAS replicas running."
        log "Deployed image: $CURRENT_IMAGE"
        docker service ps "${FULL_SERVICE_NAME}" | head -n 6
        exit 0
    fi
    
    echo -n "."
    sleep 2
    ATTEMPTS=$((ATTEMPTS+1))
done

fail "Deployment timed out. Service did not reach desired state in 60 seconds."
