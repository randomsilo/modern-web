import Vue from 'vue'
import Router from 'vue-router'
import LoyalGuardLogin from '@/components/LoyalGuardLogin'
import TodoList from '@/components/TodoList'

import Landing from '../components/Landing';
import Simple from '../components/Simple';
import Table from '../components/Table';

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/login',
      name: 'LoyalGuardLogin',
      component: LoyalGuardLogin
    },
    {
      path: '/',
      name: 'Landing',
      component: Landing
    },
    {
        path: '/simple',
        name: 'Simple',
        component: Simple
    },
    {
        path: '/table',
        name: 'Table',
        component: Table
    },
    {
      path: '/todolist',
      name: 'TodoList',
      component: TodoList
  }
  ]
})