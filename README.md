# modern-web

The modern web application is a proving ground for new technology that allows me to build solutions to real world issues.
These pieces are parts will be used later in other applications.
The main goal is build better solutions faster.

Brash is a tool for building ASP Core 3 RESTful web services extremely fast.
Brash uses a JSON file to generate domain driven design api.
Multiple JSON files can be used to build and maintain different domains.
Each domain is hosted on a different port.

NGINX and CertBot are used to expose the domain over https.

Frontends use the API to access and modify data.
The first frontend to be developed will be Vue3; because, it is web responsive.
The next round of frontend work will be focused on native mobile (xamarin android).
The last round will be a WPF thick client.

## Conventions

This git repository is a master repository; that means it holds all related projects together.
Some people prefer to separate projects into different git repositories.
I like to keep related projects together.
If you change an api route and it could break the web application, an IoT application, and the mobile client.
A master repository allows the developer to search through all source to discover code connections.

## Frontends

The three basic frontend types are web, mobile, and thick client.
Responsive web, progressive web applications, and hybrid web applications are quickly become the standard interface.
Mobile applications are required when the phone's technology is essential to the user experience; like the camera, GPS, SMS, and contact list.

### modern-web-vue (Vue.js 3)

* Install Node 13
* Install Node Package Manager
* Install Vue3 Commandline Interface

```bash
sudo apt-get remove nodejs
sudo apt-get remove npm

curl -sL https://deb.nodesource.com/setup_13.x | sudo bash -
sudo apt-get install nodejs
sudo apt-get install npm

sudo npm install -g @vue/cli

sudo rm /usr/local/bin/vue
sudo ln -s /usr/bin/vue /usr/local/bin/vue
```

```bash
cd frontends
vue create modern-web-vue

cd modern-web-vue
npm install --save vue-router
npm install --save axios vue-axios
npm install --save bootstrap-vue portal-vue bootstrap jquery popper.js
npm run serve
```

### Xamarin Android

On hold for now.

### WPF Client Application

On hold for now.

## Backends

Backend code is composed of different domain driven design apis, IoT applications (python), and commandline applications.
IoT and commandline applications will use Brash apis to coordinate data through the web.

### Loyal Guard

Loyal Guard is a authentication and authorization.
It is meant to be a simple user name and password system with role assignment.

### Install Brash

```bash
# clone brash into a peer directory.  Do not put Brash in your project.
git clone https://github.com/randomsilo/brash.git

cd ./brash/Brash
dotnet build

cd ../brash/brashcli
dotnet build
```

### Create LoyalGuard

The commands for creating the LoyalGuard domain api are below.
If you have cloned this application then the pieces should already be there.
The commands below will only be useful if you are modifying LoyalGuard's functionality.
The url, port, and other configuration entries can be changed in the BrashConfiguration.cs.

Open two terminals:  
One for brashcli commands starting in the brashcli directory outside of the application.
Another terminal for project based commands.

* Change the paths to match your own directory structure.
* Change the url to match your dns/certbot entries.

```bash
mkdir -p /shop/randomsilo/modern-web/backends/LoyalGuard

# from brashcli directory
dotnet run project-init -n LoyalGuard -d /shop/randomsilo/modern-web/backends/LoyalGuard
dotnet run data-init -n LoyalGuard -d /shop/randomsilo/modern-web/backends/LoyalGuard

# make c# projects
cd /shop/randomsilo/modern-web/backends/LoyalGuard
. ./init.sh

# copy structure.json to loyalguard.json
# make appropriate changes 

# from brashcli directory
dotnet run sqlite-gen --file /shop/randomsilo/modern-web/backends/LoyalGuard/loyalguard.json

# combine sql scripts
cd /shop/randomsilo/modern-web/backends/LoyalGuard/sql/sqlite
. ./combine.sh

# from brashcli directory
dotnet run cs-domain --file /shop/randomsilo/modern-web/backends/LoyalGuard/loyalguard.json
dotnet run cs-repo-sqlite --file /shop/randomsilo/modern-web/backends/LoyalGuard/loyalguard.json
dotnet run cs-xtest-sqlite --file /shop/randomsilo/modern-web/backends/LoyalGuard/loyalguard.json

dotnet run cs-api-sqlite --file /shop/randomsilo/modern-web/backends/LoyalGuard/loyalguard.json \
--user API_LOYALGUARD \
--pass API_TWO_IF_BY_SEA \
--port 6100 \
--dev-site http://localhost:8080 \
--web-site https://modernwebvue.ctrlshiftesc.com

```

### Build LoyalGuard

```bash
cd /shop/randomsilo/modern-web/backends/LoyalGuard/
cd LoyalGuard.Domain
dotnet build
cd ..

cd LoyalGuard.Infrastructure
dotnet build
cd ..

cd LoyalGuard.Infrastructure.Test
dotnet build
dotnet test
cd ..

cd LoyalGuard.Api
dotnet build
dotnet run

#ctrl c to quit

```