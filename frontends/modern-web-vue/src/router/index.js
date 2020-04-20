import Vue from 'vue'
import Router from 'vue-router'

import Landing from '@/components/Landing';
import PrivacyPolicy from '@/components/PrivacyPolicy';
import TermsOfService from '@/components/TermsOfService';

import LoyalGuardLogin from '@/components/LoyalGuard/Login'
import TodoList from '@/components/Todo/List'



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
      path: '/PrivacyPolicy',
      name: 'PrivacyPolicy',
      component: PrivacyPolicy
    },
    {
      path: '/TermsOfService',
      name: 'TermsOfService',
      component: TermsOfService
    },
    {
      path: '/Todolist',
      name: 'TodoList',
      component: TodoList
  }
  ]
})