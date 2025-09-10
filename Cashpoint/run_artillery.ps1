# Pull the Artillery image from Docker Hub
docker pull artilleryio/artillery

# Build the Docker image from the Dockerfile.artillery
docker build --no-cache -f Dockerfile_test -t artillery-image .

# Run the Docker container with volume mount
docker run -it -p 5282:5282 --rm -v "$(Get-Location).Path:/app" artillery-image