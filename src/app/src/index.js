import Vue from "vue";
import VueRouter from "vue-router";
import App from "./App";
import InventoryList from "./containers/InventoryList";
import WishList from "./containers/WishList";
import ArchiveList from "./containers/ArchiveList";
import WineDetails from "./containers/WineDetails";
import "font-awesome/css/font-awesome.min.css";

Vue.use(VueRouter);

const router = new VueRouter({
  routes: [
    {
      name: "wine-info",
      path: "/:id",
      component: WineDetails,
      props: true
    },
    {
      name: "inventory",
      path: "/",
      component: InventoryList
    },
    {
      name: "wishlist",
      path: "/wishlist",
      component: WishList
    },
    {
      name: "archive",
      path: "/archive",
      component: ArchiveList
    }
  ]
});

new Vue({
  el: "#app",
  router,
  render(h) {
    return h("App");
  },
  components: {
    App
  }
});
