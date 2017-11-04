<template>
  <div id="app">
      <AppHeader />
      <div id="view-container">
          <router-view></router-view>
      </div>
  </div>
</template>

<script>
import AppHeader from './components/AppHeader.vue'

export default {
  name: 'app',
  components: {
      AppHeader
  }
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
}
#view-container {
    text-align: center;
    color: #2c3e50;
    margin-top: 60px;
}
</style>
