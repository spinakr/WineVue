import Vue from 'vue'
import VueRouter from 'vue-router'
import App from './App'
import InventoryList from './components/InventoryList'
import WishList from './components/WishList'
import 'font-awesome/css/font-awesome.min.css';

Vue.use(VueRouter);

const router = new VueRouter({
  routes: [{
      name: 'inventory',
      path: '/',
      component: InventoryList
    },
    {
      name: 'wishlist',
      path: '/wishlist',
      component: WishList
    }
  ]
})

new Vue({
  el: '#app',
  router,
  render(h) {
    return h('App')
  },
  components: {
    App
  }
})
