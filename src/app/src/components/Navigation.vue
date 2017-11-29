<template>
  <div id="footer">
    <router-link to="/">
      <i class="fa fa-home fa-3x nav-item"></i>
    </router-link>
    <router-link to="wishlist">
      <i class="fa fa-list fa-3x nav-item"></i>
    </router-link>
    <router-link to="archive">
      <i class="fa fa-archive fa-3x nav-item"></i>
    </router-link>
  </div>
</template>

<script>
export default {
  name: "Navigation",
  data() {
    return {
      msg: "This is header text"
    };
  }
};
</script>

<style scoped>
#footer {
  position: fixed;
  left: 0px;
  bottom: 0px;
  height: 60px;
  width: 100%;
  display: flex;
  justify-content: center;
  align-items: center;

  background-color: rgba(117, 190, 218, 0.5);
}

.nav-item {
  padding: 5px;
}
</style>
