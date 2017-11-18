<template>
  <div id="app">
    <AppHeader />
    <div id="view-container">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
import AppHeader from "./components/AppHeader.vue";

export default {
  name: "app",
  components: {
    AppHeader
  }
};
</script>

<style>
html,
body,
#app {
  height: 100%;
  width: 100%;
  font-family: "Avenir", Helvetica, Arial, sans-serif;
}

#view-container {
  margin: 0 auto;
  height: 100%;
  padding: 10px;
  max-width: 920px;
}
</style>
