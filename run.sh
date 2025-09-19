#!/bin/bash

# Function to detect the available compose command
detect_compose_command() {
    if command -v podman-compose &> /dev/null; then
        echo "podman-compose"
    elif command -v docker &> /dev/null && docker compose version &> /dev/null; then
        echo "docker compose"
    elif command -v docker-compose &> /dev/null; then
        echo "docker-compose"
    else
        echo ""
    fi
}

COMPOSE_CMD=$(detect_compose_command)

if [ -z "$COMPOSE_CMD" ]; then
    echo "Error: Neither podman-compose, docker compose, nor docker-compose found."
    echo "Please install one of them to proceed."
    exit 1
fi

echo "Using compose command: $COMPOSE_CMD"

# Define certificate path and password
# Changed CERT_DIR to be relative to the repository root
CERT_DIR="./certs"
CERT_FILE="aspnetapp.pfx"
CERT_PATH="$CERT_DIR/$CERT_FILE"
CERT_PASSWORD="your_secure_dev_password" # IMPORTANT: Use a strong password in production, or better, use a proper certificate management solution.

# Define the Swagger UI URL
SWAGGER_URL="https://localhost:7175/swagger"

# Function to wait for a URL to be accessible
wait_for_url() {
    local url="$1"
    local max_attempts=30
    local attempt=1
    local status_code

    echo "Waiting for $url to be accessible..."
    while [ $attempt -le $max_attempts ]; do
        # Added -L to follow redirects
        status_code=$(curl -k -s -L -o /dev/null -w "%{http_code}" "$url")
        if [ "$status_code" -eq 200 ]; then
            echo "$url is accessible."
            return 0
        else
            echo "Attempt $attempt/$max_attempts: $url returned status $status_code. Retrying in 2 seconds..."
            sleep 2
        fi
        attempt=$((attempt + 1))
    done

    echo "Error: $url did not become accessible after $max_attempts attempts."
    return 1
}

# Function to open a URL in the default browser
open_browser() {
    local url="$1"
    OS_NAME=$(uname -s)
    if [[ "$OS_NAME" == "Linux" ]]; then
        if command -v xdg-open &> /dev/null; then
            xdg-open "$url" &> /dev/null &
        else
            echo "xdg-open command not found. Please install it or open $url manually."
        fi
    elif [[ "$OS_NAME" == "Darwin" ]]; then
        open "$url" &> /dev/null &
    elif [[ "$OS_NAME" == CYGWIN* || "$OS_NAME" == MINGW* ]]; then
        # For Git Bash on Windows
        start "" "$url" &> /dev/null &
    else
        echo "Could not automatically open browser. Please navigate to $url manually."
    fi
}

# Function to handle starting the services
start_services() {
    echo "--- Option 1: Starting Services ---"

    # 1. Check and configure self-signed certs
    if [ ! -f "$CERT_PATH" ]; then
        echo "ASP.NET Core developer certificate not found at $CERT_PATH."
        echo "Generating developer certificate with password..."
        mkdir -p "$CERT_DIR" # Ensure directory exists
        dotnet dev-certs https -ep "$CERT_PATH" -p "$CERT_PASSWORD"
        if [ $? -eq 0 ]; then
            echo "Certificate generated successfully."
            # Set read permissions for others
            chmod o+r "$CERT_PATH"
            echo "Set read permissions for $CERT_PATH."
            
            # Detect OS for --trust command
            OS_NAME=$(uname -s)
            if [[ "$OS_NAME" == "Darwin" || "$OS_NAME" == CYGWIN* || "$OS_NAME" == MINGW* ]]; then
                echo "Attempting to trust the certificate (Windows/macOS)..."
                dotnet dev-certs https --trust
                if [ $? -eq 0 ]; then
                    echo "Certificate trusted successfully."
                else
                    echo "Failed to trust certificate. You might need to run 'dotnet dev-certs https --trust' manually."
                fi
            else
                echo "Certificate trust is typically not needed on Linux."
            fi
        else
            echo "Failed to generate developer certificate. Please check your .NET SDK installation."
            return # Return to menu instead of exiting
        fi
    else
        echo "ASP.NET Core developer certificate found."
        # Ensure read permissions are set in case they were changed
        chmod o+r "$CERT_PATH"
        echo "Ensured read permissions for $CERT_PATH."
        # Even if found, try to trust it in case it's untrusted or expired
        OS_NAME=$(uname -s)
        if [[ "$OS_NAME" == "Darwin" || "$OS_NAME" == CYGWIN* || "$OS_NAME" == MINGW* ]]; then
            echo "Attempting to ensure certificate is trusted (Windows/macOS)..."
            dotnet dev-certs https --trust
            if [ $? -eq 0 ]; then
                echo "Certificate trusted successfully."
            else
                echo "Failed to trust certificate. You might need to run 'dotnet dev-certs https --trust' manually."
            fi
        fi
    fi

    # 2. Set environment variables for the certificate in the container
    # These variables will be picked up by docker-compose and passed to the 'api' service
    export ASPNETCORE_Kestrel__Certificates__Default__Path="/home/app/.aspnet/https/$CERT_FILE"
    export ASPNETCORE_Kestrel__Certificates__Default__Password="$CERT_PASSWORD"

    # 3. Run compose up -d
    echo "Running '$COMPOSE_CMD up -d' with certificate environment variables..."
    $COMPOSE_CMD up -d
    if [ $? -eq 0 ]; then
        echo "Services started successfully in detached mode."
        echo "You can access your API (e.g., Swagger UI) at $SWAGGER_URL"
        
        # Wait for the Swagger UI to be accessible and then open it
        if wait_for_url "$SWAGGER_URL"; then
            open_browser "$SWAGGER_URL"
        fi
    else
        echo "Failed to start services. Please check the output above for errors."
    fi
}

# Function to handle stopping the services
stop_services() {
    echo "--- Option 2: Stopping Services ---"
    echo "Running '$COMPOSE_CMD down'... "
    $COMPOSE_CMD down
    if [ $? -eq 0 ]; then
        echo "Services stopped and removed successfully."
    else
        echo "Failed to stop services. Please check the output above for errors."
    fi
}

# Main menu loop
while true; do
    echo "" # Add a blank line for readability
    echo "Choose an option:"
    echo "1) Start services (configure certs if needed, then $COMPOSE_CMD up -d)"
    echo "2) Stop services ($COMPOSE_CMD down)"
    echo "q) Quit"

    read -p "Enter your choice: " choice

    case "$choice" in
        1)
            start_services
            ;;
        2)
            stop_services
            ;;
        q|Q)
            echo "Exiting."
            break # Exit the loop
            ;;
        *)
            echo "Invalid choice. Please enter 1, 2, or q."
            ;;
    esac
done
