# MovieNight 

ASP.NET Core MVC web application to suggest and vote on movies to watch together.

## Requirements
- MariaDB/MySQL database
- SMTP Credentials/Server
- ASP.NET Core 6 Runtime installed
- [OMDb API Key](https://www.omdbapi.com/)

## Usage

- Clone the repo
- Navigate to the `MovieNight` project folder
- Edit `appsettings.json` and enter your OMDb API Key, SMTP credentials and MySQL/MariaDB database connection
- Migrate the DB with `dotnet ef database update`
- Build the project with `dotnet publish -c Release -o /path/to/built/app`
- Run the app within the output directory with `./MovieNight` (Windows may differ)
- Optionally create a systemd service and reverse proxy as shown below

## Linux systemd Service

To create a systemd service and run the application on boot, create a service file, for example under
`/etc/systemd/system/movienight.service`.

Service file contents:
```
[Unit]
Description=MovieNight .NET Core MVC Application

[Service]
Type=notify
WorkingDirectory=/opt/movienight
ExecStart=/usr/bin/dotnet /opt/movienight/MovieNight.dll
SyslogIdentifier=MovieNight

User=YOUR-USER

Restart=always
RestartSec=5

KillSignal=SIGINT
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=DOTNET_ROOT=/home/YOUR-USERE/.dotnet/

[Install]
WantedBy=multi-user.target
```

Edit `WorkingDirectory`, `ExecStart`, `User` and `DOTNET_ROOT` according to your installation.
Don't run this as root. Make sure your `User` has access to the `WorkingDirectory`.

```
sudo systemctl daemon-reload
sudo systemctl start movienight.service
sudo systemctl status movienight
```

If all is well, enable the service to be started on boot:

`sudo systemctl enable movienight`

## NGINX Reverse Proxy

Shown below is an example of an NGINX + LetsEncrypt reverse proxy config for this application.

```
server {
    listen                  443 ssl http2;
    listen                  [::]:443 ssl http2;
    server_name             movienight.yourdomain.com;

    # SSL
    ssl_certificate         /etc/letsencrypt/live/movienight.yourdomain.com/fullchain.pem;
    ssl_certificate_key     /etc/letsencrypt/live/movienight.yourdomain.com/privkey.pem;
    ssl_trusted_certificate /etc/letsencrypt/live/movienight.yourdomain.com/chain.pem;

    # security headers
    add_header X-Frame-Options           "DENY";
    add_header X-XSS-Protection          "1; mode=block" always;
    add_header X-Content-Type-Options    "nosniff" always;
    add_header Referrer-Policy           "no-referrer-when-downgrade" always;
    add_header Content-Security-Policy   "default-src 'self' http: https: data: blob: 'unsafe-inline'" always;
    add_header Strict-Transport-Security 'max-age=31536000; includeSubDomains; preload';
    add_header X-Permitted-Cross-Domain-Policies master-only;


    # . files
    location ~ /\.(?!well-known) {
        deny all;
    }

    # logging
    access_log              /var/log/nginx/movienight.yourdomain.com.access.log;
    error_log               /var/log/nginx/movienight.yourdomain.com.error.log warn;

    # reverse proxy
    location / {
        proxy_pass http://127.0.0.1:5000;
        proxy_http_version                 1.1;
        proxy_cache_bypass                 $http_upgrade;

        # Proxy headers
        proxy_set_header Upgrade           $http_upgrade;
        proxy_set_header Connection        "upgrade";
        proxy_set_header Host              $host;
        proxy_set_header X-Real-IP         $remote_addr;
        proxy_set_header X-Forwarded-For   $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_set_header X-Forwarded-Host  $host;
        proxy_set_header X-Forwarded-Port  $server_port;

        # Proxy timeouts
        proxy_connect_timeout              60s;
        proxy_send_timeout                 60s;
        proxy_read_timeout                 60s;
    }
}

# HTTP redirect
server {
    listen      80;
    listen      [::]:80;
    server_name movienight.yourdomain.com;

    # ACME-challenge
    location ^~ /.well-known/acme-challenge/ {
        root /var/www/_letsencrypt;
    }

    location / {
        return 301 https://movienight.yourdomain.com$request_uri;
    }
}
```
