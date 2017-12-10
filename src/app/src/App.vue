<template>
  <div id="app">
    <div id="view-container">
      <router-view></router-view>
    </div>
    <navigation />
  </div>
</template>

<script>
import navigation from "./components/Navigation.vue";

export default {
  name: "app",
  components: {
    navigation
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
  background: #fdf6e3;
}

#view-container {
  margin: 0 auto;
  height: 100%;
  padding: 10px;
  max-width: 920px;
}
</style>
