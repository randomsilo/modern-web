import Vue from 'vue';
import App from './App.vue';
import router from './router';
import LoyalGuardApi from './backend/loyalguard-api'
import TodoListApi from './backend/todolist-api'

Vue.config.productionTip = false;

import 'bootstrap'; 
import 'bootstrap/dist/css/bootstrap.css'


// Vue Application
new Vue({
  router,
  LoyalGuardApi,
  TodoListApi,
  render: h => h(App)
}).$mount('#app')