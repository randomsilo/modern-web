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
