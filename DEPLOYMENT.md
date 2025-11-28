# CafeDebug Backend API - AWS EC2 Deployment Guide

## Overview

This guide covers deploying the CafeDebug API to AWS EC2 using Docker Swarm with:
- **Caddy** as reverse proxy (modern Nginx alternative)
- **Rolling updates** with zero-downtime deployments
- **Automatic rollback** on deployment failures
- **Health checks** for container readiness
- **Comprehensive logging** for error tracking
- **Secure .env file** generated from GitHub Secrets

## Architecture

```
Internet Traffic
       ↓
   [Caddy 443/80]  ← Reverse Proxy, TLS termination, load balancing
       ↓
   [Overlay Network]
       ↓
   [CafeDebug API Replicas: 1, 2, 3]  ← 3 instances for HA
       ↓
   [MySQL Database]
   [AWS S3]
```

## Prerequisites

- AWS EC2 instance (Ubuntu 20.04 LTS or later recommended)
- Docker Engine 20.10+
- Docker initialized as Swarm manager: `docker swarm init`
- GitHub Actions secrets configured (see **Required GitHub Secrets** section below)
- Production environment configured in GitHub repository settings

## Initial Setup on AWS EC2

### 1. Install Docker

```bash
# Update system
sudo apt update && sudo apt upgrade -y

# Install Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Add user to docker group
sudo usermod -aG docker $USER
newgrp docker

# Verify installation
docker --version
docker run hello-world
```

### 2. Initialize Docker Swarm

```bash
# Initialize as manager node
docker swarm init

# Output will show token for adding workers (if needed)
docker swarm join-token worker
```

### 3. Set Up Project Directory

```bash
# Create deployment directory
mkdir -p /opt/cafedebug
cd /opt/cafedebug

# NOTE: GitHub Actions will automatically copy files here during deployment
# You can optionally clone the repo for reference, but it's not required:
# git clone https://github.com/RegisBarros/cafedebug-backend.api.git
# cd cafedebug-backend.api
```

**Important:** The `.env` file will be automatically generated from GitHub Secrets during deployment. You do NOT need to manually create it on the EC2 instance.

### 4. Configure GitHub Secrets

In your GitHub repository, navigate to **Settings → Secrets and variables → Actions** and add the following secrets:

#### AWS/EC2 Configuration
- `AWS_EC2_HOST` - EC2 instance IP or hostname
- `AWS_EC2_USER` - SSH username (e.g., `ubuntu`)
- `AWS_EC2_SSH_KEY` - Private SSH key for EC2 access
- `AWS_EC2_PORT` - SSH port (default: `22`)
- `DOCKER_COMPOSE_PATH` - Deployment directory path (e.g., `/opt/cafedebug`)

#### Application Configuration
- `CADDY_DOMAIN` - Production domain (e.g., `api.cafedebug.com.br`)
- `DB_CONNECTION` - MySQL connection string (e.g., `Server=host;Port=3306;Database=db;User=user;Password=pass`)

#### JWT Configuration
- `JWT_ISSUER` - JWT token issuer URL (e.g., `https://api.cafedebug.com.br`)
- `JWT_AUDIENCE` - JWT token audience URL (e.g., `https://cafedebug.com.br`)
- `JWT_SIGNING_KEY` - Secret key for JWT signing (minimum 32 characters)

#### AWS S3 Storage
- `AWS_S3_BUCKET` - S3 bucket name
- `AWS_S3_REGION` - AWS region (e.g., `us-east-1`)
- `AWS_ACCESS_KEY_ID` - AWS access key
- `AWS_SECRET_ACCESS_KEY` - AWS secret key
- `AWS_S3_BASE_URL` - S3 base URL (e.g., `https://s3.amazonaws.com/bucket-name`)

#### Email/SMTP Configuration
- `SMTP_SERVER` - SMTP server hostname
- `SMTP_PORT` - SMTP port (default: `587`)
- `SMTP_USERNAME` - SMTP username
- `SMTP_PASSWORD` - SMTP password
- `SMTP_FROM_EMAIL` - From email address

### 5. Create GitHub Environment

In your GitHub repository:

1. Navigate to **Settings → Environments**
2. Click **New environment**
3. Name it `production`
4. (Optional) Add required reviewers for deployment approval
5. Click **Configure environment**

This enables the deployment workflow to use the `production` environment protection rules.

### 6. Initial Deployment

The first deployment will be triggered automatically when you publish a GitHub release:

1. Go to your repository → **Releases** → **Create a new release**
2. Choose a tag (e.g., `v1.0.0`)
3. Write release notes
4. Click **Publish release**
5. GitHub Actions will automatically:
   - Generate `.env` from your secrets
   - Copy files to EC2 (docker-compose.yml, Caddyfile, scripts)
   - Execute `deploy.sh` with the new image tag
   - Verify deployment

**Monitor the deployment:**
```bash
# SSH to EC2 and check status
ssh -i your-key.pem ubuntu@your-ec2-ip
docker service ls
docker service ps cafedebug-stack_cafedebug-api
```

## Monitoring and Logs

### View Service Status

```bash
# List all services
docker service ls

# View service tasks (replicas)
docker service ps cafedebug-stack_cafedebug-api

# Get detailed service info
docker service inspect cafedebug-stack_cafedebug-api
```

### View Logs

```bash
# Real-time logs from all replicas
docker service logs -f cafedebug-stack_cafedebug-api

# Logs from Caddy (reverse proxy)
docker service logs -f cafedebug-stack_caddy

# Tail last 100 lines
docker service logs --tail=100 cafedebug-stack_cafedebug-api

# JSON formatted logs
docker service logs --timestamps cafedebug-stack_cafedebug-api | grep "ERROR"
```

### Check Container Health

```bash
# Check healthcheck status
docker ps --filter "name=cafedebug-api" --format "table {{.ID}}\t{{.Status}}\t{{.Names}}"

# Inspect specific container health
docker inspect --format='{{.State.Health.Status}}' <container_id>

# View health history
docker inspect --format='{{range .State.Health.Log}}{{.Output}}{{end}}' <container_id>
```

## Deployment Process

### Via GitHub Actions (Recommended)

The deployment is fully automated via the `deploy-on-release.yml` workflow:

1. **Merge changes to main branch** → Triggers CI/CD pipeline
   - Builds Docker image
   - Runs tests
   - Pushes image to GHCR with tag `main-{commit-sha}`
   - Creates a DRAFT release

2. **Publish the draft release** → Triggers deployment workflow
   - GitHub Actions generates `.env` from secrets
   - Copies infrastructure files to EC2 (docker-compose.yml, Caddyfile, scripts/)
   - Executes `scripts/deploy.sh` with the new image tag
   - Swarm performs rolling update (3 replicas, one at a time)
   - Verifies deployment and shows logs
   - Automatic rollback on failure

3. **Monitor deployment in Actions tab**
   - Real-time logs
   - Service status
   - Replica health

### Manual Deployment (Emergency/Testing)

```bash
# SSH into EC2
ssh -i your-key.pem ubuntu@your-ec2-ip

cd /opt/cafedebug

# Ensure you have the required files
# (docker-compose.yml, Caddyfile, .env, scripts/deploy.sh)

# Deploy a specific image tag
bash scripts/deploy.sh main-abc1234

# Dry-run to see what would happen
bash scripts/deploy.sh main-abc1234 --dry-run
```

## Rolling Update Strategy

The `docker-compose.yml` defines:

```yaml
update_config:
  parallelism: 1           # Update 1 replica at a time
  delay: 15s               # Wait 15s between updates (app startup time)
  failure_action: rollback # Automatic rollback on failure
  order: start-first       # Start new → kill old (zero-downtime)
```

### Update Flow

1. **Start new replica** with new image
2. **Wait 15 seconds** for container to stabilize and app to start
3. **Health check passes** (3 consecutive healthy checks required)
   - Healthcheck runs every 30s with 40s start period
   - Retries 3 times before marking as unhealthy
4. **Remove old replica** once new one is healthy
5. **Repeat** for next replica (3 total replicas)
6. **All replicas** updated = deployment complete

**Total update time:** ~3 minutes for 3 replicas (15s delay + health checks per replica)

### If a replica fails:

- Health check fails after 3 retries (90s total)
- Swarm automatically triggers rollback with `--with-rollback`
- All replicas revert to previous image
- Deployment marked as failed

## Rollback

### Automatic Rollback

Automatically triggered if:
- Health check fails
- Container exits with error code
- Deployment times out

### Manual Rollback

Use GitHub Actions rollback workflow:

```bash
# Navigate to Actions → Rollback Workflow
# Fill in previous image tag (e.g., main-previous1234)
# Workflow SSHes to EC2 and runs deploy.sh with old tag
```

Or manually via SSH:

```bash
ssh -i your-key.pem ubuntu@your-ec2-ip
cd /opt/cafedebug/cafedebug-backend.api

# Get previous image tag from Docker
docker service inspect cafedebug-stack_cafedebug-api \
  --format='{{.Spec.TaskTemplate.ContainerSpec.Image}}'

# Rollback
./scripts/deploy.sh <previous-image-tag>
```

## Scaling

### Scale API Replicas

```bash
# Increase to 5 replicas
docker service scale cafedebug-stack_cafedebug-api=5

# Decrease to 2 replicas
docker service scale cafedebug-stack_cafedebug-api=2

# View updated service
docker service ps cafedebug-stack_cafedebug-api
```

## Troubleshooting

### Service won't start

```bash
# Check logs for errors
docker service logs cafedebug-stack_cafedebug-api | tail -50

# Verify image is available
docker image ls | grep cafedebug

# Check environment variables
docker service inspect cafedebug-stack_cafedebug-api \
  --format='{{json .Spec.TaskTemplate.ContainerSpec.Env}}'
```

### Health check failures

```bash
# Manually test health endpoint
curl -f http://localhost/health

# Check container is listening on port 80
docker ps -a --format "table {{.Names}}\t{{.Ports}}"

# Exec into container and test
docker exec <container_id> curl -f http://localhost:80/health
```

### High CPU/Memory usage

```bash
# Check resource usage
docker stats cafedebug-stack_cafedebug-api

# Check current limits
docker service inspect cafedebug-stack_cafedebug-api \
  --format='{{.Spec.TaskTemplate.Resources}}'

# Adjust limits if needed (requires service recreation)
```

### Network issues

```bash
# Inspect overlay network
docker network ls
docker network inspect cafedebug-backend.api_cafedebug-network

# Test connectivity between services
docker exec <caddy_container> ping -c 3 cafedebug-api
```

## Maintenance

### View stored logs

```bash
# Docker stores logs in JSON format
sudo tail -f /var/lib/docker/containers/<container_id>/<container_id>-json.log

# Parse JSON logs
sudo tail -f /var/lib/docker/containers/<container_id>/<container_id>-json.log | jq '.log'
```

### Prune unused resources

```bash
# Remove unused volumes, networks, images
docker system prune

# Include stopped containers
docker system prune -a

# Include volumes (WARNING: deletes data)
docker system prune -a --volumes
```

### Update Caddy/API image

```bash
# Pull latest images
docker pull caddy:2.7-alpine
docker pull ghcr.io/regisbarros/cafedebug-backend.api:latest

# Recreate services with new images
docker stack up -c docker-compose.yml cafedebug-stack
```

## Security Best Practices

1. **Use environment variables** for all secrets (do not hardcode)
2. **Enable TLS/HTTPS** in Caddyfile (automatic HTTPS recommended)
3. **Run containers as non-root** (already configured in Dockerfile)
4. **Set resource limits** to prevent DoS (configured in docker-compose.yml)
5. **Regular backups** of database and uploaded files
6. **Monitor logs** for suspicious activity
7. **Keep Docker updated** with security patches
8. **Use private container registry** (GitHub Container Registry configured)

## Alternatives to Caddy (if needed)

### Nginx
- More mature, larger community
- More configuration options
- Higher memory footprint
- Setup: `nginx:latest` image + nginx.conf volume mount

### Traefik
- Native Docker/Swarm integration
- Automatic service discovery
- Built-in middleware
- More complex configuration

### HAProxy
- Lightweight, high-performance
- Advanced load balancing
- Lower-level configuration

**Why Caddy is recommended:**
- Simpler configuration syntax
- Automatic HTTPS
- JSON logging (better for Docker)
- Built-in health checks
- Great Docker Swarm support

## Additional Resources

- [Docker Swarm Documentation](https://docs.docker.com/engine/swarm/)
- [Caddy Documentation](https://caddyserver.com/docs/)
- [Docker Service Update Reference](https://docs.docker.com/engine/reference/commandline/service_update/)
- [Health Checks Guide](https://docs.docker.com/compose/compose-file/#healthcheck)
